using UnityEditor;
using UnityEngine;
using Utils;

namespace Editor
{
    [CustomPropertyDrawer(typeof(EnumMultiSelect))]
    public class EnumMultiSelectDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.intValue = EditorGUI.MaskField(position, label, property.intValue, property.enumNames);
        }
    }
}