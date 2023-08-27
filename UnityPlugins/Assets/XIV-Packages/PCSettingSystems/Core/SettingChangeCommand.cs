using System;
using System.Collections.Generic;
using UnityEngine.Pool;

namespace XIV_Packages.PCSettingsSystem
{
    public struct SettingChange
    {
        public int index;
        public Type settingType;
        public ISetting from;
        public ISetting to;

        public SettingChange reversed => new SettingChange(index, settingType, to, from);

        public SettingChange(int index, ISetting from, ISetting to) : this(index, from.GetType(), from, to)
        {

        }

        public SettingChange(int index, Type settingType, ISetting from, ISetting to)
        {
            this.index = index;
            this.settingType = settingType;
            this.from = from;
            this.to = to;
        }

        public void Reverse()
        {
            var temp = to;
            to = from;
            from = temp;
        }
    }

    public struct SettingChangeCommand : ICommand
    {
        public readonly int index;
        public readonly ISetting setting;

        public readonly Type settingType;
        public readonly Dictionary<Type, SettingChange> unappliedSettings;
        public readonly UndoRedoStack<ICommand> commandOwner;
        public readonly IList<ISettingListener> listeners;
        public readonly IList<ISetting> settings;
        public readonly IList<SettingPreset> presets;
        SettingChange presetChange;
        SettingChange settingChange;
        bool isPresetChanged;

        public SettingChangeCommand(UndoRedoStack<ICommand> commandOwner, int index, Dictionary<Type, SettingChange> unappliedSettings, ISetting newValue,
            IList<ISettingListener> listeners, IList<ISetting> settings, IList<SettingPreset> presets)
        {
            this.commandOwner = commandOwner;
            this.index = index;
            this.unappliedSettings = unappliedSettings;
            this.settingType = newValue.GetType();
            this.setting = newValue;
            this.listeners = listeners;
            this.settings = settings;
            this.presets = presets;
            presetChange = default;
            settingChange = default;
            isPresetChanged = false;
        }

        void ICommand.Execute()
        {
            bool containsKey = unappliedSettings.ContainsKey(settingType);
            var currentValue = containsKey ? unappliedSettings[settingType].to : settings[index];
            settingChange = new SettingChange(index, currentValue, setting);

            if (containsKey) unappliedSettings[settingType] = settingChange;
            else unappliedSettings.Add(settingType, settingChange);

            InformSettingChange(settingChange);

            presetChange = ResolvePreset(out isPresetChanged);
            if (isPresetChanged == false) return;

            Type settingPresetType = typeof(SettingPreset);
            if (unappliedSettings.ContainsKey(settingPresetType)) unappliedSettings[settingPresetType] = presetChange;
            else unappliedSettings.Add(settingPresetType, presetChange);

            InformSettingChange(presetChange);
        }

        void ICommand.Unexecute()
        {
            settingChange.Reverse();
            unappliedSettings[settingType] = settingChange;

            InformSettingChange(settingChange);

            if (isPresetChanged == false) return;

            Type settingPresetType = typeof(SettingPreset);

            presetChange.Reverse();
            unappliedSettings[settingPresetType] = presetChange;
            InformSettingChange(presetChange);
        }

        SettingChange ResolvePreset(out bool isChanged)
        {
            Type settingPresetType = typeof(SettingPreset);
            bool containsKey = unappliedSettings.ContainsKey(settingPresetType);
            var presetIndex = IndexOfSetting(settingPresetType, settings);

            var currentPreset = (SettingPreset)(containsKey ? unappliedSettings[settingPresetType].to : settings[presetIndex]);

            var dic = DictionaryPool<Type, ISetting>.Get();
            foreach (SettingChange unappliedSetting in unappliedSettings.Values)
            {
                dic.Add(unappliedSetting.settingType, unappliedSetting.to);
            }

            foreach (var setting in settings)
            {
                dic.TryAdd(setting.GetType(), setting);
            }

            dic.Remove(settingPresetType);

            var correspondingPresetIndex = GetCorrespondingPresetIndex(dic.Values);

            SettingChange change;
            if (correspondingPresetIndex == -1)
            {
                var newPreset = new SettingPreset(SettingQualityLevel.Custom, new List<ISetting>(dic.Values));
                change = new SettingChange(presetIndex, currentPreset, newPreset);
                isChanged = true;
            }
            else
            {
                var newPreset = presets[correspondingPresetIndex];
                if (SettingPreset.IsEqual(newPreset, currentPreset.presetSettings))
                {
                    change = default;
                    isChanged = false;
                }
                else
                {
                    change = new SettingChange(presetIndex, currentPreset, newPreset);
                    isChanged = true;
                }
            }
            DictionaryPool<Type, ISetting>.Release(dic);
            return change;
        }

        void InformSettingChange(SettingChange settingChange)
        {
            int count = listeners.Count;
            for (int i = 0; i < count; i++)
            {
                listeners[i].OnSettingChanged(settingChange);
            }
        }

        static int IndexOfSetting(Type type, IList<ISetting> settings)
        {
            int count = settings.Count;
            for (int i = 0; i < count; i++)
            {
                if (settings[i].GetType() == type) return i;
            }
            return -1;
        }

        int GetCorrespondingPresetIndex(IEnumerable<ISetting> settings)
        {
            int count = presets.Count;
            for (int i = 0; i < count; i++)
            {
                if (SettingPreset.IsEqual(presets[i], settings))
                {
                    return i;
                }
            }

            return -1;
        }

        public override string ToString()
        {
            return settingType.Name;
        }
    }

    public readonly struct ApplySettingChangesCommand : ICommand
    {
        public readonly ISettingContainer container;
        public readonly ISettingApplier settingApplier;
        public readonly IList<ISettingListener> listeners;
        public readonly IList<ISetting> settings;
        public readonly Dictionary<Type, SettingChange> unappliedChanges;

        public readonly List<SettingChange> changes;

        public ApplySettingChangesCommand(ISettingContainer container, ISettingApplier settingApplier, IList<ISettingListener> listeners, IList<ISetting> settings,
            Dictionary<Type, SettingChange> unappliedChanges)
        {
            this.container = container;
            this.settingApplier = settingApplier;
            this.listeners = listeners;
            this.settings = settings;
            this.unappliedChanges = unappliedChanges;
            changes = new List<SettingChange>();
        }

        void ICommand.Execute()
        {
            InformBeforeApply();

            foreach (var settingChange in unappliedChanges.Values)
            {
                changes.Add(settingChange);
                settings[settingChange.index] = settingChange.to;
            }

            unappliedChanges.Clear();
            settingApplier.Apply(container);

            InformAfterApply();
        }

        void ICommand.Unexecute()
        {
            InformBeforeApply();

            int count = changes.Count;
            for (int i = 0; i < count; i++)
            {
                var change = changes[i];
                change.Reverse();
                settings[change.index] = change.to;
                unappliedChanges.Add(change.settingType, change);
                InformSettingChange(change);
            }

            changes.Clear();
            settingApplier.Apply(container);

            InformAfterApply();
        }

        void InformBeforeApply()
        {
            int count = listeners.Count;
            for (int i = 0; i < count; i++)
            {
                listeners[i].OnBeforeApply(container);
            }
        }

        void InformSettingChange(SettingChange settingChange)
        {
            int count = listeners.Count;
            for (int i = 0; i < count; i++)
            {
                listeners[i].OnSettingChanged(settingChange);
            }
        }

        void InformAfterApply()
        {
            int count = listeners.Count;
            for (int i = 0; i < count; i++)
            {
                listeners[i].OnAfterApply(container);
            }
        }
    }

    public struct ChangeSettingPresetCommand : ICommand
    {
        int index;
        UndoRedoStack<ICommand> commandOwner;
        Dictionary<Type, SettingChange> unappliedSettings;
        IList<ISettingListener> listeners;
        SettingPreset preset;
        IList<ISetting> settings;
        SettingChange previousPresetChange;

        public ChangeSettingPresetCommand(int index, Dictionary<Type, SettingChange> unappliedSettings, IList<ISettingListener> listeners,
            SettingPreset preset, IList<ISetting> settings, UndoRedoStack<ICommand> commandOwner)
        {
            this.index = index;
            this.commandOwner = commandOwner;
            this.unappliedSettings = unappliedSettings;
            this.listeners = listeners;
            this.preset = preset;
            this.settings = settings;
            previousPresetChange = default;
        }

        void ICommand.Execute()
        {
            int count = preset.presetSettings.Count;
            for (int i = 0; i < count; i++)
            {
                ISetting setting = preset.presetSettings[i];
                var type = setting.GetType();
                var hasKey = unappliedSettings.ContainsKey(type);
                var settingIndex = IndexOfSetting(type, settings);
                var currentSetting = (hasKey ? unappliedSettings[type].to : settings[settingIndex]);
                var change = new SettingChange(settingIndex, currentSetting, setting);

                if (hasKey) unappliedSettings[type] = change;
                else unappliedSettings.Add(type, change);

                InformSettingChange(change);
            }

            Type settingPresetType = typeof(SettingPreset);
            bool containsKey = unappliedSettings.ContainsKey(settingPresetType);
            var previousPreset = (SettingPreset)(containsKey ? unappliedSettings[settingPresetType].to : settings[index]);
            previousPresetChange = new SettingChange(index, previousPreset, preset);

            if (containsKey) unappliedSettings[settingPresetType] = previousPresetChange;
            else unappliedSettings.Add(settingPresetType, previousPresetChange);

            InformSettingChange(previousPresetChange);
        }

        void ICommand.Unexecute()
        {
            previousPresetChange.Reverse();
            var preset = (SettingPreset)previousPresetChange.to;
            int count = preset.presetSettings.Count;
            for (int i = 0; i < count; i++)
            {
                ISetting setting = preset.presetSettings[i];
                var type = setting.GetType();
                var settingIndex = IndexOfSetting(type, settings);
                var change = new SettingChange(settingIndex, unappliedSettings[type].to, setting);

                unappliedSettings[type] = change;
                InformSettingChange(change);
            }

            Type settingPresetType = typeof(SettingPreset);
            unappliedSettings[settingPresetType] = previousPresetChange;

            InformSettingChange(previousPresetChange);
        }

        void InformSettingChange(SettingChange settingChange)
        {
            int count = listeners.Count;
            for (int i = 0; i < count; i++)
            {
                listeners[i].OnSettingChanged(settingChange);
            }
        }

        static int IndexOfSetting(Type type, IList<ISetting> settings)
        {
            int count = settings.Count;
            for (int i = 0; i < count; i++)
            {
                if (settings[i].GetType() == type) return i;
            }
            return -1;
        }
    }
}