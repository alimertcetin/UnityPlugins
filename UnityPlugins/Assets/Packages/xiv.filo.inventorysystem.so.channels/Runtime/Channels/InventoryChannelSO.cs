using UnityEngine;
using XIV_Packages.ScriptableObjects.Channels;

namespace XIV_Packages.InventorySystem.ScriptableObjects.Channels
{
    [CreateAssetMenu(menuName = MenuPaths.CHANNEL_MENU + nameof(InventoryChannelSO))]
    public class InventoryChannelSO : XIVChannelSO<Inventory>
    {
        
    }
}