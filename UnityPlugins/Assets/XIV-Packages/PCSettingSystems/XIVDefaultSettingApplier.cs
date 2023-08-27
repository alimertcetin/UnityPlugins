using System;
using System.Collections.Generic;

namespace XIV_Packages.PCSettingsSystem
{
    public class XIVDefaultSettingApplier : ISettingApplier
    {
        Dictionary<Type, Command> settingApplyDictionary = new();

        bool ISettingApplier.AddApplyCommand<T>(Command<T> command)
        {
            var type = typeof(T);
            if (settingApplyDictionary.ContainsKey(type)) return false;
            settingApplyDictionary.Add(type, command);
            return true;
        }

        bool ISettingApplier.RemoveApplyCommand<T>()
        {
            return settingApplyDictionary.Remove(typeof(T));
        }

        void ISettingApplier.Apply(ISettingContainer settingContainer)
        {
            foreach (ISetting setting in settingContainer.GetSettings())
            {
                if (settingApplyDictionary.TryGetValue(setting.GetType(), out var command))
                {
                    command.Apply(setting);
                }
#if UNITY_EDITOR
                else
                {
                    UnityEngine.Debug.LogError("There is no command added for " + setting.GetType().Name);
                }
#endif
            }
        }
    }
}