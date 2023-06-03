using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Kira.UI
{
    public class SlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        protected Image slotIcon;

        protected bool hoveringSlot;

        public virtual void OnClicked()
        {
        }

        protected void SetIcon(Sprite icon)
        {
            slotIcon.sprite = icon;
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
        }
    }
}