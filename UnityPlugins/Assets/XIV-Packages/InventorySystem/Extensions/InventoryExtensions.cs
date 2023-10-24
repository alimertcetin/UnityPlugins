using System;
using System.Buffers;

namespace XIV_Packages.InventorySystem.Extensions
{
    public static class InventoryExtensions
    {
        /// <summary>
        /// Moves the <paramref name="index1"/> over <paramref name="index2"/>.
        /// Merges as much as possible if items are same.
        /// Swaps them if they are different.
        /// <seealso cref="Inventory.Merge"/>
        /// <seealso cref="Inventory.Swap"/>
        /// </summary>
        public static void Move(this Inventory inventory, int index1, int index2)
        {
            if (inventory.CanMerge(index1, index2)) inventory.Merge(index1, index2);
            else inventory.Swap(index1, index2);
        }
        
        /// <summary>
        /// Returns true if we can add all giving <paramref name="quantity"/>
        /// </summary>
        public static bool CanAddItem(this Inventory inventory, ItemBase item, int quantity)
        {
            int slotCount = inventory.slotCount;
            // First check stackable amount with existing items
            for (var i = 0; i < slotCount && quantity > 0; i++)
            {
                ReadOnlyInventoryItem inventoryItem = inventory[i];
                if (inventoryItem.IsEmpty || inventoryItem.Item.Equals(item) == false) continue;

                int stackLeft = inventoryItem.Item.StackableAmount - inventoryItem.Quantity;
                quantity -= Math.Min(stackLeft, quantity);
            }
            
            // Check empty slots if amount is still greater than 0
            for (int i = 0; i < slotCount && quantity > 0; i++)
            {
                if (inventory[i].IsEmpty)
                {
                    quantity -= Math.Min(item.StackableAmount, quantity);
                }
            }

            return quantity <= 0;
        }
        
        /// <summary>
        /// Returns true if <typeparamref name="T"/> can be removed from <paramref name="inventory"/> completely
        /// </summary>
        public static bool CanRemoveItem<T>(this Inventory inventory, int amount) where T : ItemBase
        {
            return CalculateRemainingAfterRemove<T>(inventory, amount) == 0;
        }
        
        /// <summary>
        /// Returns the quantity that will remain after <see cref="Inventory.Remove"/>
        /// </summary>
        public static int CalculateRemainingAfterRemove<T>(this Inventory inventory, int quantity) where T : ItemBase
        {
            var readonlyInventoryItemBuffer = ArrayPool<ReadOnlyInventoryItem>.Shared.Rent(inventory.slotCount);
            int length = GetItemsOfTypeNonAlloc<T>(inventory, readonlyInventoryItemBuffer);
            for (var i = 0; i < length && quantity > 0; i++)
            {
                quantity -= Math.Min(readonlyInventoryItemBuffer[i].Quantity, quantity);
            }
            ArrayPool<ReadOnlyInventoryItem>.Shared.Return(readonlyInventoryItemBuffer);
            return quantity;
        }
        
        /// <summary>
        /// Returns the quantity that will remain after <see cref="Inventory.Add"/>
        /// </summary>
        public static int CalculateRemainingAfterAdd(this Inventory inventory, ItemBase item, int quantity)
        {
            int slotCount = inventory.slotCount;
            // Fill existing item slots
            for (var i = 0; i < slotCount && quantity > 0; i++)
            {
                var inventoryItem = inventory[i];
                if (inventoryItem.IsEmpty || inventoryItem.Item.Equals(item) == false) continue;
                
                int stackLeft = inventoryItem.Item.StackableAmount - inventoryItem.Quantity;
                if (stackLeft == 0) continue;

                int addAmount = Math.Min(stackLeft, quantity);
                quantity -= addAmount;
            }

            // Fill new slots if quantity is greater than 0
            for (int i = 0; i < slotCount && quantity > 0; i++)
            {
                var inventoryItem = inventory[i];
                if (inventoryItem.IsEmpty == false) continue;

                int addAmount = Math.Min(item.StackableAmount, quantity);
                quantity -= addAmount;
            }

            return quantity;
        }
        
        /// <summary>
        /// Fills the <paramref name="indicesBuffer"/> with the corresponding indices of <typeparamref name="T"/>
        /// </summary>
        public static int GetIndicesOf<T>(this Inventory inventory, int[] indicesBuffer) where T : ItemBase
        {
            int length = indicesBuffer.Length;
            int count = 0;
            int slotCount = inventory.slotCount;
            for (int i = 0; i < slotCount && count < length; i++)
            {
                var inventoryItem = inventory[i];
                if (inventoryItem.IsEmpty == false && inventoryItem.Item is T)
                {
                    indicesBuffer[count++] = i;
                }
            }

            return count;
        }
        
        /// <summary>
        /// Fills the <paramref name="indicesBuffer"/> with the corresponding indices of <typeparamref name="T"/>
        /// and matches with <paramref name="match"/>
        /// </summary>
        public static int GetIndicesOf<T>(this Inventory inventory, int[] indicesBuffer, Predicate<T> match) where T : ItemBase
        {
            var buffer = ArrayPool<int>.Shared.Rent(inventory.slotCount);
            int length = GetIndicesOf<T>(inventory, buffer);
            int indicesBufferLength = indicesBuffer.Length;
            int count = 0;
            for (int i = 0; i < length && count < indicesBufferLength; i++)
            {
                var inventoryItem = inventory[buffer[i]];
                if (match.Invoke((T)inventoryItem.Item))
                {
                    indicesBuffer[count++] = buffer[i];
                }
            }
            
            ArrayPool<int>.Shared.Return(buffer);
            return count;
        }

        public static int GetOccupiedIndices(this Inventory inventory, int[] indicesBuffer)
        {
            return GetIndicesOf<ItemBase>(inventory, indicesBuffer);
        }
        
        /// <summary>
        /// Returns a <see cref="ReadOnlyInventoryItem"/> array
        /// while making sure all <see cref="ReadOnlyInventoryItem.Item"/> is type of <typeparamref name="T"/>
        /// </summary>
        public static ReadOnlyInventoryItem[] GetItemsOfType<T>(this Inventory inventory) where T : ItemBase
        {
            var indicesBuffer = ArrayPool<int>.Shared.Rent(inventory.slotCount);
            int length = GetIndicesOf<T>(inventory, indicesBuffer);
            var itemsArr = new ReadOnlyInventoryItem[length];
            Fill(inventory, itemsArr, length, indicesBuffer, length);
            ArrayPool<int>.Shared.Return(indicesBuffer);
            return itemsArr;
        }
        
        /// <summary>
        /// Returns a <see cref="ReadOnlyInventoryItem"/> array
        /// while making sure all <see cref="ReadOnlyInventoryItem.Item"/> is type of <typeparamref name="T"/>
        /// and they all match with <paramref name="match"/>
        /// </summary>
        public static ReadOnlyInventoryItem[] GetItemsOfType<T>(this Inventory inventory, Predicate<T> match) where T : ItemBase
        {
            var indicesBuffer = ArrayPool<int>.Shared.Rent(inventory.slotCount);
            int length = GetIndicesOf<T>(inventory, indicesBuffer, match);
            var itemsArr = new ReadOnlyInventoryItem[length];
            Fill(inventory, itemsArr, length, indicesBuffer, length);
            ArrayPool<int>.Shared.Return(indicesBuffer);
            return itemsArr;
        }
        
        /// <summary>
        /// Fills the <paramref name="inventoryItemBuffer"/> and returns the filled count
        /// while making sure all <see cref="ReadOnlyInventoryItem.Item"/> is type of <typeparamref name="T"/>
        /// </summary>
        public static int GetItemsOfTypeNonAlloc<T>(this Inventory inventory, ReadOnlyInventoryItem[] inventoryItemBuffer) where T : ItemBase
        {
            int[] indicesBuffer = ArrayPool<int>.Shared.Rent(inventory.slotCount);
            int indicesBufferLength = GetIndicesOf<T>(inventory, indicesBuffer);
            int count = Fill(inventory, inventoryItemBuffer, inventoryItemBuffer.Length, indicesBuffer, indicesBufferLength);
            ArrayPool<int>.Shared.Return(indicesBuffer);
            return count;
        }
        
        /// <summary>
        /// Fills the <paramref name="inventoryItemBuffer"/> and returns the filled count
        /// while making sure all <see cref="ReadOnlyInventoryItem.Item"/> is type of <typeparamref name="T"/>
        /// and they all match with <paramref name="match"/>
        /// </summary>
        public static int GetItemsOfTypeNonAlloc<T>(this Inventory inventory, ReadOnlyInventoryItem[] inventoryItemBuffer, Predicate<T> match) where T : ItemBase
        {
            int[] indicesBuffer = ArrayPool<int>.Shared.Rent(inventory.slotCount);
            int indicesBufferLength = GetIndicesOf<T>(inventory, indicesBuffer, match);
            int count = Fill(inventory, inventoryItemBuffer, inventoryItemBuffer.Length, indicesBuffer, indicesBufferLength);
            ArrayPool<int>.Shared.Return(indicesBuffer);
            return count;
        }
        
        /// <summary>
        /// Returns how many slots are occupied by type <typeparamref name="T"/>
        /// </summary>
        public static int GetCountOf<T>(this Inventory inventory) where T : ItemBase
        {
            var indicesBuffer = ArrayPool<int>.Shared.Rent(inventory.slotCount);
            int length = GetIndicesOf<T>(inventory, indicesBuffer);
            ArrayPool<int>.Shared.Return(indicesBuffer);
            return length;
        }

        /// <summary>
        /// Returns the amount of items that matches with <paramref name="match"/>
        /// and also makes sure they are of type <typeparamref name="T"/>
        /// </summary>
        public static int GetCountOf<T>(this Inventory inventory, Predicate<T> match) where T : ItemBase
        {
            var indicesBuffer = ArrayPool<int>.Shared.Rent(inventory.slotCount);
            int length = GetIndicesOf<T>(inventory, indicesBuffer, match);
            ArrayPool<int>.Shared.Return(indicesBuffer);
            return length;
        }
        
        /// <summary>
        /// Returns the total quantity of type <typeparamref name="T"/>
        /// </summary>
        public static int GetQuantityOf<T>(this Inventory inventory) where T : ItemBase
        {
            var indicesBuffer = ArrayPool<int>.Shared.Rent(inventory.slotCount);
            int length = GetIndicesOf<T>(inventory, indicesBuffer);
            int quantity = 0;
            for (int i = 0; i < length; i++)
            {
                quantity += inventory[indicesBuffer[i]].Quantity;
            }
            
            ArrayPool<int>.Shared.Return(indicesBuffer);
            return quantity;
        }

        /// <summary>
        /// Returns the item that has minimum quantity
        /// </summary>
        public static ReadOnlyInventoryItem GetItemHasMinQuantity<T>(this Inventory inventory) where T : ItemBase
        {
            int min = int.MaxValue;
            ReadOnlyInventoryItem current = default;
            var indicesBuffer = ArrayPool<int>.Shared.Rent(inventory.slotCount);
            int length = GetIndicesOf<T>(inventory, indicesBuffer);
            for (var i = 0; i < length; i++)
            {
                var inventoryItem = inventory[indicesBuffer[i]];
                if (inventoryItem.Quantity > min) continue;
                
                min = inventoryItem.Quantity;
                current = inventoryItem;
            }
            ArrayPool<int>.Shared.Return(indicesBuffer);
            return current;
        }

        /// <summary>
        /// Returns the item that has maximum quantity
        /// </summary>
        public static ReadOnlyInventoryItem GetItemHasMaxQuantity<T>(this Inventory inventory) where T : ItemBase
        {
            int max = int.MinValue;
            ReadOnlyInventoryItem current = default;
            var indicesBuffer = ArrayPool<int>.Shared.Rent(inventory.slotCount);
            int length = GetIndicesOf<T>(inventory, indicesBuffer);
            for (var i = 0; i < length; i++)
            {
                var inventoryItem = inventory[i];
                if (inventoryItem.Quantity < max) continue;
                
                max = inventoryItem.Quantity;
                current = inventoryItem;
            }
            ArrayPool<int>.Shared.Return(indicesBuffer);
            return current;
        }

        /// <summary>
        /// Use to fill <paramref name="inventoryItemBuffer"/> if both buffers requires different limits
        /// </summary>
        static int Fill(Inventory inventory, 
            ReadOnlyInventoryItem[] inventoryItemBuffer, int inventoryItemBufferLength, 
            int[] indicesBuffer, int indicesBufferLength)
        {
            int count = 0;
            for (; count < indicesBufferLength && count < inventoryItemBufferLength; count++)
            {
                inventoryItemBuffer[count] = inventory[indicesBuffer[count]];
            }
            return count;
        }
    }
}