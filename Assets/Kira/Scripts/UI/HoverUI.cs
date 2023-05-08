using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kira.UI
{
    public class HoverUI : PanelUI
    {
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
            headerText.text = tile.tileName;
            descriptionText.text = tile.isOccupied ? $"{tile.health:F0}/{tile.maxHealth:F0}" : "";
            ShowPanel();
        }

        private void OnPointerExit()
        {
            HidePanel();
        }
    }
}
