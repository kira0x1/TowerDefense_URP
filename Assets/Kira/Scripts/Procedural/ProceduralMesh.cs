using Kira.Procedural.Generators;
using Kira.Procedural.Streams;
using UnityEngine;

namespace Kira.Procedural
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class ProceduralMesh : MonoBehaviour
    {
        private Mesh mesh;

        private void Awake()
        {
            mesh = new Mesh
            {
                name = "Procedural Mesh"
            };

            GenerateMesh();
            GetComponent<MeshFilter>().mesh = mesh;
        }

        private void GenerateMesh()
        {
            Mesh.MeshDataArray meshDataArray = Mesh.AllocateWritableMeshData(1);
            Mesh.MeshData meshData = meshDataArray[0];

            MeshJob<SquareGrid, SingleStream>.ScheduleParallel(
                mesh, meshData, default
            ).Complete();

            Mesh.ApplyAndDisposeWritableMeshData(meshDataArray, mesh);
        }
    }
}