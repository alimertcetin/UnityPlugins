namespace XIV_Packages.PCSettingsSystem
{
    public class GraphicSettingPresetCommand : Command<SettingPreset>
    {
        public GraphicSettingPresetCommand(ISettingApplier settingApplier) : base()
        {
            settingApplier.AddApplyCommand(new AntialiasingCommand());
            settingApplier.AddApplyCommand(new ResolutionCommand());
            settingApplier.AddApplyCommand(new ShadowQualityCommand());
            settingApplier.AddApplyCommand(new TextureQualityCommand());
            settingApplier.AddApplyCommand(new DisplayTypeCommand());
            settingApplier.AddApplyCommand(new VsyncCommand());
        }

        public override void Apply(SettingPreset _)
        {

        }
    }
}