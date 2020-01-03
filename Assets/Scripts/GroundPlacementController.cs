using System;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class EditorHotkeysTracker
{
    static Event e;
    static GameObject _currentPlaceableObject;
    static float _mouseWheelRotation;
    static System.Random random = new System.Random();
    static int numberOfPrefabs = 9;
    static double scaleMin = -1;
    static double scaleMax = 1;

    static EditorHotkeysTracker()
    {
        SceneView.duringSceneGui += (view) => Update();
    }

    private static void Update()
    {
        e = Event.current;
        if (e.isMouse && e.button == 1 && e.type == EventType.MouseDown && e.modifiers == EventModifiers.Alt && _currentPlaceableObject == null)
        {
            e.Use();
            UnityEngine.Object go = AssetDatabase.LoadAssetAtPath(GetRandomPrefab(), typeof(GameObject));
            _currentPlaceableObject = PrefabUtility.InstantiatePrefab(go, UnityEngine.SceneManagement.SceneManager.GetActiveScene()) as GameObject;
            ApplyRandomScale();
        }

        if (_currentPlaceableObject != null)
        {
            MoveCurrentObject();
            //RotateFromMouseWheel();
            if (e.isMouse && e.button == 1 && e.type == EventType.MouseUp && e.modifiers == EventModifiers.Alt)
                ReleaseObject();
        }
    }

    private static void ApplyRandomScale()
    {
        float x = (float)(random.NextDouble() * (scaleMax - scaleMin) + scaleMin);
        float y = (float)(random.NextDouble() * (scaleMax - scaleMin) + scaleMin);
        float z = (float)(random.NextDouble() * (scaleMax - scaleMin) + scaleMin);
        _currentPlaceableObject.transform.localScale += new Vector3(x, y, z);
    }

    private static string GetRandomPrefab()
    {
        String path = "Assets/Prefabs/Plants/Grass/Grass{0}.prefab";
        int number = random.Next(numberOfPrefabs);
        return String.Format(path, number);
    }

    private static void ReleaseObject()
    {
        e.Use();
        // apply some random rotation since mouse wheel doesn't work
        int rotation = random.Next(360);
        _currentPlaceableObject.transform.Rotate(Vector3.up, rotation);
        _currentPlaceableObject = null;
        _mouseWheelRotation = 0;
    }

    /// <summary>
    /// Doesn't work while 
    /// _currentPlaceableObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
    /// is used.
    /// </summary>
    private static void RotateFromMouseWheel()
    {
        if (e.isScrollWheel && e.modifiers == EventModifiers.Alt)
        {
            e.Use();
            _mouseWheelRotation += e.delta.y;
            //Debug.Log(_mouseWheelRotation*10);
            _currentPlaceableObject.transform.Rotate(Vector3.up, _mouseWheelRotation * 10f, Space.World);
        }
    }

    private static void MoveCurrentObject()
    {
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("Ground")))
        {
            _currentPlaceableObject.transform.position = hit.point;
            _currentPlaceableObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        }
    }
}