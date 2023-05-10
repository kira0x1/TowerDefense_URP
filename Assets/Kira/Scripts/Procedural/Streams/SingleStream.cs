using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace Kira.Procedural.Streams
{
    public class SingleStream : IMeshStreams
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct Stream0
        {
            public float3 position, normal;
            public half4 tangent;
            public half2 texCoord0;
        }

        private NativeArray<Stream0> stream0;
        private NativeArray<int3> triangles;

        public void Setup(Mesh.MeshData data, int vertexCount, int indexCount)
        {
            var descriptor = new NativeArray<VertexAttributeDescriptor>(4, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
            descriptor[0] = new VertexAttributeDescriptor(dimension: 3);
            descriptor[1] = new VertexAttributeDescriptor(VertexAttribute.Normal, dimension: 3);
            descriptor[2] = new VertexAttributeDescriptor(VertexAttribute.Tangent, dimension: 4);
            descriptor[3] = new VertexAttributeDescriptor(VertexAttribute.TexCoord0, dimension: 2);

            data.SetVertexBufferParams(vertexCount, descriptor);
            descriptor.Dispose();

            data.SetIndexBufferParams(indexCount, IndexFormat.UInt32);

            data.subMeshCount = 1;
            data.SetSubMesh(0, new SubMeshDescriptor(0, indexCount));

            stream0 = data.GetVertexData<Stream0>();
            triangles = data.GetIndexData<int>().Reinterpret<int3>(4);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetVertex(int index, Vertex vertex)
        {
            stream0[index] = new Stream0
            {
                position = vertex.position,
                normal = vertex.normal,
                tangent = vertex.tangent,
                texCoord0 = vertex.texCoord0
            };
        }

        public void SetTriangle(int index, int3 triangle) => triangles[index] = triangle;
    }
}