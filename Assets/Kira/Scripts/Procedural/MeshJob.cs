using Kira.Procedural.Generators;
using Kira.Procedural.Streams;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Kira.Procedural
{
    [BurstCompile(FloatPrecision.Standard, FloatMode.Fast, CompileSynchronously = true)]
    public struct MeshJob<G, S> : IJobFor
        where G : struct, IMeshGenerator
        where S : struct, IMeshStreams
    {
        private G generator;

        [WriteOnly]
        private S streams;

        public static JobHandle ScheduleParallel(Mesh mesh, Mesh.MeshData meshData, JobHandle dependency)
        {
            var job = new MeshJob<G, S>();
            job.streams.Setup(meshData,
                mesh.bounds = job.generator.Bounds,
                job.generator.VertexCount,
                job.generator.IndexCount);
            return job.ScheduleParallel(job.generator.JobLength, 1, dependency);
        }

        public void Execute(int i) => generator.Execute(i, streams);
    }
}