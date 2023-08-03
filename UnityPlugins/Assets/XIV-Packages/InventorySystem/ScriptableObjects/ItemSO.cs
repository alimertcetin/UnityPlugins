using UnityEngine;

namespace XIV_Packages.InventorySystem.ScriptableObjects
{
    public abstract class ItemSO : ScriptableObject
    {
#if UNITY_EDITOR
        [ContextMenu(nameof(GenerateID))]
        void GenerateID()
        {
            UnityEditor.Undo.RegisterCompleteObjectUndo(this, "Generate ID");
            GetItem().GenerateID();
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif

        public abstract ItemBase GetItem();
    }
    
    public class ItemSO<T> : ItemSO where T : ItemBase
    {
        [SerializeField] T item;
     
        public override ItemBase GetItem()
        {
            return item;
        }
    }
}