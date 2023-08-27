namespace XIV_Packages.PCSettingsSystem
{
    [System.Serializable]
    public struct DisplayTypeSetting : ISetting
    {
        public bool isFullScreen;

        bool ISetting.canIncludeInPresets => false;

        bool ISetting.IsCritical => true;

        public DisplayTypeSetting(DisplayTypeSetting displayTypeSetting)
        {
            this.isFullScreen = displayTypeSetting.isFullScreen;
        }

        public DisplayTypeSetting(bool isFullScreen)
        {
            this.isFullScreen = isFullScreen;
        }

        public override string ToString()
        {
            return $"{nameof(DisplayTypeSetting)}, {nameof(isFullScreen)} : {isFullScreen}";
        }
    }
}