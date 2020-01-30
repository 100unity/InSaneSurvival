using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Utils
{
    /// <summary>
    /// From <see cref="http://wiki.unity3d.com/index.php/FramesPerSecond"/>
    /// </summary>
    public class FPSCounter : MonoBehaviour
    {
        private float _deltaTime;

        private readonly List<float> _averages = new List<float>();

        private float _average;

        private void Awake()
        {
            if (!Debug.isDebugBuild)
                DestroyImmediate(gameObject);

            for (int i = 0; i < 100; i++) 
                _averages.Add(0);
        }

        private void Update()
        {
            _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
        }

        private void OnGUI()
        {
            int w = Screen.width, h = Screen.height;

            GUIStyle style = new GUIStyle();

            Rect rect = new Rect(0, 0, w, h * 2 / 100);
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = h * 2 / 100;
            style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
            float msec = _deltaTime * 1000.0f;
            float fps = 1.0f / _deltaTime;
            _averages.Add(fps);
            _averages.Remove(_averages.First());
            _average = _averages.Sum() / _averages.Count;
            string text = string.Format("{0:0.0} ms ({1:0.} fps) | Avg: {2:0.}", msec, fps, _average);
            GUI.Label(rect, text, style);
        }
    }
}