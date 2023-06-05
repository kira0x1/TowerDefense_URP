using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kira.UI
{
    public class ToolTipUI : PanelUI
    {
        [SerializeField]
        private TextMeshProUGUI headerText;

        [SerializeField]
        private TextMeshProUGUI bodyText;

        private Action<string, string> OnShowTooltipEvent;
        private Action HideTooltipEvent;

        private static ToolTipUI instance;
        public static ToolTipUI Instance => instance;
        private Camera cam;
        private bool tooltipOn;

        [SerializeField]
        private float offsetX;

        [SerializeField]
        private float offsetY;

        protected override void Awake()
        {
            base.Awake();
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
        }

        private void Start()
        {
            cam = Camera.main;
        }

        private void Update()
        {
            if (!tooltipOn) return;

            var pos = Input.mousePosition;
            pos.x += offsetX;
            pos.y -= offsetY;

            transform.position = pos;
        }

        public void ShowTooltip(string header, string body)
        {
            headerText.text = header;
            bodyText.text = body;
            ShowPanel(false);
            tooltipOn = true;
        }

        public void HideTooltip()
        {
            HidePanel();
            tooltipOn = false;
        }
    }
}