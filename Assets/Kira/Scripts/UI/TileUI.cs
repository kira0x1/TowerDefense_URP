using TMPro;
using UnityEngine;

namespace Kira.UI
{
    public class TileUI : PanelUI
    {
        [SerializeField] private TextMeshProUGUI headerText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        private TileSelector tileSelector;

        private void Start()
        {
            tileSelector = FindObjectOfType<TileSelector>();
            tileSelector.OnTilePointerEnter += OnTilePointerEnter;
            tileSelector.OnTilePointerExit += OnTilePointerExit;
        }

        private void OnTilePointerEnter(Tile tile)
        {
            headerText.text = tile.tileName;
            ShowPanel();
        }

        private void OnTilePointerExit()
        {
            HidePanel();
        }
    }
}