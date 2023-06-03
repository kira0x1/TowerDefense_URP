using Kira.Building;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Kira.UI
{
    public class BuildingSlotUI : SlotUI
    {
        private BuildingData building;

        public override void OnPointerEnter(PointerEventData eventData)
        {
            ToolTipUI.Instance.ShowTooltip(building.Name, building.Tooltip);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            ToolTipUI.Instance.HideTooltip();
        }

        public void SetBuilding(BuildingData building)
        {
            this.building = building;
            SetIcon(building.Icon);
        }

        public override void OnClicked()
        {
            Debug.Log($"{building.name} clicked!");
        }
    }
}