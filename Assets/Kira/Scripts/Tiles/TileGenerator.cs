using Kira.GridGen;
using Kira.Noise;
using UnityEngine;
using Grid = Kira.GridGen.Grid;

namespace Kira
{
    public class TileGenerator : MonoBehaviour
    {
        #region VARIABLES

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

        [SerializeField]
        private NoiseSettings noiseSettings;

        public Grid grid;
        private GameObject[,] tileMeshes;

        private static readonly int BaseColorProp = Shader.PropertyToID("_BaseColor");

        #endregion

        private void OnValidate()
        {
            if (villageOffset < 0) villageOffset = 0;
            if (gridWidth < 0) gridWidth = 0;
            if (gridHeight < 0) gridHeight = 0;

            if (gridWidth > 100) gridWidth = 100;
        }

        private void ClearGrid()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject child = transform.GetChild(i).gameObject;
                Destroy(child);
            }
        }

        private void Start()
        {
            ClearGrid();
            CreateGrid();
            SpawnGridTiles();
        }

        private void SpawnGridTiles()
        {
            Transform tileParent = new GameObject("Grid Tiles").transform;
            Transform roadParent = new GameObject("Road Tiles").transform;
            Transform basicTileParent = new GameObject("Basic Tiles").transform;

            tileParent.SetParent(transform);
            roadParent.SetParent(tileParent);
            basicTileParent.SetParent(tileParent);

            Tile[,] tiles = grid.tiles;
            tileMeshes = new GameObject[tiles.GetLength(0), tiles.GetLength(1)];

            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                for (int x = 0; x < tiles.GetLength(0); x++)
                {
                    Vector3 pos = grid.GetWorldPosition(x, y) + new Vector3(cellSize, 0, cellSize) * 0.5f;

                    Vector3 tilePos = pos;
                    tilePos.y -= 0.5f;
                    Tile tile = tiles[x, y];
                    GameObject tilePrefab = grassTilePrefab;
                    Transform parent = basicTileParent;

                    if (tile.tileType == TileType.RoadTile)
                    {
                        tilePrefab = roadTilePrefab;
                        parent = roadParent;
                    }
                    else if (tile.tileType == TileType.VillageTile)
                    {
                        parent = tileParent;
                        tilePrefab = roadTilePrefab;
                        GameObject villageMesh = Instantiate(villageOrnament, tilePos, Quaternion.identity, tileParent);
                        villageMesh.transform.localScale = Vector3.one * cellSize * 0.15f;
                    }

                    GameObject tileMesh = Instantiate(tilePrefab, tilePos, Quaternion.Euler(90, 0, 0), parent);
                    tileMesh.name = tile.tileName;
                    tileMesh.transform.localScale = Vector3.one * cellSize;
                    tileMeshes[x, y] = tileMesh;
                }
            }
        }

        [ContextMenu("Display Noise")]
        private void DisplayNoise()
        {
            noiseSettings.width = gridWidth;
            noiseSettings.height = gridHeight;
            float[,] noiseMap = NoiseGenerator.GenerateNoiseMap(noiseSettings);
            SpawnGridMeshEditor();

            for (int y = 0; y < tileMeshes.GetLength(1); y++)
            {
                for (int x = 0; x < tileMeshes.GetLength(0); x++)
                {
                    GameObject tile = tileMeshes[x, y];
                    Renderer tileRend = tile.GetComponent<Renderer>();
                    MaterialPropertyBlock block = new MaterialPropertyBlock();
                    Color noiseColor = Color.Lerp(Color.black, Color.white, noiseMap[x, y]);
                    block.SetColor(BaseColorProp, noiseColor);
                    tileRend.SetPropertyBlock(block);
                }
            }
        }

        public Grid CreateGrid()
        {
            grid = new Grid(gridWidth, gridHeight, cellSize, gridGizmos.gridTextFontSize, GetStartPosition(), centerY, villageOffset);
            return grid;
        }

        private Vector3 GetStartPosition()
        {
            Vector3 startPosition = transform.position;
            startPosition += new Vector3(-(gridWidth * cellSize) / 2f, 0, -(gridHeight * cellSize) / 2f);
            return startPosition;
        }

        #region Editor

        private void OnDrawGizmos()
        {
            if (!showGizmos) return;
            gridGizmos.OnDrawGizmos(this);
        }

        #if UNITY_EDITOR
        [ContextMenu("Spawn Grid")]
        private void SpawnGridMeshEditor()
        {
            // Create the actual grid information
            CreateGrid();

            // Clear any existing grid meshes
            ClearGridMeshEditor();

            // Spawn Grid Meshes
            SpawnGridTiles();
        }

        [ContextMenu("Clear Grid")]
        private void ClearGridMeshEditor()
        {
            if (transform.childCount == 0) return;
            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject child = transform.GetChild(i).gameObject;
                DestroyImmediate(child);
            }
        }

        #endif

        #endregion
    }
}
