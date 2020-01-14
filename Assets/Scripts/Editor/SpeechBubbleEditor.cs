using UI;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(SpeechBubble))]
    public class SpeechBubbleEditor : UnityEditor.Editor
    {
        private static Sprite image;
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!EditorApplication.isPlaying) return;

            SpeechBubble speechBubble = (SpeechBubble) target;
            
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Testing UI for Development", EditorStyles.boldLabel);
            image = (Sprite) EditorGUILayout.ObjectField("Image to display", image, typeof(Sprite), false);

            if (GUILayout.Button("Show speech bubble") && image != null)
                speechBubble.ShowSpeechBubble(image);
        }
    }
}