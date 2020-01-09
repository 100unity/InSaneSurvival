using System.Linq;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Editor.Brush
{
    public class BrushLogic
    {
        public bool IsEnabled { get; private set; }

        private Event _e;
        private GameObject _currentPlaceableObject;
        private float _mouseWheelRotation;

        private System.Random _random = new System.Random();
        private string _parentName;
        private Transform _parent;
        private int _parentNumber;
        private GameObject[] _prefabs;
        private Range _scaleXZ;
        private Range _scaleY;
        private bool _justScaleY;
        private bool _isTree;

        /// <summary>
        /// Store vars internally.
        /// </summary>
        public void InitVars(CustomBrush customBrush)
        {
            _scaleXZ = customBrush.ScaleXZ;
            _scaleY = customBrush.ScaleY;
            _prefabs = customBrush.Prefabs;
            _justScaleY = customBrush.JustScaleY;
            _isTree = customBrush.IsTree;

            // check if naming was changed and reset number
            if (_parentName != customBrush.ParentNaming)
            {
                _parentName = customBrush.ParentNaming;
                Reset();
            }
        }

        /// <summary>
        /// Subscribe the brush the the scene view hotkeys.
        /// </summary>
        public void SubscribeBrush()
        {
            IsEnabled = true;
            SceneView.duringSceneGui += Listen;
        }

        public void UnsubscribeBrush()
        {
            IsEnabled = false;
            SceneView.duringSceneGui -= Listen;
        }

        /// <summary>
        /// Reset parent counter.
        /// </summary>
        private void Reset()
        {
            IsEnabled = false;
            _parentNumber = 0;
            _parent = null;
        }

        /// <summary>
        /// Create new parent using the given name and a successive number.
        /// </summary>
        public void CreateNewParent()
        {
            string name;
            do
            {
                name = _parentName + _parentNumber;
                _parentNumber += 1;
            } while (GameObject.Find(name));

            GameObject parent = new GameObject(name);
            _parent = parent.transform;
        }

        /// <summary>
        /// On Alt + RightClickDown randomly spawns one of the prefabs of <see cref="_prefabs"/> and scales it randomly as 
        /// defined with <see cref="_scaleY"/> and <see cref="_scaleXZ"/>. The spawned object is child of the current <see cref="_parent"/>.
        /// If Alt + RightClick is hold down and an object spawned, moves the object to the current mouse position.
        /// On Alt + RightClickUp releases the spawned object and rotates it randomly.
        /// User-driven rotating seems not possible.
        /// </summary>
        /// <param name="obj">_</param>
        private void Listen(SceneView obj)
        {
            //InitVars();
            _e = Event.current;
            if (_e.isMouse && _e.button == 1 && _e.type == EventType.MouseDown && _e.modifiers == EventModifiers.Alt &&
                _currentPlaceableObject == null)
            {
                _e.Use();
                if (_parent == null)
                    CreateNewParent();
                int numberOfPrefabs = _prefabs.Count();
                GameObject toSpawn = _prefabs[_random.Next(numberOfPrefabs)];
                _currentPlaceableObject =
                    PrefabUtility.InstantiatePrefab(toSpawn,
                        _parent.transform) as GameObject; // instantiates prefabs, not clones (idk if that's helpful ;)
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
        /// Apply random scale to the spawned object using <see cref="_scaleY"/> and <see cref="_scaleXZ"/>.
        /// </summary>
        private void ApplyRandomScale()
        {
            float x = 0;
            float y = (float) (_random.NextDouble() * (_scaleY.max - _scaleY.min) + _scaleY.min);
            float z = 0;
            if (!_justScaleY)
            {
                x = (float) (_random.NextDouble() * (_scaleXZ.max - _scaleXZ.min) + _scaleXZ.min);
                z = (float) (_random.NextDouble() * (_scaleXZ.max - _scaleXZ.min) + _scaleXZ.min);
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
                if (!_isTree)
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