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

        private TilePointer tilePointer;
        private Dictionary<Tile, GameObject> selectionMeshPool;
        public List<Tile> tilesSelected;

        private static Tile tileSelected;
        public static Tile TileSelected => tileSelected;

        private static int selectionCount;
        public static int SelectionCount => selectionCount;
        private bool hasSelection;

        public Action<Tile> OnTileSelected;
        public Action<Tile> OnTileDeselected;

        #endregion

        private void Start()
        {
            tilePointer = GetComponent<TilePointer>();
            selectionMeshPool = new Dictionary<Tile, GameObject>();
            tilesSelected = new List<Tile>();

            tilePointer.OnTileClicked += OnTileClicked;
            tilePointer.OnDeselectAll += DeselectAll;
        }

        private void SelectTile(Tile tile)
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

        private void DeselectTile(Tile tile)
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

        private void DeselectAll()
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

        #region EVENTS

        public void OnTileClicked(Tile tile, bool addToSelection)
        {
            if (addToSelection)
            {
                if (tilesSelected.Contains(tile))
                {
                    DeselectTile(tile);
                    return;
                }

                SelectTile(tile);
                return;
            }

            if (hasSelection)
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
