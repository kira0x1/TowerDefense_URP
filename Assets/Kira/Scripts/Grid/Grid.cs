using UnityEngine;

namespace Kira.GridGen
{
    public class Grid
    {
        public readonly int width;
        public readonly int height;
        public readonly float cellSize;
        public Tile[,] tiles;
        private readonly TextMesh[,] gridText;
        public readonly Vector3 originPos;
        private readonly int fontSize;
        private readonly bool spawnText;
        private readonly bool centerY;
        private readonly int villageOffset;

        public Grid(int width, int height, float cellSize, int fontSize, Vector3 originPos, bool centerY, int villageOffset)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            this.fontSize = fontSize;
            this.originPos = originPos;
            this.centerY = centerY;
            this.villageOffset = villageOffset;

            tiles = new Tile[width, height];
            gridText = new TextMesh[width, height];

            GenerateTiles();
        }

        public void SpawnText(Transform parent, Quaternion rotation)
        {
            Transform textParent = new GameObject("Grid Text").transform;
            textParent.SetParent(parent);

            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    Vector3 pos = GetWorldPosition(x, y) + new Vector3(cellSize, 0, cellSize) * 0.5f;

                    TextMesh text = GridUtils.CreateWorldText(tiles[x, y].ToString(), textParent, pos, fontSize, Color.white);
                    text.transform.rotation = rotation;
                    gridText[x, y] = text;
                }
            }
        }

        private int GetVillageY()
        {
            int villageIndex = centerY ? height / 2 : 0;
            villageIndex += villageOffset;
            return villageIndex;
        }


        public void GenerateTiles()
        {
            tiles = new Tile[width, height];

            Vector3 startPosition = originPos;
            startPosition.x += cellSize / 2f;
            startPosition.z += cellSize / 2f;

            int villageIndex = GetVillageY();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    bool isRoadTile = x == width / 2 && y >= villageIndex + 1;
                    bool isVillageTile = x == width / 2 && y == villageIndex;

                    Vector3 pos = startPosition;
                    pos.x += x * cellSize;
                    pos.z += y * cellSize;

                    TileType tileType = TileType.BasicTile;

                    if (isRoadTile)
                    {
                        tileType = TileType.RoadTile;
                    }
                    else if (isVillageTile)
                    {
                        tileType = TileType.VillageTile;
                    }
                    else if (x == width / 2 && y >= villageIndex + 1)
                    {
                        tileType = TileType.RoadTile;
                    }

                    Tile tile = new Tile(x, y, tileType);
                    tile.worldPosition = pos;
                    tiles[x, y] = tile;
                }
            }
        }

        public Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x, 0, y) * cellSize + originPos;
        }

        private void GetXY(Vector3 worldPosition, out int x, out int y)
        {
            x = Mathf.FloorToInt((worldPosition - originPos).x / cellSize);
            y = Mathf.FloorToInt((worldPosition - originPos).z / cellSize);
        }

        public void SetTile(int x, int y, Tile tile)
        {
            if (x < 0 || y < 0) return;
            if (x >= width || y >= height) return;

            tiles[x, y] = tile;

            if (gridText.GetLength(0) > width && gridText.GetLength(1) > y)
                gridText[x, y].text = tile.ToString();
        }

        public void SetTile(Vector3 worldPosition, Tile tile)
        {
            GetXY(worldPosition, out int x, out int y);
            SetTile(x, y, tile);
        }

        public Tile GetTile(int x, int y)
        {
            if (x < 0 || y < 0) return tiles[0, 0];
            if (x >= width || y >= height) return tiles[tiles.GetLength(0) - 1, tiles.GetLength(1) - 1];
            return tiles[x, y];
        }

        public Tile GetTile(int x, int y, out bool hasTile)
        {
            hasTile = true;
            if (x < 0 || y < 0) hasTile = false;
            if (x >= width || y >= height) hasTile = false;
            return GetTile(x, y);
        }

        public Tile GetTile(Vector3 worldPosition)
        {
            GetXY(worldPosition, out int x, out int y);
            return GetTile(x, y);
        }

        public Tile GetTile(Vector3 worldPosition, out bool hasTile)
        {
            GetXY(worldPosition, out int x, out int y);
            return GetTile(x, y, out hasTile);
        }
    }
}