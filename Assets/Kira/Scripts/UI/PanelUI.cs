using UnityEngine;

namespace Kira.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class PanelUI : MonoBehaviour
    {
        protected CanvasGroup canvas;

        protected virtual void Awake()
        {
            canvas = GetComponent<CanvasGroup>();
        }

        protected void ShowPanel()
        {
            canvas.alpha = 1f;
            canvas.interactable = true;
            canvas.blocksRaycasts = true;
        }

        protected void HidePanel()
        {
            canvas.alpha = 0f;
            canvas.interactable = false;
            canvas.blocksRaycasts = false;
        }

        #if UNITY_EDITOR
        [ContextMenu("Show Panel")]
        private void ShowPanelEditor()
        {
            canvas = GetComponent<CanvasGroup>();
            ShowPanel();
        }

        [ContextMenu("Hide Panel")]
        private void HidePanelEditor()
        {
            canvas = GetComponent<CanvasGroup>();
            HidePanel();
        }
        #endif
    }
}