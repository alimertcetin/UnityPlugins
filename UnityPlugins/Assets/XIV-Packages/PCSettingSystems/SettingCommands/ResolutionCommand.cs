using System;
using UnityEngine;

namespace XIV_Packages.PCSettingsSystem
{
    public class ResolutionCommand : Command<ResolutionSetting>
    {
        public override void Apply(ResolutionSetting value)
        {
            Screen.SetResolution(value.x, value.y, Screen.fullScreen);
        }
    }
}