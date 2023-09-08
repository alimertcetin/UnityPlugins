using UnityEngine;
using UnityEngine.Audio;
using XIV_Packages.PCSettingSystems.Core;
using XIV_Packages.PCSettingSystems.Extras.SettingDatas.AudioDatas;

namespace XIV_Packages.PCSettingSystems.Extras.SettingApplyCommands.AudioCommands
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