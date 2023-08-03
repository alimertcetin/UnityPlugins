using UnityEngine;
using XIV.Core;

namespace XIV_Packages.InventorySystem
{
    [System.Serializable]
    public abstract class ItemBase
    {
        [field : SerializeField, DisplayWithoutEdit] public string id { get; private set; }
        public string title;
        public string description;
        
        [Min(1)]
        public int StackableAmount = 1;

        public void GenerateID() => id = System.Guid.NewGuid().ToString();
        
        public virtual bool Equals(ItemBase other)
        {
            return other.id == this.id;
        }
    }
}