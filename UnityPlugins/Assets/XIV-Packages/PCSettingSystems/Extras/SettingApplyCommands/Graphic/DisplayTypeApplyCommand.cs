using Assets.XIV;
using UnityEngine;

namespace XIV_Packages.PCSettingsSystem
{
    public class DisplayTypeApplyCommand : ApplyCommand<DisplayTypeSetting>
    {
        public override void Apply(DisplayTypeSetting value)
        {
            Screen.fullScreen = value.isFullScreen;
        }
    }
}