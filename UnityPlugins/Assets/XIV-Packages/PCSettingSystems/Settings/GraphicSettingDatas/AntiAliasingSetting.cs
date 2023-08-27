namespace XIV_Packages.PCSettingsSystem
{
    [System.Serializable]
    public struct AntiAliasingSetting : ISetting
    {
        public int antiAliasing;

        bool ISetting.canIncludeInPresets => true;

        bool ISetting.IsCritical => false;

        public AntiAliasingSetting(AntiAliasingSetting setting)
        {
            this.antiAliasing = setting.antiAliasing;
        }

        public AntiAliasingSetting(int antiAliasing)
        {
            this.antiAliasing = antiAliasing;
        }

        public override string ToString()
        {
            return $"{nameof(antiAliasing)} : {antiAliasing}";
        }
    }
}