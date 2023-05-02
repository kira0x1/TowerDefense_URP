using System;
using UnityEngine;

namespace Kira
{
    [Serializable]
    public class RoadChunkPlan
    {
        [HideInInspector]
        public Vector3 startPosition;

        public bool startAtLastEnd;
        public bool randomizeLength;

        public int startX;
        public int startY;

        [Range(1, 100)]
        public int maxWidth;
        [Range(1, 100)]
        public int maxLength;

        public RoadChunkPlan(Vector3 startPosition, int startX, int startY, int maxWidth, int maxLength)
        {
            this.startPosition = startPosition;
            this.startX = startX;
            this.startY = startY;
            this.maxWidth = maxWidth;
            this.maxLength = maxLength;
        }

        public RoadChunk CreateRoad()
        {
            return new RoadChunk(startX, startY, startPosition, maxWidth, maxLength, randomizeLength);
        }
    }
}