using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Kira.GridGen
{
    public enum PreviewType
    {
        Grid,
        Noise
    }

    [System.Serializable]
    public class GridGizmos
    {
        private Grid grid;

        [SerializeField]
        private PreviewType previewType;
        [SerializeField]
        private Mesh gizmoMesh;

        [Header("Grid Colors")]
        [SerializeField, Range(0f, 1f)]
        private float gridAlphaMultiplier = 0.7f;
        [SerializeField]
        private Color gridColor = Color.white;

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

        [Header("Grid Text"), Range(0, 64)]
        public int gridTextFontSize = 24;
        public Color gridTextColor = Color.white;

        public void SpawnGridText(Tile[,] tiles, Transform parent = null)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                for (int x = 0; x < tiles.GetLength(0); x++)
                {
                    Tile tile = tiles[x, y];
                    Vector3 pos = tile.worldPosition;
                    pos.y += 1.5f;
                    Quaternion rot = Quaternion.Euler(90, 0, 0);
                    TextMesh text = GridUtils.CreateWorldText("1", parent, pos, gridTextFontSize, gridTextColor);
                    text.transform.rotation = rot;
                }
            }
        }

        public void OnDrawGizmos(TileGenerator tileGenerator)
        {
            if (gizmoMesh == null)
                return;


            if (previewType == PreviewType.Grid)
            {
                grid = tileGenerator.CreateGrid();
                Tile[,] tiles = grid.tiles;
                IEnumerable<Tile> tilesArray = tiles.Cast<Tile>();
                tilesArray = tilesArray as Tile[] ?? tilesArray.ToArray();
                DrawTileGizmo(tilesArray, tileGenerator.cellSize);
                DrawGridLines(tiles);
            }
        }

        private void DrawTileGizmo(IEnumerable<Tile> tiles, float cellSize)
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
                Gizmos.DrawMesh(gizmoMesh, tile.worldPosition, Quaternion.Euler(90f, 0, 0), Vector3.one * cellSize);
            }
        }

        private void DrawGridLines(Tile[,] tiles)
        {
            Gizmos.color = gridColor * gridAlphaMultiplier;
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                for (int x = 0; x < tiles.GetLength(0); x++)
                {
                    Gizmos.DrawLine(grid.GetWorldPosition(x, y), grid.GetWorldPosition(x, y + 1));
                    Gizmos.DrawLine(grid.GetWorldPosition(x, y), grid.GetWorldPosition(x + 1, y));
                }
            }

            Gizmos.DrawLine(grid.GetWorldPosition(0, grid.height), grid.GetWorldPosition(grid.width, grid.height));
            Gizmos.DrawLine(grid.GetWorldPosition(grid.width, 0), grid.GetWorldPosition(grid.width, grid.height));
        }
    }
}