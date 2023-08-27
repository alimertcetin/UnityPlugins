using System;
using UnityEngine;

namespace XIV_Packages.PCSettingsSystem
{
    public class AntialiasingCommand : Command<AntiAliasingSetting>
    {
        public override void Apply(AntiAliasingSetting value)
        {
            QualitySettings.antiAliasing = value.antiAliasing;
        }
    }
}