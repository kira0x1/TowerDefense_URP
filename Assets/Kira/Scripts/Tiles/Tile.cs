﻿using System;
using UnityEngine;

namespace Kira
{
    public enum TileType
    {
        BuildingTile,
        RoadTile,
        VillageTile,
        BranchTile
    }

    public class Tile : IEquatable<Tile>
    {
        public readonly string tileName;
        public readonly int x;
        public readonly int y;
        public Vector3 worldPosition;
        public TileType tileType;

        public Tile(int x, int y, TileType tileType)
        {
            this.x = x;
            this.y = y;

            this.tileType = tileType;
            this.tileName = "Normal Tile";

            if (tileType == TileType.RoadTile)
            {
                tileName = "Road Tile";
            }
            else if (tileType == TileType.VillageTile)
            {
                tileName = "Village Tile";
            }
        }

        public bool Equals(Tile other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return x == other.x && y == other.y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((Tile)obj);
        }

        public bool Equals(int x, int y)
        {
            return this.x == x && this.y == y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y);
        }

        public override string ToString()
        {
            return tileName;
        }
    }
}