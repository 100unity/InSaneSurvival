using UnityEditor;
using UnityEngine;
using Utils;

namespace Editor
{
    [CustomPropertyDrawer(typeof(Range))]
    public class RangeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            float halfWidth = position.width / 2;
            float labelWidth = 26;
            
            Rect minLabelRect = new Rect(position.x, position.y, labelWidth, position.height);
            Rect maxLabelRect = new Rect(position.x + halfWidth, position.y, labelWidth, position.height);
            Rect minRect = new Rect(position.x + labelWidth, position.y, halfWidth - labelWidth, position.height);
            Rect maxRect = new Rect(position.x + labelWidth + halfWidth, position.y, halfWidth - labelWidth, position.height);
            
            EditorGUI.LabelField(minLabelRect, "Min");
            EditorGUI.LabelField(maxLabelRect, "Max");
            EditorGUI.PropertyField(minRect, property.FindPropertyRelative("min"), GUIContent.none);
            EditorGUI.PropertyField(maxRect, property.FindPropertyRelative("max"), GUIContent.none);
            
            EditorGUI.EndProperty();
        }
    }
}