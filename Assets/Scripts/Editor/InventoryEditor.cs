using Inventory;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(InventoryController))]
    public class InventoryEditor : UnityEditor.Editor
    {
        public Item item;
        private bool _showCustomInspector;
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            if (!EditorApplication.isPlaying) return;

            InventoryController inventory = (InventoryController) target;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Testing UI for Development", EditorStyles.boldLabel);
            item = (Item) EditorGUILayout.ObjectField("Item to add", item, typeof(Item), true);

            if (GUILayout.Button("Add Items"))
                inventory.AddItem(item);
        }
    }
}