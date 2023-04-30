using System.Collections.Generic;
using UnityEngine;

namespace Kira
{
    public enum PreviewType
    {
        Grid,
        Noise
    }

    public class TileGenerator : MonoBehaviour
    {
        [SerializeField]
        private bool centerY = false;

        [SerializeField]
        private int villageOffset = 2;

        [SerializeField]
        private Mesh gizmoMesh;
        [SerializeField]
        private int gridWidth = 10;
        [SerializeField]
        private int gridLength = 10;
        [SerializeField]
        private PreviewType previewType;

        [Header("Colors")]
        [SerializeField, Range(0f, 1f)]
        private float alphaMultiplier = 0.7f;
        [SerializeField]
        private Color buildingColor = Color.blue;
        [SerializeField]
        private Color roadColor = Color.red;
        [SerializeField]
        private Color villageColor = Color.green;
        [SerializeField]
        private Color branchColor = Color.yellow;

        private float[,] noiseMap;
        private List<Tile> tiles;
        private List<Tile> roadTiles;

        [SerializeField]
        private List<RoadChunk> roadChunks;

        private void OnValidate()
        {
            if (villageOffset < 0) villageOffset = 0;
            if (gridWidth < 0) gridWidth = 0;
            if (gridLength < 0) gridLength = 0;

            if (gridWidth > 100) gridWidth = 100;
            if (gridLength > 100) gridLength = 100;
        }

        private Vector3 GetStartPosition()
        {
            Vector3 startPosition = transform.position;

            int villageIndex = centerY ? gridLength / 2 : 0;
            villageIndex += villageOffset;

            startPosition.x -= gridWidth / 2f - 0.5f;
            startPosition.z -= villageIndex - 0.5f;
            return startPosition;
        }

        private int GetVillageY()
        {
            int villageIndex = centerY ? gridLength / 2 : 0;
            villageIndex += villageOffset;
            return villageIndex;
        }

        private void GenerateRoad(int startX, int startY, int maxX, int maxY)
        {
            Vector3 startPosition = GetStartPosition();
            RoadChunk roadChunk = new RoadChunk(startX, startY, startPosition, maxX, maxY);
            roadChunks.Add(roadChunk);
        }

        private void GenerateTiles()
        {
            if (gridWidth < 0) gridWidth = 0;
            if (gridLength < 0) gridLength = 0;

            tiles = new List<Tile>();
            roadTiles = new List<Tile>();

            Vector3 startPosition = GetStartPosition();
            int villageIndex = GetVillageY();

            // Create Village
            int villageX = gridWidth / 2;

            Vector3 villagePos = startPosition;
            villagePos.x += villageX;
            villagePos.z += villageIndex;

            Tile villageTile = new Tile(villageX, villageIndex);
            villageTile.tileType = TileType.VillageTile;
            villageTile.worldPosition = villagePos;
            tiles.Add(villageTile);

            for (int y = 0; y < gridLength; y++)
            {
                for (int x = 0; x < gridWidth; x++)
                {
                    bool isRoadTile = x == gridWidth / 2 && y >= villageIndex + 1;
                    bool isVillageTile = x == gridWidth / 2 && y == villageIndex;

                    if (isRoadTile || isVillageTile)
                    {
                        continue;
                    }

                    Vector3 pos = startPosition;
                    pos.x += x;
                    pos.z += y;

                    Tile tile = new Tile(x, y);

                    // if (x == gridWidth / 2 && y == villageIndex)
                    // {
                    //     tile.tileType = TileType.VillageTile;
                    // }
                    // else if (x == gridWidth / 2 && y >= villageIndex + 1)
                    // {
                    //     tile.tileType = TileType.RoadTile;
                    // }

                    tile.worldPosition = pos;
                    // tiles[x, y] = tile;
                    // tiles.Add(tile);
                }
            }

            // CreateBranch(gridWidth / 2, villageIndex + 1);

            if (roadChunks.Count < 2)
            {
                roadChunks = new List<RoadChunk>();
                GenerateRoad(0, villageIndex + 2, gridWidth, villageIndex + 3);
                Tile lastRoadTile = roadChunks[0].roadTiles[^1];
                GenerateRoad(lastRoadTile.x + 1, lastRoadTile.y, lastRoadTile.x + 2, gridLength);
            }
        }

        private void CreateBranch(int startX, int startY)
        {
            Vector3 startPosition = GetStartPosition();

            Vector3 branchPos = startPosition;
            branchPos.z += startY;
            branchPos.x += startX;

            Tile branchTile = new Tile(startX, startY);
            branchTile.tileType = TileType.BranchTile;
            branchTile.worldPosition = branchPos;

            tiles.Add(branchTile);

            for (int y = startY + 1; y < gridLength; y++)
            {
                Vector3 pos = startPosition;
                pos.x += startX;
                pos.z += y;

                Tile tile = new Tile(startX, y);
                tile.tileType = TileType.RoadTile;
                tile.worldPosition = pos;
                roadTiles.Add(tile);
            }
        }

        private void Start()
        {
            GenerateTiles();
        }

        private void DrawTileGizmo(IEnumerable<Tile> tiles)
        {
            foreach (Tile tile in tiles)
            {
                if (tile.tileType == TileType.BuildingTile)
                    Gizmos.color = buildingColor;
                else if (tile.tileType == TileType.RoadTile)
                    Gizmos.color = roadColor;
                else if (tile.tileType == TileType.VillageTile)
                    Gizmos.color = villageColor;
                else if (tile.tileType == TileType.BranchTile)
                    Gizmos.color = branchColor;

                Gizmos.color *= alphaMultiplier;

                Gizmos.DrawMesh(gizmoMesh, tile.worldPosition, Quaternion.Euler(90f, 0, 0));
            }
        }

        private void OnDrawGizmos()
        {
            if (gizmoMesh == null)
                return;

            GenerateTiles();

            if (previewType == PreviewType.Grid)
            {
                DrawTileGizmo(tiles);
                DrawTileGizmo(roadTiles);
                foreach (RoadChunk roadChunk in roadChunks)
                {
                    DrawTileGizmo(roadChunk.roadTiles);
                    Gizmos.color = Color.green;
                    Gizmos.DrawWireCube(roadChunk.bounds.center, roadChunk.bounds.size);
                }
            }
            else if (previewType == PreviewType.Noise)
            {
                for (int y = 0; y < gridWidth; y++)
                {
                    for (int x = 0; x < gridWidth; x++)
                    {
                        float n = noiseMap[x, y];
                        Gizmos.color = Color.Lerp(Color.black, Color.white, n);
                        Gizmos.DrawMesh(gizmoMesh, new Vector3(x, 0, y), Quaternion.Euler(90f, 0, 0));
                    }
                }
            }
        }
    }
}