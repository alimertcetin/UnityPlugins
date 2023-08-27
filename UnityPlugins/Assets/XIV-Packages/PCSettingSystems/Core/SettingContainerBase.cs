using System;
using System.Collections.Generic;

namespace XIV_Packages.PCSettingsSystem
{
    public abstract class SettingContainerBase : ISettingContainer
    {
        protected List<ISetting> settings;
        protected List<ISettingListener> listeners;
        protected UndoRedoStack<ICommand> changes;
        protected Dictionary<Type, SettingChange> unappliedChanges;

        protected ISettingApplier settingApplier;

        public SettingContainerBase(ISettingApplier settingApplier)
        {
            this.settings = new List<ISetting>();
            this.listeners = new List<ISettingListener>();
            this.changes = new UndoRedoStack<ICommand>();
            this.unappliedChanges = new Dictionary<Type, SettingChange>();
            this.settingApplier = settingApplier;
        }

        public SettingContainerBase(SettingContainerBase other)
        {
            this.settings = new List<ISetting>(other.settings);
            this.listeners = new List<ISettingListener>(other.listeners);
            this.changes = new UndoRedoStack<ICommand>(other.changes);
            this.unappliedChanges = new Dictionary<Type, SettingChange>(other.unappliedChanges);
            this.settingApplier = other.settingApplier;
        }

        public virtual bool HasUnappliedChange() => unappliedChanges.Count > 0;

        public T GetSetting<T>() where T : ISetting => (T)settings[IndexOfSetting(typeof(T))];

        public bool ChangeSetting<T>(T newValue) where T : ISetting => ChangeSetting(typeof(T), newValue, true);

        public virtual void ApplyChanges(bool keepChanges = false)
        {
            InformBeforeApply();

            foreach (var settingChange in unappliedChanges.Values)
            {
                settings[settingChange.index] = settingChange.to;
            }

            unappliedChanges.Clear();
            settingApplier.Apply(this);
            if (keepChanges == false) changes.Clear();

            InformAfterApply();
        }

        public IEnumerable<ISetting> GetSettings()
        {
            return settings;
        }

        public IEnumerable<SettingChange> GetUnappliedSettings()
        {
            return unappliedChanges.Values;
        }

        public virtual bool Undo()
        {
            if (changes.undoCount == 0) return false;
            changes.Undo();
            return true;
        }

        public virtual bool Redo()
        {
            if (changes.redoCount == 0) return false;
            changes.Redo();
            return true;
        }

        public void ClearChangeHistory() => changes.Clear();

        public bool AddListener(ISettingListener settingListener)
        {
            if (listeners.Contains(settingListener)) return false;
            listeners.Add(settingListener);
            return true;
        }

        public bool RemoveListener(ISettingListener settingListener)
        {
            return listeners.Remove(settingListener);
        }

        protected virtual bool ChangeSetting(Type settingType, ISetting newValue, bool saveChanges)
        {
            var index = IndexOfSetting(settingType);
            var settingValue = settings[index];

            if (newValue.Equals(settingValue))
            {
                if (unappliedChanges.ContainsKey(settingType) == false) return false;

                var rollbackChange = new SettingChangeCommand(changes, index, unappliedChanges, newValue, listeners, settings, default);
                changes.Do(rollbackChange);
                return true;
            }

            SettingChangeCommand change = new SettingChangeCommand(changes, index, unappliedChanges, newValue, listeners, settings, default);
            changes.Do(change);
            return true;
        }

        protected int IndexOfSetting(Type type)
        {
            var count = settings.Count;
            for (int i = 0; i < count; i++)
            {
                if (settings[i].GetType() == type) return i;
            }
            return -1;
        }

        protected void InformBeforeApply()
        {
            int count = listeners.Count;
            for (int i = 0; i < count; i++)
            {
                listeners[i].OnBeforeApply(this);
            }
        }

        protected void InformAfterApply()
        {
            int count = listeners.Count;
            for (int i = 0; i < count; i++)
            {
                listeners[i].OnAfterApply(this);
            }
        }
    }
}