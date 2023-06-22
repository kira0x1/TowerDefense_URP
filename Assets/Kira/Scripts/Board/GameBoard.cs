using System.Collections.Generic;
using UnityEngine;

namespace Kira.Board
{
    public class GameBoard : MonoBehaviour
    {
        [SerializeField]
        private Transform ground = default;

        [SerializeField]
        private GameTile tilePrefab = default;

        private Vector2Int size;
        private GameTile[] tiles;
        private Queue<GameTile> searchFrontier = new Queue<GameTile>();

        public GameTile GetTile(Ray ray)
        {
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                int x = (int)(hit.point.x + size.x * 0.5f);
                int y = (int)(hit.point.z + size.y * 0.5f);

                if (x >= 0 && x < size.x && y >= 0 && y < size.y)
                {
                    return tiles[x + y * size.x];
                }
            }

            return null;
        }

        public void Initialize(Vector2Int size)
        {
            this.size = size;
            ground.localScale = new Vector3(size.x, size.y, 1f);

            Vector2 offset = new Vector2((size.x - 1) * 0.5f, (size.y - 1) * 0.5f);
            tiles = new GameTile[size.x * size.y];

            for (int i = 0, y = 0; y < size.y; y++)
            {
                for (int x = 0; x < size.x; x++, i++)
                {
                    GameTile tile = tiles[i] = Instantiate(tilePrefab);
                    tile.IsAlternative = (x & 1) == 0;

                    if ((y & 1) == 0)
                    {
                        tile.IsAlternative = !tile.IsAlternative;
                    }

                    if (x > 0)
                    {
                        GameTile.MakeEastWestNeighbours(tile, tiles[i - 1]);
                    }

                    if (y > 0)
                    {
                        GameTile.MakeNorthSouthNeighbours(tile, tiles[i - size.x]);
                    }

                    Transform tileTransform = tile.transform;
                    tileTransform.SetParent(transform, false);
                    tileTransform.localPosition = new Vector3(x - offset.x, 0f, y - offset.y);
                }
            }

            FindPaths();
        }

        private void FindPaths()
        {
            foreach (GameTile tile in tiles)
            {
                tile.ClearPath();
            }

            tiles[tiles.Length / 2].BecomeDestination();
            searchFrontier.Enqueue(tiles[tiles.Length / 2]);

            while (searchFrontier.Count > 0)
            {
                GameTile nextTile = searchFrontier.Dequeue();

                if (nextTile != null)
                {
                    if (nextTile.IsAlternative)
                    {
                        searchFrontier.Enqueue(nextTile.GrowPathNorth());
                        searchFrontier.Enqueue(nextTile.GrowPathSouth());
                        searchFrontier.Enqueue(nextTile.GrowPathEast());
                        searchFrontier.Enqueue(nextTile.GrowPathWest());
                    }
                    else
                    {
                        searchFrontier.Enqueue(nextTile.GrowPathWest());
                        searchFrontier.Enqueue(nextTile.GrowPathEast());
                        searchFrontier.Enqueue(nextTile.GrowPathSouth());
                        searchFrontier.Enqueue(nextTile.GrowPathNorth());
                    }
                }
            }

            foreach (GameTile tile in tiles)
            {
                tile.ShowPath();
            }
        }
    }
}