using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kira
{
    public class TileSelector : MonoBehaviour
    {
        #region VARIABLES

        [SerializeField]
        private GameObject selectionMeshPrefab;

        [SerializeField]
        private Transform selectionParent;

        [SerializeField]
        private KeyCode addToSelectionKey = KeyCode.LeftShift;

        [SerializeField]
        private KeyCode removeFromSelectionKey = KeyCode.LeftControl;

        private TilePointer tilePointer;
        private Dictionary<Tile, GameObject> selectionMeshPool;
        private List<Tile> tilesSelected;
        private bool hasSelection;

        public Action<Tile> OnTileSelected;
        public Action<Tile> OnTileDeselected;

        public static int SelectionCount => selectionCount;
        public static Tile TileSelected => tileSelected;
        private static Tile tileSelected;
        private static int selectionCount;

        #endregion

        private void Start()
        {
            tilePointer = GetComponent<TilePointer>();
            selectionMeshPool = new Dictionary<Tile, GameObject>();
            tilesSelected = new List<Tile>();

            tilePointer.OnTileClicked += OnTileClicked;
            tilePointer.OnDeselectAll += DeselectAll;
        }

        public void SelectTile(Tile tile)
        {
            tilesSelected.Add(tile);
            selectionCount++;

            GameObject selectionMesh = CreateSelectionMesh();
            selectionMeshPool.Add(tile, selectionMesh);
            MoveSelectionMesh(tile, selectionMesh);
            selectionMesh.SetActive(true);

            hasSelection = true;
            tileSelected = tile;
            OnTileSelected?.Invoke(tile);
        }

        public void DeselectTile(Tile tile)
        {
            tilesSelected.Remove(tile);
            selectionCount--;

            selectionMeshPool.Remove(tile, out GameObject mesh);
            Destroy(mesh);

            if (selectionCount > 0)
            {
                tileSelected = tilesSelected[^1];
            }

            OnTileDeselected?.Invoke(tile);
        }

        public void DeselectAll()
        {
            selectionCount = 0;
            hasSelection = false;

            foreach (Tile tile in tilesSelected)
            {
                GameObject selector = selectionMeshPool[tile];
                Destroy(selector);
            }

            tilesSelected.Clear();
            selectionMeshPool.Clear();
        }

        public void SelectTiles(Tile[] tiles)
        {
            if (tiles.Length == 0) return;

            bool addToSelection = Input.GetKey(addToSelectionKey);
            bool removeFromSelection = Input.GetKey(removeFromSelectionKey);

            if (hasSelection && !addToSelection && !removeFromSelection)
            {
                DeselectAll();
            }

            foreach (Tile tile in tiles)
            {
                if (tilesSelected.Contains(tile))
                {
                    if (removeFromSelection)
                    {
                        DeselectTile(tile);
                    }
                }
                else if (!removeFromSelection)
                {
                    SelectTile(tile);
                }
            }
        }

        #region EVENTS

        public void OnTileClicked(Tile tile)
        {
            bool addToSelection = Input.GetKey(addToSelectionKey);
            bool removeFromSelection = Input.GetKey(removeFromSelectionKey);

            if (tilesSelected.Contains(tile) && (removeFromSelection || addToSelection))
            {
                DeselectTile(tile);
                return;
            }


            if (hasSelection && !addToSelection)
            {
                DeselectAll();
            }

            SelectTile(tile);
        }

        #endregion

        #region HELPERS

        private GameObject CreateSelectionMesh()
        {
            GameObject mesh = Instantiate(selectionMeshPrefab, selectionParent);
            return mesh;
        }

        private void MoveSelectionMesh(Tile tile, GameObject selectionMesh)
        {
            var pos = tile.worldPosition;
            pos.y = 0f;
            selectionMesh.transform.position = pos;
        }

        #endregion
    }
}