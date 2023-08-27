using System;
using UnityEngine;

namespace XIV_Packages.PCSettingsSystem
{
    public class DisplayTypeCommand : Command<DisplayTypeSetting>
    {
        public override void Apply(DisplayTypeSetting value)
        {
            Screen.fullScreen = value.isFullScreen;
        }
    }
}