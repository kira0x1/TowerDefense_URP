using Unity.Mathematics;
using UnityEngine;

namespace Kira.Procedural.Streams
{
    public interface IMeshStreams
    {
        void Setup(Mesh.MeshData data, int vertexCount, int indexCount);
        void SetVertex(int index, Vertex vertex);

        void SetTriangle(int index, int3 triangle);
    }
}