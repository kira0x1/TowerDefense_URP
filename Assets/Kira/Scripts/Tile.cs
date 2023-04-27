using UnityEngine;

namespace Kira
{
    public enum TileType
    {
        BuildingTile,
        RoadTile,
        VillageTile
    }

    public class Tile
    {
        public readonly int x;
        public readonly int y;
        public Vector3 worldPosition;
        public TileType tileType;

        public Tile(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}