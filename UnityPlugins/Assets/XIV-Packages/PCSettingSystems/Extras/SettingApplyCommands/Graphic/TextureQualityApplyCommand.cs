using Assets.XIV;
using UnityEngine;

namespace XIV_Packages.PCSettingsSystem
{
    public class TextureQualityApplyCommand : ApplyCommand<TextureQualitySetting>
    {
        public override void Apply(TextureQualitySetting value)
        {
            QualitySettings.masterTextureLimit = value.masterTextureLimit;
        }
    }
}