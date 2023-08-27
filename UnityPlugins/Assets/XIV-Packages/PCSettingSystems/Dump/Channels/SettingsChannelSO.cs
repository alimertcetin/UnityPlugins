using UnityEngine;
using XIV_Packages.PCSettingsSystem;

namespace TheGame.ScriptableObjects.Channels
{
    [CreateAssetMenu(menuName = MenuPaths.CHANNEL_BASE_MENU + nameof(SettingsChannelSO))]
    public class SettingsChannelSO : XIVChannelSO<XIVSettings>
    {
        
    }
}