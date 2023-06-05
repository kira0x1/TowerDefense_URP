using UnityEngine;

namespace Kira.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class PanelUI : MonoBehaviour
    {
        protected CanvasGroup canvas;

        protected bool isVisible;
        public bool ISVisible => isVisible;

        protected virtual void Awake()
        {
            canvas = GetComponent<CanvasGroup>();
            CheckVisible();
        }

        protected virtual void ShowPanel(bool blockRaycasts = true)
        {
            isVisible = true;
            canvas.alpha = 1f;
            canvas.interactable = true;
            canvas.blocksRaycasts = blockRaycasts;
        }

        protected virtual void HidePanel()
        {
            isVisible = false;
            canvas.alpha = 0f;
            canvas.interactable = false;
            canvas.blocksRaycasts = false;
        }

        public virtual void TogglePanel()
        {
            if (isVisible)
            {
                HidePanel();
            }
            else
            {
                ShowPanel();
            }
        }

        /// <summary>
        /// Force check if visible by checking if alpha, interactability and blockraycasts are 1f, true, and true respectivly
        /// preferably not use this or to use this once on initialization
        /// </summary>
        private void CheckVisible()
        {
            isVisible = canvas.alpha >= 0.9f && canvas.interactable && canvas.blocksRaycasts;
        }

        #if UNITY_EDITOR
        [ContextMenu("Show Panel")]
        private void ShowPanelEditor()
        {
            canvas = GetComponent<CanvasGroup>();
            ShowPanel();
        }

        [ContextMenu("Hide Panel")]
        protected virtual void HidePanelEditor()
        {
            canvas = GetComponent<CanvasGroup>();
            HidePanel();
        }
        #endif
    }
}