using Assets.XIV;
using UnityEngine;

namespace XIV_Packages.PCSettingsSystem
{
    public class VsyncApplyCommand : ApplyCommand<VsyncSetting>
    {
        public override void Apply(VsyncSetting value)
        {
            QualitySettings.vSyncCount = value.isOn ? 1 : 0;
        }
    }
}