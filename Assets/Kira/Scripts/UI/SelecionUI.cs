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

            tilePointer.OnDeselectAll += OnDesectAll;
            tileSelector.OnTileSelected += HandleTileSelection;
            tileSelector.OnTileDeselected += HandleTileSelection;
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

        private void HandleTileSelection(Tile tile)
        {
            Tile tileSelected = TileSelector.TileSelected;

            if (TileSelector.SelectionCount <= 0)
            {
                HidePanel();
                return;
            }

            if (TileSelector.SelectionCount > 1)
            {
                headerText.text = $"{TileSelector.SelectionCount} tiles";
                healthParent.SetActive(false);
            }
            else
            {
                healthParent.SetActive(tileSelected.isOccupied);
                headerText.text = tileSelected.tileName;
                healthText.text = tileSelected.isOccupied ? $"{tileSelected.health:F0}/{tileSelected.maxHealth:F0}" : "";
                healthFill.fillAmount = tileSelected.health / tileSelected.maxHealth;
            }

            ShowPanel();
        }

        private void OnDesectAll()
        {
            HidePanel();
        }
    }
}
