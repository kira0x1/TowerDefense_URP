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
        private bool isDragging;
        [SerializeField]
        private Vector3 dragStartPos;

        private Camera cam;
        private Vector3 dragMovement;

        public bool IsDragging => isDragging;

        public Vector3 DragMovement => dragMovement;

        public void Init(Camera cam)
        {
            this.cam = cam;
        }

        public void Update()
        {
            if (Input.GetKeyDown(key))
            {
                isDragging = true;
                dragStartPos = GetMousePoint();
                Cursor.lockState = CursorLockMode.Confined;
            }

            if (Input.GetKeyUp(key))
            {
                Cursor.lockState = CursorLockMode.None;
                isDragging = false;
                dragMovement = Vector3.zero;
            }

            if (isDragging)
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

            if (!smoothDrag) mouseDelta = Vector3.Normalize(mouseDelta);
            mouseDelta *= Time.deltaTime;

            dragMovement = mouseDelta;
        }


        private Vector3 GetMousePoint()
        {
            Vector3 mousePoint = cam.ScreenToViewportPoint(Input.mousePosition);
            (mousePoint.z, mousePoint.y) = (mousePoint.y, mousePoint.z);
            return mousePoint;
        }
    }
}