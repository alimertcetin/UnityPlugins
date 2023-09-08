using UnityEngine;
using XIV_Packages.PCSettingSystems.Core;
using XIV_Packages.ScriptableObjects.Channels;

namespace XIV_Packages.PCSettingSystems.Extras.ScriptableObjects.Channels
{
    [CreateAssetMenu(menuName = MenuPaths.CHANNEL_BASE_MENU + nameof(SettingsChannelSO))]
    public class SettingsChannelSO : XIVChannelSO<XIVSettings>
    {
        
    }
}