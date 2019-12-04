using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(Inventory))]
    public class InventoryEditor : UnityEditor.Editor
    {
        public Item item;
        private bool _showCustomInspector;
        
        public override void OnInspectorGUI()
        {
            Inventory inventory = (Inventory) target;

            _showCustomInspector =
                EditorGUILayout.BeginFoldoutHeaderGroup(_showCustomInspector,
                    "Add items to inventory (for dev purposes)");
            
            if (_showCustomInspector)
            {
                item = (Item) EditorGUILayout.ObjectField("Item to add", item, typeof(Item), true);

                if (GUILayout.Button("Add Items"))
                    inventory.Add(item);
            }
            
            EditorGUILayout.EndFoldoutHeaderGroup();
            
            base.OnInspectorGUI();
        }
    }
}