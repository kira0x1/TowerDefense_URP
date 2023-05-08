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

        private static int selectionCount;
        public static int SelectionCount => selectionCount;
        private bool hasSelection;

        #endregion

        private void Start()
        {
            tilePointer = GetComponent<TilePointer>();
            selectionMeshPool = new Dictionary<Tile, GameObject>();
            tilesSelected = new List<Tile>();

            tilePointer.OnTileClicked += OnTileSelected;
            tilePointer.OnTileDeselected += OnTileDeselected;
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
        }

        private void DeselectTile(Tile tile)
        {
            tilesSelected.Remove(tile);
            selectionCount--;

            selectionMeshPool.Remove(tile, out GameObject mesh);
            Destroy(mesh);
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

        private void OnTileSelected(Tile tile, bool addToSelection)
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

        private void OnTileDeselected(Tile tile, bool removeOne)
        {
            if (removeOne) DeselectTile(tile);
            else DeselectAll();
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
