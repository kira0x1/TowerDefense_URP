using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kira.UI
{
    public class SelecionUI : PanelUI
    {
        [SerializeField] private TextMeshProUGUI headerText;
        [SerializeField] private TextMeshProUGUI descriptionText;

        private GameObject healthParent;
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private Image healthFill;

        private TilePointer tilePointer;
        private TileSelector tileSelector;

        private void Start()
        {
            tilePointer = FindObjectOfType<TilePointer>();
            tileSelector = FindObjectOfType<TileSelector>();
            healthParent = healthFill.transform.parent.gameObject;
            tilePointer.OnTileClicked += OnTileSelected;
            tilePointer.OnTileDeselected += OnTileDeselected;
            tilePointer.OnDeselectAll += OnDesectAll;
        }

        protected override void HidePanel()
        {
            if (Application.isEditor)
            {
                healthParent = healthFill.transform.parent.gameObject;
            }

            base.HidePanel();
            healthParent.SetActive(false);
            healthText.text = "";
            descriptionText.text = "";
        }

        private void OnTileSelected(Tile tile, bool addToSelection)
        {
            healthParent.SetActive(tile.isOccupied);
            headerText.text = TileSelector.SelectionCount == 1 ? tile.tileName : $"{TileSelector.SelectionCount} tiles";
            healthText.text = tile.isOccupied ? $"{tile.health:F0}/{tile.maxHealth:F0}" : "";
            healthFill.fillAmount = tile.health / tile.maxHealth;
            ShowPanel();
        }

        private void OnTileDeselected(Tile tile, bool removeFromSelection)
        {
            HidePanel();
        }

        private void OnDesectAll()
        {
            HidePanel();
        }
    }
}
