using System;
using UnityEngine;

namespace Kira
{
    public class TilePointer : MonoBehaviour
    {
        [SerializeField]
        private Transform highlighter;

        private TileGenerator tileGenerator;
        private Camera cam;
        private bool selectorVisible;

        // EVENTS
        public Action<Tile> OnTilePointerEnter;
        public Action OnTilePointerExit;
        public Action<Tile> OnTileClicked;
        public Action OnDeselectAll;

        private void Start()
        {
            cam = FindObjectOfType<Camera>();
            tileGenerator = GetComponent<TileGenerator>();

            highlighter.localScale = Vector3.one * tileGenerator.cellSize;
            HideHoverSelector();
        }

        #region Helpers

        private void HideHoverSelector()
        {
            highlighter.gameObject.SetActive(false);
            selectorVisible = false;
        }

        private void ShowSelector()
        {
            highlighter.gameObject.SetActive(true);
            selectorVisible = true;
        }

        #endregion

        private void Update()
        {
            Vector3 pointerPos = cam.ScreenToWorldPoint(Input.mousePosition);
            Tile tile = tileGenerator.grid.GetTile(pointerPos, out bool hasTile);

            if (Input.GetKeyUp(KeyCode.Mouse0) && !MarqueSelector.IsSelecting && Time.time > MarqueSelector.SelectionStoppedTime)
            {
                if (hasTile)
                {
                    ClickTile(tile);
                }
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                DeselectAll();
            }

            if (hasTile && !MarqueSelector.IsSelecting)
            {
                HoverTile(tile);
                OnTilePointerEnter?.Invoke(tile);
            }
            else if (selectorVisible)
            {
                OnTilePointerExit?.Invoke();
                HideHoverSelector();
            }
        }

        private void HoverTile(Tile tile)
        {
            ShowSelector();
            var pos = tile.worldPosition;
            pos.y += 0.1f;
            highlighter.position = pos;
        }

        private void ClickTile(Tile tile)
        {
            OnTileClicked?.Invoke(tile);
        }

        private void DeselectAll()
        {
            OnDeselectAll?.Invoke();
        }
    }
}