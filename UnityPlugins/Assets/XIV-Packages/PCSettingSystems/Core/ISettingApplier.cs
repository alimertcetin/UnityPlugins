namespace XIV_Packages.PCSettingsSystem
{
    public interface ISettingApplier
    {
        bool AddApplyCommand<T>(Command<T> command) where T : ISetting;
        bool RemoveApplyCommand<T>() where T : ISetting;
        void Apply(ISettingContainer settingContainer);
    }
}