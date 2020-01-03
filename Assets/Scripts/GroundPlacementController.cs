using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class EditorHotkeysTracker
{
    static EditorHotkeysTracker()
    {
        SceneView.duringSceneGui -= (view) => Method();
    }

    private static void Method()
    {
        var e = Event.current;
        if (e != null && e.keyCode == KeyCode.A && e.type == EventType.KeyDown)
        {
            
        }
            
    }
    /*
    private static void HandleKeyDown()
    {
        if (_currentPlaceableObject == null)
        {
            _currentPlaceableObject = Instantiate(placeableObjectPrefab);
        }
        else
        {
            Destroy(_currentPlaceableObject);
        }
    }*/
}
/*
public class GroundPlacementController : MonoBehaviour
{
    [SerializeField]
    private GameObject placeableObjectPrefab;

    [SerializeField]
    private KeyCode hotkey = KeyCode.A;

    private GameObject _currentPlaceableObject;
    private float _mouseWheelRotation;

    private void Update()
    {
        HandleHotkey();

        if (_currentPlaceableObject != null)
        {
            MoveCurrentPlaceableObjectToMouse();
            RotateFromMouseWheel();
            ReleaseIfClicked();
        }
    }

    private void ReleaseIfClicked()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _currentPlaceableObject = null;
        }
    }

    private void RotateFromMouseWheel()
    {
        _mouseWheelRotation += Input.mouseScrollDelta.y;
        _currentPlaceableObject.transform.Rotate(Vector3.up, _mouseWheelRotation * 10f);
    }

    private void MoveCurrentPlaceableObjectToMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            _currentPlaceableObject.transform.position = hit.point;
            _currentPlaceableObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        }
    }

    private void HandleHotkey()
    {
        if (Input.GetKeyDown(hotkey))
        {
            if (_currentPlaceableObject == null)
            {
                _currentPlaceableObject = Instantiate(placeableObjectPrefab);
            }
            else
            {
                Destroy(_currentPlaceableObject);
            }
        }
    }
}*/
