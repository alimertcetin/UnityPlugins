namespace XIV_Packages.PCSettingsSystem
{
    [System.Serializable]
    public struct TextureQualitySetting : ISetting
    {
        /// <summary>
        /// 0 is Very High
        /// </summary>
        public int masterTextureLimit;

        bool ISetting.canIncludeInPresets => true;

        bool ISetting.IsCritical => false;

        public TextureQualitySetting(TextureQualitySetting setting)
        {
            this.masterTextureLimit = setting.masterTextureLimit;
        }

        public TextureQualitySetting(int masterTextureLimit)
        {
            this.masterTextureLimit = masterTextureLimit;
        }

        public override string ToString()
        {
            return $"{nameof(TextureQualitySetting)}, {nameof(masterTextureLimit)} : {masterTextureLimit}";
        }
    }
}