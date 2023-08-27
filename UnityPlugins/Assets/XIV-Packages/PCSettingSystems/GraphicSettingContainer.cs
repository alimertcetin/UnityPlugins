using System;
using System.Collections.Generic;

namespace XIV_Packages.PCSettingsSystem
{
    public class GraphicSettingContainer : ISettingContainer
    {
        public int presetCount => presets.Count;

        List<SettingPreset> presets;
        List<ISetting> settings;
        List<ISettingListener> listeners;
        Dictionary<Type, SettingChange> unappliedSettings;
        ISettingApplier settingApplier;
        public UndoRedoStack<ICommand> commands;

        public GraphicSettingContainer(ISettingApplier settingApplier, SettingPreset preset)
        {
            this.settingApplier = settingApplier;
            presets = new List<SettingPreset>();
            settings = new List<ISetting>();
            listeners = new List<ISettingListener>();
            unappliedSettings = new Dictionary<Type, SettingChange>();
            commands = new UndoRedoStack<ICommand>();

            presets.Add(preset);
            settings.Add(preset);
            int count = preset.presetSettings.Count;
            for (int i = 0; i < count; i++)
            {
                settings.Add(preset.presetSettings[i]);
            }
        }

        public void AddPreset(SettingPreset preset)
        {
            int count = presets.Count;
            for (int i = 0; i < count; i++)
            {
                if (SettingPreset.IsEqual(preset, presets[i].presetSettings))
                {
                    return;
                }
            }
            presets.Add(preset);
        }

        public bool RemovePreset(SettingPreset preset)
        {
            int count = presets.Count;
            for (int i = 0; i < count; i++)
            {
                if (SettingPreset.IsEqual(preset, presets[i].presetSettings))
                {
                    presets.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        public void ChangePreset(SettingPreset preset)
        {
            Type settingPresetType = typeof(SettingPreset);
            var index = IndexOfSetting(settingPresetType);
            bool containsKey = unappliedSettings.ContainsKey(settingPresetType);
            var currentPreset = (SettingPreset)(containsKey ? unappliedSettings[settingPresetType].to : settings[index]);

            if (SettingPreset.IsEqual(preset, currentPreset.presetSettings))
            {
                if (containsKey == false) return;

                unappliedSettings.Remove(settingPresetType);
                InformSettingChange(new SettingChange(index, currentPreset, preset));
                return;
            }

            commands.Do(new ChangeSettingPresetCommand(index, unappliedSettings, listeners, preset, settings, commands));
        }

        public bool Undo()
        {
            if (commands.undoCount == 0) return false;
            commands.Undo();
            return true;
        }

        public bool Redo()
        {
            if (commands.redoCount == 0) return false;
            commands.Redo();
            return true;
        }

        public void ApplyChanges(bool keepChanges = false)
        {
            commands.Do(new ApplySettingChangesCommand(this, settingApplier, listeners, settings, unappliedSettings));
        }

        public SettingPreset GetPresetAt(int index)
        {
            return presets[index];
        }

        void InformSettingChange(SettingChange settingChange)
        {
            int count = listeners.Count;
            for (int i = 0; i < count; i++)
            {
                listeners[i].OnSettingChanged(settingChange);
            }
        }

        int IndexOfSetting(Type type)
        {
            int count = settings.Count;
            for (int i = 0; i < count; i++)
            {
                if (settings[i].GetType() == type) return i;
            }
            return -1;
        }

        public bool HasUnappliedChange()
        {
            return unappliedSettings.Count > 0;
        }

        public T GetSetting<T>() where T : ISetting
        {
            return (T)settings[IndexOfSetting(typeof(T))];
        }

        public bool ChangeSetting<T>(T newValue) where T : ISetting
        {
            var type = typeof(T);
            var index = IndexOfSetting(type);
            var currentValue = unappliedSettings.ContainsKey(type) ? unappliedSettings[type].to : settings[index];
            if (currentValue.Equals(newValue)) return false;
            commands.Do(new SettingChangeCommand(commands, index, unappliedSettings, newValue, listeners, settings, presets));
            return true;
        }

        public IEnumerable<ISetting> GetSettings()
        {
            return settings;
        }

        public IEnumerable<SettingChange> GetUnappliedSettings()
        {
            return unappliedSettings.Values;
        }

        public void ClearChangeHistory()
        {
            commands.Clear();
        }

        public bool AddListener(ISettingListener listener)
        {
            if (listeners.Contains(listener)) return false;
            listeners.Add(listener);
            return true;
        }

        public bool RemoveListener(ISettingListener listener)
        {
            return listeners.Remove(listener);
        }
    }
}