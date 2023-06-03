using System.Collections.Generic;
using Kira.Building;
using TMPro;
using UnityEngine;

namespace Kira.UI
{
    public class BuildMenuUI : PanelUI
    {
        [SerializeField]
        private TextMeshProUGUI headerText;

        [SerializeField]
        private Transform slotsParent;

        [SerializeField]
        private BuildingSlotUI slotPrefab;

        private List<BuildingSlotUI> slots;
        private BuildingSettings buildingSettings;
        private bool hasPopulated;

        private void Start()
        {
            buildingSettings = FindObjectOfType<BuildingSettings>();
        }

        protected override void ShowPanel()
        {
            if (!hasPopulated)
            {
                PopulateSlots();
            }

            base.ShowPanel();
        }

        private void PopulateSlots()
        {
            if (!Application.isPlaying) return;
            slots = new List<BuildingSlotUI>();

            foreach (BuildingData building in buildingSettings.GetBuildings())
            {
                BuildingSlotUI slot = Instantiate(slotPrefab, slotsParent);
                slot.SetBuilding(building);
            }

            hasPopulated = true;
        }
    }
}