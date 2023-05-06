using System;
using UnityEngine;

namespace Kira
{
    public class TileSelector : MonoBehaviour
    {
        [SerializeField]
        private Transform highlighter;

        public Action<Tile> OnTilePointerEnter;
        public Action OnTilePointerExit;
        public Action<Tile> OnTileSelected;
        public Action OnTileDeselected;

        private TileGenerator tileGenerator;
        private bool selectorVisible;
        private Camera cam;

        private void Start()
        {
            cam = FindObjectOfType<Camera>();
            tileGenerator = GetComponent<TileGenerator>();
            highlighter.localScale = Vector3.one * tileGenerator.cellSize;
            HideSelector();
        }

        private void HideSelector()
        {
            highlighter.gameObject.SetActive(false);
            selectorVisible = false;
        }

        private void ShowSelector()
        {
            highlighter.gameObject.SetActive(true);
            selectorVisible = true;
        }

        private void Update()
        {
            Vector3 pointerPos = cam.ScreenToWorldPoint(Input.mousePosition);
            Tile tile = tileGenerator.grid.GetTile(pointerPos, out bool hasTile);

            if (hasTile)
            {
                OnSelectTile(tile);
                OnTilePointerEnter?.Invoke(tile);
            }
            else if (selectorVisible)
            {
                OnTilePointerExit?.Invoke();
                HideSelector();
            }
        }

        private void OnSelectTile(Tile tile)
        {
            ShowSelector();
            var pos = tile.worldPosition;
            pos.y += 0.1f;
            highlighter.position = pos;
        }
    }
}