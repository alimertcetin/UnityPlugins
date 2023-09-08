using UnityEngine;
using XIV_Packages.PCSettingsSystem;
using XIV_Packages.ScriptableObjects.Channels;

namespace Assets.XIV
{
    [CreateAssetMenu(menuName = MenuPaths.CHANNEL_BASE_MENU + nameof(SettingsChannelSO))]
    public class SettingsChannelSO : XIVChannelSO<XIVSettings>
    {
        
    }
}