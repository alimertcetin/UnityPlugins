using Assets.XIV;
using UnityEngine;

namespace XIV_Packages.PCSettingsSystem
{
    public class AntialiasingApplyCommand : ApplyCommand<AntiAliasingSetting>
    {
        public override void Apply(AntiAliasingSetting value)
        {
            QualitySettings.antiAliasing = value.antiAliasing;
        }
    }
}