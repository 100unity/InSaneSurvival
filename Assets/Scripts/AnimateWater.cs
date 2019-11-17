using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateWater : MonoBehaviour
{
    public float power = 3f;
    public float scale = 1f;
    public float frequency = 1;

    private float xOffset, yOffset;
    private MeshFilter mf;

    // Start is called before the first frame update
    void Start()
    {
        mf = GetComponent<MeshFilter>();
        AnimateWaves();
    }

    // Update is called once per frame
    void Update()
    {
        AnimateWaves();
        xOffset += Time.deltaTime * frequency;
        if(yOffset <= 0.3) yOffset += Time.deltaTime * frequency;
        if (yOffset >= power) yOffset -= Time.deltaTime * frequency;



    }

    void AnimateWaves()
    {
        Vector3[] vertices = mf.mesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i].y = CalculateHeight(vertices[i].x, vertices[i].z) * power;
        }

        mf.mesh.vertices = vertices;
    }

    float CalculateHeight(float x, float y)
    {
        float xCord = x * scale + xOffset;
        float yCord = y * scale + yOffset;

        return Mathf.PerlinNoise(xCord, yCord);
    }
}
