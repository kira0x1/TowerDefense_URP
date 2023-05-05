using Kira.GridGen;
using UnityEngine;
using Grid = Kira.GridGen.Grid;

namespace Kira
{
    public class TileGenerator : MonoBehaviour
    {
        [Header("Tiles")]
        public GameObject grassTilePrefab;
        public GameObject roadTilePrefab;
        public GameObject villageOrnament;


        [Header("Grid")]
        public int gridWidth = 10;
        public int gridHeight = 10;
        public float cellSize = 1;

        [SerializeField] private int villageOffset = 2;
        [SerializeField] private bool centerY = false;

        [Header("Gizmos"), SerializeField]
        private bool showGizmos = false;

        [SerializeField]
        private GridGizmos gridGizmos;

        private Grid grid;
        public Tile[,] tiles;

        private void OnValidate()
        {
            if (villageOffset < 0) villageOffset = 0;
            if (gridWidth < 0) gridWidth = 0;
            if (gridHeight < 0) gridHeight = 0;

            if (gridWidth > 100) gridWidth = 100;
            if (gridHeight > 100) gridHeight = 100;
        }

        private void Start()
        {
            GenerateTiles();
            grid = CreateGrid();
            grid.SpawnGrid(this, transform, Quaternion.Euler(90, 0, 0));
        }

        public Grid CreateGrid()
        {
            return new Grid(gridWidth, gridHeight, cellSize, gridGizmos.gridTextFontSize, GetStartPosition());
        }

        private Vector3 GetStartPosition()
        {
            Vector3 startPosition = transform.position;
            startPosition += new Vector3(-(gridWidth * cellSize) / 2f, 0, -(gridHeight * cellSize) / 2f);
            return startPosition;
        }

        private int GetVillageY()
        {
            int villageIndex = centerY ? gridHeight / 2 : 0;
            villageIndex += villageOffset;
            return villageIndex;
        }

        private void GenerateTiles()
        {
            if (gridWidth < 0) gridWidth = 0;
            if (gridHeight < 0) gridHeight = 0;

            tiles = new Tile[gridWidth, gridHeight];

            Vector3 startPosition = GetStartPosition();
            startPosition.x += cellSize / 2f;
            startPosition.z += cellSize / 2f;

            int villageIndex = GetVillageY();

            for (int y = 0; y < gridHeight; y++)
            {
                for (int x = 0; x < gridWidth; x++)
                {
                    bool isRoadTile = x == gridWidth / 2 && y >= villageIndex + 1;
                    bool isVillageTile = x == gridWidth / 2 && y == villageIndex;

                    Vector3 pos = startPosition;
                    pos.x += x * cellSize;
                    pos.z += y * cellSize;

                    Tile tile = new Tile(x, y);

                    if (isRoadTile)
                    {
                        tile.tileType = TileType.RoadTile;
                    }
                    else if (isVillageTile)
                    {
                        tile.tileType = TileType.VillageTile;
                    }
                    else if (x == gridWidth / 2 && y >= villageIndex + 1)
                    {
                        tile.tileType = TileType.RoadTile;
                    }

                    tile.worldPosition = pos;
                    tiles[x, y] = tile;
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (!showGizmos) return;
            GenerateTiles();
            gridGizmos.OnDrawGizmos(this);
        }
    }
}