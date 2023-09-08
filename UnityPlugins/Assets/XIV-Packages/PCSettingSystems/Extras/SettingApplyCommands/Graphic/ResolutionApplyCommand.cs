using Assets.XIV;
using UnityEngine;

namespace XIV_Packages.PCSettingsSystem
{
    public class ResolutionApplyCommand : ApplyCommand<ResolutionSetting>
    {
        public override void Apply(ResolutionSetting value)
        {
            Screen.SetResolution(value.x, value.y, Screen.fullScreen);
        }
    }
}