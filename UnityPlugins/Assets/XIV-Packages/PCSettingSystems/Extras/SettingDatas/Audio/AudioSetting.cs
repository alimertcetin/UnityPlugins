namespace Assets.XIV
{
    [System.Serializable]
    public struct AudioSetting : ISetting
    {
        /// <summary>
        /// The name of the parameter in <see cref="UnityEngine.Audio.AudioMixer"/>
        /// </summary>
        public string mixerParameter;

        /// <summary>
        /// A number between 0 and 1
        /// </summary>
        public float value01;

        bool ISetting.canIncludeInPresets => false;

        bool ISetting.IsCritical => false;

        public AudioSetting(AudioSetting setting)
        {
            this.mixerParameter = setting.mixerParameter;
            this.value01 = setting.value01;
        }

        public AudioSetting(string mixerParameter, float soundLevel)
        {
            this.mixerParameter = mixerParameter;
            this.value01 = soundLevel;
        }

        public override string ToString()
        {
            return $"{nameof(mixerParameter)} : {mixerParameter}, {nameof(value01)} : {value01}";
        }
    }
}