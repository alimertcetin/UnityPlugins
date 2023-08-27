using System;
using UnityEngine;

namespace XIV_Packages.PCSettingsSystem
{
    public class TextureQualityCommand : Command<TextureQualitySetting>
    {
        public override void Apply(TextureQualitySetting value)
        {
            QualitySettings.masterTextureLimit = value.masterTextureLimit;
        }
    }
}