using System;
using UnityEngine;

namespace XIV_Packages.PCSettingsSystem
{
    public class VsyncCommand : Command<VsyncSetting>
    {
        public override void Apply(VsyncSetting value)
        {
            QualitySettings.vSyncCount = value.isOn ? 1 : 0;
        }
    }
}