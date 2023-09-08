using Assets.XIV;

namespace XIV_Packages.PCSettingsSystem
{
    public interface ISettingListener
    {
        /// <summary>
        /// Called when a settings is changed
        /// </summary>
        void OnSettingChanged(SettingChange settingChange);

        /// <summary>
        /// Before applying the changes
        /// </summary>
        void OnBeforeApply(ISettingContainer container);

        /// <summary>
        /// After applying the changes
        /// </summary>
        void OnAfterApply(ISettingContainer container);
    }
}