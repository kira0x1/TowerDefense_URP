using UnityEngine;
using Grid = Kira.GridGen.Grid;

namespace Kira
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class TileMesh : MonoBehaviour
    {
        private Grid grid;
        private Vector3[] vertices;
        private int[] triangles;
        private int triangleIndex;
        private int vertexIndex;
        private Vector2 uv;

        private MeshRenderer meshRenderer;
        private MeshFilter meshFilter;

        private void Start()
        {
            grid = FindObjectOfType<TileGenerator>().grid;
            meshFilter = GetComponent<MeshFilter>();
            meshRenderer = GetComponent<MeshRenderer>();
            CreateMesh();
        }

        private void CreateMesh()
        {
            const int gridX = 2;
            const int gridY = 2;

            vertices = new Vector3[gridX * gridY * 6];
            triangles = new int[vertices.Length * 3];

            float cellSize = grid.cellSize;
            triangleIndex = 0;
            vertexIndex = 0;

            Mesh mesh = meshFilter.mesh;

            for (int y = 0; y < gridY; y++)
            {
                for (int x = 0; x < gridX; x++)
                {
                    float xPos = x * cellSize;
                    float yPos = y * cellSize;

                    Vector3 v0 = new Vector3(xPos, 0f, yPos);
                    Vector3 v1 = new Vector3(xPos, 0f, yPos + cellSize);
                    Vector3 v2 = new Vector3(xPos + cellSize, 0f, yPos);
                    Vector3 v3 = new Vector3(xPos + cellSize, 0f, yPos + cellSize);

                    vertices[vertexIndex] = v0;
                    vertices[vertexIndex + 1] = v1;
                    vertices[vertexIndex + 2] = v2;
                    vertices[vertexIndex + 3] = v3;
                    vertexIndex += 4;

                    int a = triangleIndex;
                    int b = triangleIndex + 1;
                    int c = triangleIndex + 2;
                    int d = triangleIndex + 3;

                    SetTriangles(a, b, c, d);
                }
            }


            mesh.vertices = vertices;
            mesh.triangles = triangles;
            meshFilter.mesh = mesh;
        }

        private void SetUV(Vector3 vertex)
        {
        }

        private void SetTriangles(int a, int b, int c, int d)
        {
            triangles[triangleIndex] = a;
            triangles[triangleIndex + 1] = b;
            triangles[triangleIndex + 2] = c;

            triangles[triangleIndex + 3] = c;
            triangles[triangleIndex + 4] = d;
            triangles[triangleIndex + 5] = b;

            triangleIndex += 6;
        }
    }
}