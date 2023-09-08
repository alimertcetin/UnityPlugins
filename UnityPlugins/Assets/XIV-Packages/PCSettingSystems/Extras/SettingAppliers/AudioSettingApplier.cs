using System;
using XIV_Packages.PCSettingsSystem;

namespace Assets.XIV
{
    public class AudioSettingApplier : ISettingApplier
    {
        AudioSettingApplyCommand audioSettingApplyCommand;

        bool ISettingApplier.AddApplyCommand<T>(ApplyCommand<T> command)
        {
            if (command is not AudioSettingApplyCommand audioSettingApplyCommand) return false;
            this.audioSettingApplyCommand = audioSettingApplyCommand;
            return true;
        }

        bool ISettingApplier.RemoveApplyCommand<T>()
        {
            bool isNull = audioSettingApplyCommand == null;
            audioSettingApplyCommand = null;
            return isNull == false;
        }

        void ISettingApplier.Apply(ISettingContainer settingContainer)
        {
            foreach (ISetting setting in settingContainer.GetSettings())
            {
                if (setting is AudioSetting audioSetting)
                    audioSettingApplyCommand.Apply(setting);
#if UNITY_EDITOR
                else
                {
                    UnityEngine.Debug.LogWarning("There is no command added for " + setting.GetType().Name);
                }
#endif
            }
        }
    }
}