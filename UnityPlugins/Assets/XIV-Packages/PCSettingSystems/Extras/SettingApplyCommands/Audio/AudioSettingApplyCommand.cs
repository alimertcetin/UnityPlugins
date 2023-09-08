using Assets.XIV;
using UnityEngine;
using UnityEngine.Audio;

namespace XIV_Packages.PCSettingsSystem
{
    public class AudioSettingApplyCommand : ApplyCommand<AudioSetting>
    {
        readonly AudioMixer mixer;

        public AudioSettingApplyCommand(AudioMixer mixer)
        {
            this.mixer = mixer;
        }

        public override void Apply(AudioSetting value)
        {
            mixer.SetFloat(value.mixerParameter, Mathf.Log10(value.value01) * 20f);
        }
    }
}