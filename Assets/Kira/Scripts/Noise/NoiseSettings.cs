using UnityEngine;

namespace Kira.Noise
{
    [System.Serializable]
    public struct NoiseSettings
    {
        public int width;
        public int height;
        public Vector2 offset;
        [Range(1f, 256f)] public float scale;
        [Range(1, 24)] public int octaves;
        [Range(0.01f, 2f)] public float lacunarity;
        [Range(0.01f, 2f)] public float persistance;
        [Range(0f, 10f)] public float heightMultiplier;
        public int seed;

        public NoiseSettings(int width, int height, float scale, Vector2 offset, int octaves, float lacunarity, float persistance, int seed, float heightMultiplier)
        {
            this.persistance = persistance;
            this.lacunarity = lacunarity;
            this.octaves = octaves;
            this.scale = scale;
            this.height = height;
            this.width = width;
            this.offset = offset;
            this.seed = seed;
            this.heightMultiplier = heightMultiplier;
        }
    }
}