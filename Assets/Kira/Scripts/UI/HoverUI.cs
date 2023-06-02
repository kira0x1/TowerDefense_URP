using TMPro;
using UnityEngine;

namespace Kira.UI
{
    public class HoverUI : PanelUI
    {
        [Header("Settings")]
        [SerializeField] private bool showHoverUI;

        [Header("UI")]
        [SerializeField] private TextMeshProUGUI headerText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        private TilePointer tilePointer;

        private void Start()
        {
            tilePointer = FindObjectOfType<TilePointer>();
            tilePointer.OnTilePointerEnter += OnPointerEnter;
            tilePointer.OnTilePointerExit += OnPointerExit;
        }


        private void OnPointerEnter(Tile tile)
        {
            if (!showHoverUI) return;

            headerText.text = tile.tileName;
            descriptionText.text = tile.isOccupied ? $"{tile.health:F0}/{tile.maxHealth:F0}" : "";
            ShowPanel();
        }

        private void OnPointerExit()
        {
            if (!showHoverUI) return;
            HidePanel();
        }
    }
}