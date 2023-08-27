namespace XIV_Packages.PCSettingsSystem
{
    [System.Serializable]
    public struct VsyncSetting : ISetting
    {
        public bool isOn;

        bool ISetting.canIncludeInPresets => false;

        bool ISetting.IsCritical => false;

        public VsyncSetting(VsyncSetting vsyncSetting)
        {
            this.isOn = vsyncSetting.isOn;
        }

        public VsyncSetting(bool isOn)
        {
            this.isOn = isOn;
        }

        public override string ToString()
        {
            return $"{nameof(VsyncSetting)}, {nameof(isOn)} : {isOn}";
        }
    }
}