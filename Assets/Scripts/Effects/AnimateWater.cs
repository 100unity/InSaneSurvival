using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Effects
{
    public class AnimateWater : MonoBehaviour
    {
        [SerializeField] [Tooltip("Determines the difference between the peak and bottom of a wave")]
        private float power;
    
        [SerializeField] [Tooltip("Determines the size of waves")]
        private float scale;
    
        [SerializeField] [Tooltip("Determines how fast waves aree")]
        private float frequency;

        private float _xOffset, _yOffset;
        private MeshFilter _meshFilter;


        private void Awake()
        {
            _meshFilter = GetComponent<MeshFilter>();
        }
    
        private void Update()
        {
            AnimateWaves();
            _xOffset += Time.deltaTime * frequency;
            if(_yOffset <= 0.3) _yOffset += Time.deltaTime * frequency;
            if (_yOffset >= power) _yOffset -= Time.deltaTime * frequency;
        }
    
        private void AnimateWaves()
        {
            Vector3[] vertices = _meshFilter.mesh.vertices;

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].y = CalculateHeight(vertices[i].x, vertices[i].z) * power;
            }

            _meshFilter.mesh.vertices = vertices;
        }

        private float CalculateHeight(float x, float y)
        {
            float xCord = x * scale + _xOffset;
            float yCord = y * scale + _yOffset;

            return Mathf.PerlinNoise(xCord, yCord);
        }
    }
}
