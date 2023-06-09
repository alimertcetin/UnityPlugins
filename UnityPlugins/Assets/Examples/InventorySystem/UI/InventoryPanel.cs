﻿using UnityEngine;
using XIV.InventorySystem.ScriptableObjects.ChannelSOs;
using XIV.InventorySystem.ScriptableObjects.NonSerializedData;

namespace XIV.InventorySystem.UI
{
    public class InventoryPanel : MonoBehaviour
    {
        [SerializeField] NonSerializedItemDataContainerSO ItemDataContainerSO;
        [SerializeField] InventoryChannelSO inventoryLoadedChannel;
        [SerializeField] InventoryChangeChannelSO inventoryChangedChannel;
        
        [SerializeField] Transform contentParent;
        InventorySlot[] slots;
        int slotCount;
        Inventory inventory;
        [Tooltip("Start Index defines where to start displaying the items from inventory")]
        public int startIndex;
        
        void OnEnable()
        {
            inventoryLoadedChannel.Register(OnInventoryLoaded);
            inventoryChangedChannel.Register(OnInventoryChanged);
            
            slotCount = contentParent.childCount;
            slots = contentParent.GetComponentsInChildren<InventorySlot>();
            for (int i = 0; i < slotCount; i++)
            {
                InventorySlot inventorySlot = slots[i];
                inventorySlot.SetItem(default, null);
            }
        }

        void OnDisable()
        {
            inventoryLoadedChannel.Unregister(OnInventoryLoaded);
            inventoryChangedChannel.Unregister(OnInventoryChanged);
        }

        void OnInventoryLoaded(Inventory inventory)
        {
            this.inventory = inventory;
            for (int i = 0, j = startIndex; i < slotCount; i++, j++)
            {
                InventorySlot inventorySlot = slots[i];
                inventorySlot.SetItem(inventory[j], ItemDataContainerSO.GetSprite(inventory[j].Item));
            }
        }

        void OnInventoryChanged(InventoryChange inventoryChange)
        {
            for (int i = 0; i < inventoryChange.ChangeCount; i++)
            {
                int index = inventoryChange.ChangedItems[i].ChangedIndex;
                if (index < startIndex || index >= slotCount + startIndex) continue;
                
                var inventorySlot = slots[index - startIndex];
                inventorySlot.SetItem(inventory[index], ItemDataContainerSO.GetSprite(inventory[index].Item));
            }
        }
    }
}