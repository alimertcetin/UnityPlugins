using UnityEngine;

namespace XIV_Packages.InventorySystem.ScriptableObjects.NonSerializedData
{
    [CreateAssetMenu(menuName = "Inventory/NonSerializedDataContainerSO")]
    public class NonSerializedItemDataContainerSO : ScriptableObject
    {
        public NonSerializedItemDataSO[] itemDataPairs;

        public Sprite GetSprite(ItemBase itemBase)
        {
            for (int i = 0; i < itemDataPairs.Length; i++)
            {
                if (itemDataPairs[i].itemSO.GetItem().id == itemBase.id)
                {
                    return itemDataPairs[i].uiSprite;
                }
            }

            return null;
        }
    }
}