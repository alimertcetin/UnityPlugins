using UnityEditor;
using UnityEngine;
using XIV.Core.Utils;
using XIV_Packages.InventorySystem.ScriptableObjects.NonSerializedData;

namespace XIV_Packages.InventorySystem.ScriptableObjects.Editor
{
    [CustomEditor(typeof(NonSerializedItemDataSO), true, isFallback = false), CanEditMultipleObjects]
    public class NonSerializedItemDataSOEditor : UnityEditor.Editor
    {
        static bool isCached;
        static GUIContent cached;

        void OnEnable()
        {
            isCached = false;
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            base.OnInspectorGUI();
            if (EditorGUI.EndChangeCheck()) isCached = false;
            
            var itemDataSO = (NonSerializedItemDataSO)target;
            if (isCached == false)
            {
                if (itemDataSO.uiSprite != null)
                {
                    cached = new GUIContent(TextureUtils.CreateTexture(itemDataSO.uiSprite));
                    isCached = true;
                }
            }

            if (isCached) GUILayout.Label(cached);
        }
    }
}