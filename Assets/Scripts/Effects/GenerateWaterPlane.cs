using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Effects
{
    [ExecuteInEditMode]
    public class GenerateWaterPlane : MonoBehaviour
    {
        [SerializeField][Tooltip("Size of the mesh")]
        private float size = 1f;
    
        [SerializeField][Tooltip("Subdivisions of the mesh")]
        private int gridSize = 16;

        private MeshFilter _filter;

        // Start is called before the first frame update
        void Start()
        {
            _filter = GetComponent<MeshFilter>();
            _filter.mesh = GenerateMesh();
        }

        private Mesh GenerateMesh()
        {
            Mesh m = new Mesh();
            List<Vector3> vertices = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();
            
            int vertCount = gridSize + 1;
        
            for(int x = 0; x < vertCount; x++)
            {
                for (int y = 0; y < vertCount; y++)
                {
                    vertices.Add(new Vector3(-size * .5f + size * (x / ((float)gridSize)), 0, -size * .5f + size * (y / ((float)gridSize))));
                    normals.Add(Vector3.up);
                    uvs.Add(new Vector2(x / (float)gridSize, y / (float)gridSize));
                }
            }

            List<int> triangles = new List<int>();

            for (int i = 0; i < vertCount * vertCount - vertCount; i++)
            {
                if ((i + 1) % vertCount == 0)
                {
                    continue;
                }
                triangles.AddRange(new List<int>() {
                    i + 1 + vertCount, i + vertCount, i,
                    i, i + 1, i + vertCount + 1
                });
            }

            m.SetVertices(vertices);
            m.SetNormals(normals);
            m.SetUVs(0, uvs);
            m.SetTriangles(triangles, 0);

            return m;
        }
    }
}


