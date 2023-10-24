using UnityEngine;

namespace XIV_Packages.InventorySystem.ScriptableObjects
{
    public abstract class ItemSO : ScriptableObject
    {
        public Sprite uiSprite;
        
#if UNITY_EDITOR
        [ContextMenu(nameof(GenerateID))]
        void GenerateID()
        {
            UnityEditor.Undo.RegisterCompleteObjectUndo(this, "Generate ID");
            GetItem().GenerateID();
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssetIfDirty(this);
        }
#endif

        public abstract ItemBase GetItem();
    }

    public abstract class ItemSO<T> : ItemSO where T : ItemBase
    {
        [SerializeField] T item;

        public override ItemBase GetItem() => item;
    }
}