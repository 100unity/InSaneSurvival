using Brush;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(CustomBrush))]
    public class BrushEditor : UnityEditor.Editor
    {
        private CustomBrush _customBrush;
        private BrushLogic _brushLogic;

        private void Awake()
        {
            _brushLogic = new BrushLogic();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            _customBrush = (CustomBrush)target;
            _brushLogic.InitVars(_customBrush);

            // ---- Custom Buttons ----
            AddButtons();
        }

        /// <summary>
        /// Unsubscribe brush if another object is selected.
        /// </summary>
        public void OnDisable()
        {
            _brushLogic.UnsubscribeBrush();
        }

        /// <summary>
        /// Add custom buttons.
        /// </summary>
        private void AddButtons()
        {
            if (GUILayout.Button(_brushLogic.IsEnabled ? "Disable" : "Enable"))
            {
                if (_brushLogic.IsEnabled)
                {
                    _brushLogic.UnsubscribeBrush();
                }
                else
                {
                    _brushLogic.SubscribeBrush();
                }
            }

            if (GUILayout.Button("New Parent"))
            {
                _brushLogic.CreateNewParent();
            }
        }
    }
}

