using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using XIV_Packages.PCSettingSystems.Core;
using XIV_Packages.PCSettingSystems.Extras.SettingAppliers;
using XIV_Packages.PCSettingSystems.Extras.SettingApplyCommands.AudioCommands;
using XIV_Packages.PCSettingSystems.Extras.SettingContainers;
using XIV_Packages.PCSettingSystems.Extras.SettingDatas.AudioDatas;

namespace XIV_Packages.PCSettingSystems.Extras
{
    public class AudioSettingManager : SettingManager
    {
        [SerializeField] AudioMixer mixer;

        AudioSettingContainer audioSettingContainer;

        public override void InitializeContainer()
        {
            audioSettingContainer = new AudioSettingContainer(CreateAudioSettingApplier());
            var audioSettings = new List<ISetting>();
            audioSettings.Add(new MasterAudioSetting("master", 0.75f));
            audioSettings.Add(new MusicAudioSetting("music", 0.5f));
            audioSettings.Add(new EffectAudioSetting("effect", 0.5f));
            audioSettingContainer.InitializeSettings(audioSettings);
            audioSettingContainer.ApplyChanges();
            audioSettingContainer.ClearUndoHistory();
        }

        public override ISettingContainer GetContainer() => audioSettingContainer;

        ISettingApplier CreateAudioSettingApplier()
        {
            ISettingApplier settingApplier = new AudioSettingApplier();
            settingApplier.AddApplyCommand(new AudioSettingApplyCommand(mixer));
            return settingApplier;
        }
    }
}