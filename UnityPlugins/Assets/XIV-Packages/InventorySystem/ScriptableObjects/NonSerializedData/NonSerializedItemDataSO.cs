using UnityEngine;

namespace XIV_Packages.InventorySystem.ScriptableObjects.NonSerializedData
{
    public abstract class NonSerializedItemDataSO : ScriptableObject
    {
        public ItemSO itemSO;
        public Sprite uiSprite;
    }
}