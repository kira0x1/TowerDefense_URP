using UnityEngine;

namespace Kira
{
    public class PlayerCamera : MonoBehaviour
    {
        [Header("Move")]
        [SerializeField]
        private float moveSpeed = 150f;
        [SerializeField]
        private float boostMultiplier = 2f;

        [Header("Zoom")]
        [SerializeField]
        private float zoomSpeed = 55f;
        [SerializeField]
        private float minZoom = 35f;
        [SerializeField]
        private float maxZoom = 200f;
        [SerializeField]
        private float zoomSpeedMultiplier = 4.0f;

        [Header("Pan")]
        [SerializeField]
        private float panSpeed = 150f;
        [SerializeField]
        private float panSpeedMultiplier = 4.0f;
        [SerializeField]
        private float minDragDistance = 0.04f;
        [SerializeField]
        private bool smoothDrag;

        private bool isDragging;
        private Vector3 dragStartPos;
        private float lastPanMagnitude;
        private float curZoom;
        private Transform camTr;
        private Camera cam;

        private bool isHoldingShift;

        private void Start()
        {
            camTr = transform;
            cam = GetComponent<Camera>();
            curZoom = cam.orthographicSize;
        }

        private void Update()
        {
            isHoldingShift = Input.GetKey(KeyCode.LeftShift);
            MoveCamera();
            ZoomCamera();
            PanCamera();
        }

        private void MoveCamera()
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            float speed = moveSpeed;

            if (isHoldingShift)
            {
                speed *= boostMultiplier;
            }

            Vector3 mov = Vector3.ClampMagnitude(new Vector3(h, 0, v), 1f);
            mov *= speed * Time.deltaTime;
            Vector3 position = camTr.position + mov;
            camTr.position = position;
        }

        private void ZoomCamera()
        {
            float speed = zoomSpeed;

            if (isHoldingShift)
            {
                speed *= zoomSpeedMultiplier;
            }

            float scroll = Input.GetAxis("Mouse ScrollWheel") * speed;

            curZoom -= scroll;

            if (curZoom < minZoom) curZoom = minZoom;
            else if (curZoom > maxZoom) curZoom = maxZoom;

            cam.orthographicSize = curZoom;
        }

        private void PanCamera()
        {
            if (Input.GetKeyDown(KeyCode.Mouse2))
            {
                isDragging = true;
                dragStartPos = GetMousePoint();
                Cursor.lockState = CursorLockMode.Confined;
            }

            if (Input.GetKeyUp(KeyCode.Mouse2))
            {
                Cursor.lockState = CursorLockMode.None;
                isDragging = false;
            }

            if (!isDragging)
                return;

            Vector3 mouseDelta = GetMousePoint() - dragStartPos;
            if (mouseDelta.magnitude < minDragDistance)
            {
                return;
            }

            float speed = panSpeed * (curZoom / 10f);
            if (isHoldingShift)
            {
                speed *= panSpeedMultiplier;
            }

            (mouseDelta.y, mouseDelta.z) = (mouseDelta.z, mouseDelta.y);
            if (!smoothDrag) mouseDelta = Vector3.Normalize(mouseDelta);
            mouseDelta *= speed * Time.deltaTime;
            Vector3 position = camTr.position + mouseDelta;
            camTr.position = position;
        }

        private Vector3 GetMousePoint()
        {
            Vector3 mousePoint = cam.ScreenToViewportPoint(Input.mousePosition);
            mousePoint.x -= 0.5f;
            mousePoint.y -= 0.5f;
            return mousePoint;
        }
    }
}