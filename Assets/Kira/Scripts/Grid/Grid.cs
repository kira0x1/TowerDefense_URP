using UnityEngine;

namespace Kira.GridGen
{
    public class Grid
    {
        public readonly int width;
        public readonly int height;
        private readonly float cellSize;
        private readonly Tile[,] gridValues;
        private readonly TextMesh[,] gridText;
        private readonly GameObject[,] gridMesh;
        private readonly Vector3 originPos;
        private readonly int fontSize;
        private readonly bool spawnText;

        public Grid(int width, int height, float cellSize, int fontSize, Vector3 originPos)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            this.fontSize = fontSize;
            this.originPos = originPos;

            gridValues = new Tile[width, height];
            gridText = new TextMesh[width, height];
            gridMesh = new GameObject[width, height];
        }

        public void SpawnText(Transform parent, Quaternion rotation)
        {
            Transform textParent = new GameObject("Grid Text").transform;
            textParent.SetParent(parent);

            for (int x = 0; x < gridValues.GetLength(0); x++)
            {
                for (int y = 0; y < gridValues.GetLength(1); y++)
                {
                    Vector3 pos = GetWorldPosition(x, y) + new Vector3(cellSize, 0, cellSize) * 0.5f;

                    TextMesh text = GridUtils.CreateWorldText(gridValues[x, y].ToString(), textParent, pos, fontSize, Color.white);
                    text.transform.rotation = rotation;
                    gridText[x, y] = text;
                }
            }
        }

        public void SpawnGrid(TileGenerator tileGenerator, Transform parent, Quaternion rotation)
        {
            Transform tileParent = new GameObject("Grid Tiles").transform;
            tileParent.SetParent(parent);

            for (int x = 0; x < gridValues.GetLength(0); x++)
            {
                for (int y = 0; y < gridValues.GetLength(1); y++)
                {
                    Vector3 pos = GetWorldPosition(x, y) + new Vector3(cellSize, 0, cellSize) * 0.5f;

                    Vector3 tilePos = pos;
                    tilePos.y -= 0.5f;
                    Tile tile = tileGenerator.tiles[x, y];
                    var rot = rotation;
                    GameObject tilePrefab = tileGenerator.grassTilePrefab;

                    if (tile.tileType == TileType.RoadTile)
                    {
                        tilePrefab = tileGenerator.roadTilePrefab;
                    }
                    else if (tile.tileType == TileType.VillageTile)
                    {
                        tilePrefab = tileGenerator.roadTilePrefab;
                        GameObject villageMesh = Object.Instantiate(tileGenerator.villageOrnament, tilePos, Quaternion.identity, tileParent);
                        villageMesh.transform.localScale = Vector3.one * cellSize * 0.15f;
                    }

                    GameObject tileMesh = Object.Instantiate(tilePrefab, tilePos, rot, tileParent);
                    tileMesh.transform.localScale = Vector3.one * cellSize;
                    gridMesh[x, y] = tileMesh;
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
            y = Mathf.FloorToInt((worldPosition - originPos).y / cellSize);
        }

        public void SetTile(int x, int y, Tile tile)
        {
            if (x < 0 || y < 0) return;
            if (x >= width || y >= height) return;

            gridValues[x, y] = tile;

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
            if (x < 0 || y < 0) return gridValues[0, 0];
            if (x >= width || y >= height) return gridValues[gridValues.GetLength(0), gridValues.GetLength(1)];
            return gridValues[x, y];
        }

        public Tile GetTile(Vector3 worldPosition)
        {
            GetXY(worldPosition, out int x, out int y);
            return GetTile(x, y);
        }
    }
}