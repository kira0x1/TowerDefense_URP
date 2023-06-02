using UnityEngine;

namespace Kira
{
    [System.Serializable]
    public class MouseDrag
    {
        [SerializeField]
        private KeyCode key;
        [SerializeField]
        private float minDragDistance = 0.04f;
        [SerializeField]
        private bool smoothDrag;
        [SerializeField]
        private bool isHoldingKey;
        [SerializeField]
        private Vector3 dragStartPos;

        private Camera cam;
        private Vector3 dragMovement;
        private Vector3 dragStartWorld;
        private bool isDragging;

        public bool IsHoldingKey => isHoldingKey;
        public bool IsDragging => isDragging;

        public Vector3 DragMovement => dragMovement;
        public Vector3 DragStart => dragStartPos;

        public Vector3 DragStartWorld => dragStartWorld;

        public void Init(Camera cam)
        {
            this.cam = cam;
        }

        public void Update()
        {
            if (Input.GetKeyDown(key))
            {
                isHoldingKey = true;
                dragStartPos = GetMousePoint();
                dragStartWorld = GetMousePointWorld();
                Cursor.lockState = CursorLockMode.Confined;
            }

            if (Input.GetKeyUp(key))
            {
                Cursor.lockState = CursorLockMode.None;
                isHoldingKey = false;
                isDragging = false;
                dragMovement = Vector3.zero;
            }

            if (isHoldingKey)
            {
                HandleDragging();
            }
        }

        private void HandleDragging()
        {
            Vector3 mouseDelta = GetMousePoint() - dragStartPos;
            if (mouseDelta.magnitude < minDragDistance)
            {
                return;
            }

            isDragging = true;
            if (!smoothDrag) mouseDelta = Vector3.Normalize(mouseDelta);
            dragMovement = mouseDelta;
        }

        public Vector3 GetMousePoint()
        {
            Vector3 mousePoint = cam.ScreenToViewportPoint(Input.mousePosition);
            return mousePoint;
        }

        public Vector3 GetMousePointWorld()
        {
            return cam.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}