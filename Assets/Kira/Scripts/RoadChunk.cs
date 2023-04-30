using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Kira
{
    [Serializable]
    public class RoadChunk
    {
        public List<Tile> roadTiles;
        public readonly Vector3 startPosition;
        public readonly int startX;
        public readonly int startY;
        public readonly Bounds bounds;

        public RoadChunk(int startX, int startY, Vector3 startPosition, int maxWidth, int maxLength)
        {
            this.startX = startX;
            this.startY = startY;
            this.startPosition = startPosition;

            CreateRoad(maxLength, maxWidth);


            Tile lastTile = roadTiles[^1];
            Vector3 lastPos = lastTile.worldPosition;

            Vector3 roadStartPos = startPosition;
            roadStartPos.x += startX;
            roadStartPos.z += startY;

            float sizeX = Mathf.Max(1, lastPos.x + 1 - roadStartPos.x);
            float sizeZ = Mathf.Max(1, lastPos.z + 1 - roadStartPos.z);

            Vector3 center = Vector3.Lerp(roadStartPos, lastPos, 0.5f);
            Vector3 size = new Vector3(sizeX, 0.5f, sizeZ);

            bounds = new Bounds(center, size);
        }

        private void CreateRoad(int maxLength, int maxWidth)
        {
            roadTiles = new List<Tile>();

            int minLength = Mathf.Min(startY + 2, maxLength);
            int roadLength = Random.Range(minLength, maxLength);

            int minWidth = Mathf.Min(startX + 2, maxWidth);
            int roadWidth = Random.Range(minWidth, maxWidth);

            Vector3 branchPos = startPosition;
            branchPos.z += startY;
            branchPos.x += startX;

            for (int y = startY; y < roadLength; y++)
            {
                for (int x = startX; x < roadWidth; x++)
                {
                    Vector3 pos = startPosition;
                    pos.x += x;
                    pos.z += y;

                    Tile tile = new Tile(x, y);
                    tile.tileType = TileType.RoadTile;
                    if (y == startY && x == startX) tile.tileType = TileType.BranchTile;
                    tile.worldPosition = pos;
                    roadTiles.Add(tile);
                }
            }

            // roadTiles[^1].tileType = TileType.VillageTile;
        }

        public bool HasRoadTile(int x, int y)
        {
            if (x < 0 || y < 0) return false;
            return roadTiles.Any(t => t.Equals(x, y));
        }
    }
}