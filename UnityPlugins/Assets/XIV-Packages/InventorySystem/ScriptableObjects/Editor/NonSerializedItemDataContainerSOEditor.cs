using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XIV.XIVEditor.Utils;
using XIV_Packages.InventorySystem.ScriptableObjects.NonSerializedData;

namespace XIV_Packages.InventorySystem.ScriptableObjects.Editor
{
    [CustomEditor(typeof(NonSerializedItemDataContainerSO))]
    public class NonSerializedItemDataContainerSOEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            NonSerializedItemDataContainerSO container = (NonSerializedItemDataContainerSO)target;

            if (GUILayout.Button("Load Data Containers"))
            {
                List<NonSerializedItemDataSO> dataContainers = AssetUtils.LoadAssetsOfType<NonSerializedItemDataSO>("Assets/ScriptableObjects");
                
                Undo.RecordObject(container, "Load Data Containers");
                container.itemDataPairs = new NonSerializedItemDataSO[dataContainers.Count];
                for (int i = 0; i < dataContainers.Count; i++)
                {
                    container.itemDataPairs[i] = dataContainers[i];
                }
            }
            
            base.OnInspectorGUI();
        }
    }
}