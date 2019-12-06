using Inventory;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(InventoryController))]
    public class InventoryEditor : UnityEditor.Editor
    {
        public AUsable item;
        private bool _showCustomInspector;
        
        public override void OnInspectorGUI()
        {
            InventoryController inventory = (InventoryController) target;

            _showCustomInspector =
                EditorGUILayout.BeginFoldoutHeaderGroup(_showCustomInspector,
                    "Add items to inventory (for dev purposes)");
            
            if (_showCustomInspector)
            {
                item = (AUsable) EditorGUILayout.ObjectField("Item to add", item, typeof(AUsable), true);

                if (GUILayout.Button("Add Items"))
                    inventory.Add(item);
            }
            
            EditorGUILayout.EndFoldoutHeaderGroup();
            
            base.OnInspectorGUI();
        }
    }
}