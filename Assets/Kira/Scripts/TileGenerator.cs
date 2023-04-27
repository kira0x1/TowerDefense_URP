using System.Collections;
using Kira.Noise;
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
        private Mesh gizmoMesh;
        [SerializeField]
        private int gridScale = 10;
        [SerializeField]
        private PreviewType previewType;
        [SerializeField]
        private NoiseSettings noiseSettings;

        private Tile[,] tiles;
        private float[,] noiseMap;

        private IEnumerator GenerateTiles()
        {
            noiseSettings.width = gridScale;
            noiseSettings.height = gridScale;

            noiseMap = NoiseGenerator.GenerateNoiseMap(noiseSettings);

            tiles = new Tile[gridScale, gridScale];
            Vector3 startPosition = transform.position;
            startPosition.x -= gridScale / 2f - 0.5f;
            startPosition.z -= gridScale / 2f - 0.5f;

            for (int y = 0; y < gridScale; y++)
            {
                for (int x = 0; x < gridScale; x++)
                {
                    Vector3 pos = startPosition;
                    pos.x += x;
                    pos.z += y;

                    Tile tile = new Tile(x, y);

                    if (x == gridScale / 2 && y == gridScale / 2)
                    {
                        tile.tileType = TileType.VillageTile;
                    }
                    else if (x == gridScale / 2 || y == gridScale / 2)
                    {
                        tile.tileType = TileType.RoadTile;
                    }

                    else
                    {
                        float branchRng = Random.Range(0f, 1f);
                        if (branchRng >= 0.35f)
                        {
                            if (x > 0 && tiles[x - 1, y].tileType == TileType.RoadTile)
                            {
                                tile.tileType = TileType.RoadTile;
                            }

                            if (y > 0 && tiles[x, y - 1].tileType == TileType.RoadTile)
                            {
                                tile.tileType = TileType.RoadTile;
                            }
                        }
                    }

                    tile.worldPosition = pos;

                    tiles[x, y] = tile;
                    Color tileColor = Color.blue;

                    if (tile.tileType == TileType.BuildingTile)
                        tileColor = Color.blue;
                    else if (tile.tileType == TileType.RoadTile)
                        tileColor = Color.red;
                    else if (tile.tileType == TileType.VillageTile)
                        tileColor = Color.green;

                    // Gizmos.color *= 0.8f;

                    GameObject go = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Quad), tile.worldPosition, Quaternion.Euler(90f, 0, 0), transform);
                    go.GetComponent<MeshRenderer>().material.color = tileColor;
                    yield return new WaitForSeconds(0.001f);
                }
            }
        }

        private void Start()
        {
            StartCoroutine(GenerateTiles());
        }

        // private void OnDrawGizmos()
        // {
        //     if (gizmoMesh == null)
        //         return;
        //
        //     StartCoroutine(GenerateTiles());
        //
        //     if (previewType == PreviewType.Grid)
        //     {
        //         foreach (Tile tile in tiles)
        //         {
        //             if (tile.tileType == TileType.BuildingTile)
        //                 Gizmos.color = Color.blue;
        //             else if (tile.tileType == TileType.RoadTile)
        //                 Gizmos.color = Color.red;
        //             else if (tile.tileType == TileType.VillageTile)
        //                 Gizmos.color = Color.green;
        //
        //             Gizmos.color *= 0.8f;
        //
        //             Gizmos.DrawMesh(gizmoMesh, tile.worldPosition, Quaternion.Euler(90f, 0, 0));
        //         }
        //     }
        //     else if (previewType == PreviewType.Noise)
        //     {
        //         for (int y = 0; y < gridScale; y++)
        //         {
        //             for (int x = 0; x < gridScale; x++)
        //             {
        //                 float n = noiseMap[x, y];
        //                 Gizmos.color = Color.Lerp(Color.black, Color.white, n);
        //                 Gizmos.DrawMesh(gizmoMesh, new Vector3(x, 0, y), Quaternion.Euler(90f, 0, 0));
        //             }
        //         }
        //     }
        // }
    }
}