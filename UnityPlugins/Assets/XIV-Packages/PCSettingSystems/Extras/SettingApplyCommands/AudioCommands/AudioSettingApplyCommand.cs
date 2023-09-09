using UnityEngine;
using UnityEngine.Audio;
using XIV_Packages.PCSettingSystems.Core;
using XIV_Packages.PCSettingSystems.Extras.SettingDatas.AudioDatas;

namespace XIV_Packages.PCSettingSystems.Extras.SettingApplyCommands.AudioCommands
{
    public class AudioSettingApplyCommand : ApplyCommand<IAudioSetting>
    {
        readonly AudioMixer mixer;

        public AudioSettingApplyCommand(AudioMixer mixer)
        {
            this.mixer = mixer;
        }

        public override void Apply(IAudioSetting value)
        {
            mixer.SetFloat(value.MixerParameter, Mathf.Log10(value.Value01) * 20f);
        }
    }
}