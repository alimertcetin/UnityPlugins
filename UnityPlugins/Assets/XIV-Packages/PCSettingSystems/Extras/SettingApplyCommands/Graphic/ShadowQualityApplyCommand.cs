using Assets.XIV;
using UnityEngine;

namespace XIV_Packages.PCSettingsSystem
{
    public class ShadowQualityApplyCommand : ApplyCommand<ShadowQualitySetting>
    {
        public override void Apply(ShadowQualitySetting value)
        {
            QualitySettings.shadowCascades = value.shadowCascades;
            QualitySettings.shadowDistance = value.shadowDistance;
            QualitySettings.shadowmaskMode = value.shadowmaskMode;
            QualitySettings.shadowResolution = value.shadowResolution;
            QualitySettings.shadows = value.shadows;
        }
    }
}