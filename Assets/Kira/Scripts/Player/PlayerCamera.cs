using UnityEngine;

namespace Kira
{
    public class PlayerCamera : MonoBehaviour
    {
        #region Variables

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

        [Header("Border Pan")]
        [SerializeField]
        private float borderPanPadding = 0.1f;
        [SerializeField]
        private float borderPanSpeed = 80f;

        private bool isDragging;
        private Vector3 dragStartPos;
        private float lastPanMagnitude;
        private float curZoom;
        private Transform camTr;
        private Camera cam;

        private bool isHoldingShift;
        private bool hasFocus;

        private bool isMovingWithWASD;

        #endregion

        private void OnApplicationFocus(bool hasFocus)
        {
            this.hasFocus = hasFocus;
        }

        private void Start()
        {
            camTr = transform;
            cam = GetComponent<Camera>();
            curZoom = cam.orthographicSize;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.lockState = CursorLockMode.Confined;
        }

        private void Update()
        {
            if (hasFocus)
            {
                Cursor.lockState = CursorLockMode.Confined;
            }
            else
            {
                isDragging = false;
            }

            if (Input.GetKeyDown(KeyCode.Mouse2))
            {
                isDragging = true;
                dragStartPos = GetMousePoint();
                Cursor.lockState = CursorLockMode.Confined;
                hasFocus = true;
            }

            if (Input.GetKeyUp(KeyCode.Mouse2))
            {
                Cursor.lockState = CursorLockMode.None;
                isDragging = false;
            }

            if (Input.GetKey(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Mouse1))
            {
                hasFocus = false;
                Cursor.lockState = CursorLockMode.None;
            }

            isHoldingShift = Input.GetKey(KeyCode.LeftShift);
            HandleCameraMove();
            ZoomCamera();
            if (isMovingWithWASD) return;
            PanCamera();

            if (!hasFocus || isDragging) return;
            HandleBorderPan();
        }

        private Vector3 GetMousePoint()
        {
            Vector3 mousePoint = cam.ScreenToViewportPoint(Input.mousePosition);
            (mousePoint.z, mousePoint.y) = (mousePoint.y, mousePoint.z);
            return mousePoint;
        }

        private void MoveCamera(Vector3 direction)
        {
            Vector3 position = camTr.position + direction;
            camTr.position = position;
        }

        private void HandleCameraMove()
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            isMovingWithWASD = h != 0 || v != 0;

            float speed = moveSpeed;

            if (isHoldingShift)
            {
                speed *= boostMultiplier;
            }

            Vector3 mov = Vector3.ClampMagnitude(new Vector3(h, 0, v), 1f);
            mov *= speed * Time.deltaTime;

            MoveCamera(mov);
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

            if (!smoothDrag) mouseDelta = Vector3.Normalize(mouseDelta);
            mouseDelta *= speed * Time.deltaTime;

            MoveCamera(mouseDelta);
        }

        private void HandleBorderPan()
        {
            Vector3 mousePoint = GetMousePoint();
            Vector3 panDir = Vector3.zero;

            float maxBorder = 1f - borderPanPadding;

            if (mousePoint.z > maxBorder)
            {
                panDir.z += borderPanSpeed;
            }
            else if (mousePoint.z <= borderPanPadding)
            {
                panDir.z -= borderPanSpeed;
            }

            if (mousePoint.x > maxBorder)
            {
                panDir.x += borderPanSpeed;
            }
            else if (mousePoint.x <= borderPanPadding)
            {
                panDir.x -= borderPanSpeed;
            }

            float speed = borderPanSpeed * (curZoom / 10f);
            panDir = Vector3.Normalize(panDir);
            panDir *= speed * Time.deltaTime;

            MoveCamera(panDir);
        }
    }
}
