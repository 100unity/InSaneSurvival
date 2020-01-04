using Brush;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(CustomBrush))]
    public class BrushEditor : UnityEditor.Editor
    {
        private Event _e;
        private GameObject _currentPlaceableObject;
        private float _mouseWheelRotation;

        private bool _isEnabled;
        private System.Random _random = new System.Random();

        private CustomBrush _customBrush;
        private string _parentName;
        private Transform _parent;
        private int _parentNumber;
        private float _minScale;
        private float _maxScale;
        private bool _justScaleY;
        private GameObject[] _prefabs;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            _customBrush = (CustomBrush)target;
            InitVars();

            // ---- Custom Buttons ----
            AddButtons();
        }

        /// <summary>
        /// Unsubscribe brush if another object is selected.
        /// </summary>
        public void OnDisable()
        {
            UnsubscribeBrush();
        }

        /// <summary>
        /// Store vars internally.
        /// </summary>
        private void InitVars()
        {
            _minScale = _customBrush.MinScale;
            _maxScale = _customBrush.MaxScale;
            _prefabs = _customBrush.Prefabs;
            _justScaleY = _customBrush.JustScaleY;

            // check if naming was changed and reset number
            if (_parentName != _customBrush.ParentNaming)
            {
                _parentName = _customBrush.ParentNaming;
                Reset();
            }
        }

        /// <summary>
        /// Add custom buttons.
        /// </summary>
        private void AddButtons()
        {
            if (GUILayout.Button(_isEnabled ? "Disable" : "Enable"))
            {
                if (_isEnabled)
                {
                    _isEnabled = false;
                    UnsubscribeBrush();
                }
                else
                {
                    _isEnabled = true;
                    SubscribeBrush();
                }
            }

            if (GUILayout.Button("New Parent"))
            {
                CreateNewParent();
            }
        }

        /// <summary>
        /// Subscribe the brush the the scene view hotkeys.
        /// </summary>
        public void SubscribeBrush()
        {
            SceneView.duringSceneGui += Listen;
        }

        public void UnsubscribeBrush()
        {
            SceneView.duringSceneGui -= Listen;
        }

        /// <summary>
        /// Reset parent counter.
        /// </summary>
        private void Reset()
        {
            _parentNumber = 0;
            _parent = null;
        }

        /// <summary>
        /// Create new parent using the given name and a successive number.
        /// </summary>
        private void CreateNewParent()
        {
            do
            {
                name = _parentName + _parentNumber;
                _parentNumber += 1;
            }
            while (GameObject.Find(name));

            GameObject parent = new GameObject(name);
            _parent = parent.transform;
        }

        /// <summary>
        /// On Alt + RightClickDown randomly spawns one of the prefabs of <see cref="_prefabs"/> and scales it randomly as 
        /// defined with <see cref="_minScale"/> and <see cref="_maxScale"/>. The spawned object is child of the current <see cref="_parent"/>.
        /// If Alt + RightClick is hold down and an object spawned, moves the object to the current mouse position.
        /// On Alt + RightClickUp releases the spawned object and rotates it randomly.
        /// User-driven rotating seems not possible.
        /// </summary>
        /// <param name="obj">_</param>
        private void Listen(SceneView obj)
        {
            InitVars();
            _e = Event.current;
            if (_e.isMouse && _e.button == 1 && _e.type == EventType.MouseDown && _e.modifiers == EventModifiers.Alt && _currentPlaceableObject == null)
            {
                _e.Use();
                if (_parent == null)
                    CreateNewParent();
                int numberOfPrefabs = _prefabs.Count();
                GameObject toSpawn = _prefabs[_random.Next(numberOfPrefabs)];
                _currentPlaceableObject = PrefabUtility.InstantiatePrefab(toSpawn, _parent.transform) as GameObject; // instantiates prefabs, not clones (idk if that's helpful ;)
                ApplyRandomScale();
            }

            // object is spawned
            if (_currentPlaceableObject != null)
            {
                MoveCurrentObject();
                //RotateFromMouseWheel();
                if (_e.isMouse && _e.button == 1 && _e.type == EventType.MouseUp && _e.modifiers == EventModifiers.Alt)
                    ReleaseObject();
            }
        }

        /// <summary>
        /// Apply random scale to the spawned object using <see cref="_minScale"/> and <see cref="_maxScale"/>.
        /// </summary>
        private void ApplyRandomScale()
        {
            float x = 0;
            float y = (float)(_random.NextDouble() * (_maxScale - _minScale) + _minScale);
            float z = 0;
            if (!_justScaleY)
            {
                x = (float)(_random.NextDouble() * (_maxScale - _minScale) + _minScale);
                z = (float)(_random.NextDouble() * (_maxScale - _minScale) + _minScale);
            }
            _currentPlaceableObject.transform.localScale += new Vector3(x, y, z);
        }

        /// <summary>
        /// Releases the spawned object at the current position and rotates it randomly.
        /// </summary>
        private void ReleaseObject()
        {
            _e.Use();
            // apply some random rotation since mouse wheel doesn't work
            int rotation = _random.Next(360);
            _currentPlaceableObject.transform.Rotate(Vector3.up, rotation);
            _currentPlaceableObject = null;
            _mouseWheelRotation = 0;
        }

        /// <summary>
        /// Moves the spawned object to the current mouse position and rotates it so that it's always orthogonal to the ground.
        /// </summary>
        private void MoveCurrentObject()
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("Ground")))
            {
                _currentPlaceableObject.transform.position = hit.point;
                _currentPlaceableObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            }
        }

        /// <summary>
        /// Doesn't work while 
        /// _currentPlaceableObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        /// is used.
        /// </summary>
        private void RotateFromMouseWheel()
        {
            if (_e.isScrollWheel && _e.modifiers == EventModifiers.Alt)
            {
                _e.Use();
                _mouseWheelRotation += _e.delta.y;
                _currentPlaceableObject.transform.Rotate(Vector3.up, _mouseWheelRotation * 10f);
            }
        }
    }
}

