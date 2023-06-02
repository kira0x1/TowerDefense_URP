using UnityEngine;

namespace Kira
{
    public class PlayerCamera : MonoBehaviour
    {
        #region Serialized Variables

        [Header("Move")]
        [SerializeField]
        private float moveSpeed = 150f;
        [SerializeField]
        private float boostMultiplier = 2f;

        [Header("Zoom")]
        [SerializeField]
        private bool smoothZoom;
        [SerializeField]
        private float zoomDuration = 0.1f;
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
        private MouseDrag panDrag;


        [Header("Border Pan")]
        [SerializeField] private bool borderPanEnabled = false;
        [SerializeField] private float borderPanPadding = 0.1f;
        [SerializeField] private float borderPanSpeed = 80f;

        [Header("Reset Position")]
        [SerializeField] private KeyCode centerKey = KeyCode.C;
        [SerializeField] private KeyCode centerKeyModifier = KeyCode.LeftShift;

        #endregion

        private float lastPanMagnitude;

        // Zoom
        private float curZoom;
        private float targetZoom;
        private float scrollAxis;

        private Transform camTr;
        private Camera cam;

        private bool isHoldingShift;
        private bool hasFocus;
        private bool isMovingWithWASD;

        private Vector3 cameraStartPos;

        private void OnApplicationFocus(bool hasFocus)
        {
            this.hasFocus = hasFocus;
        }

        private void Start()
        {
            camTr = transform;
            cam = GetComponent<Camera>();
            panDrag.Init(cam);
            curZoom = cam.orthographicSize;
            cameraStartPos = camTr.position;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.lockState = CursorLockMode.Confined;
        }

        private void Update()
        {
            if (hasFocus)
            {
                Cursor.lockState = CursorLockMode.Confined;
            }

            if (Input.GetKey(centerKeyModifier) && Input.GetKeyDown(centerKey))
            {
                CenterCamera();
            }

            panDrag.Update();

            if (Input.GetKey(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Mouse1))
            {
                hasFocus = false;
                Cursor.lockState = CursorLockMode.None;
            }

            UpdateKeys();
            HandleCameraMove();
            if (isMovingWithWASD) return;
            PanCamera();

            if (!hasFocus || panDrag.IsHoldingKey) return;
            if (borderPanEnabled) HandleBorderPan();
        }

        private void UpdateKeys()
        {
            isHoldingShift = Input.GetKey(KeyCode.LeftShift);

            float speed = zoomSpeed;

            if (isHoldingShift)
            {
                speed *= zoomSpeedMultiplier;
            }


            scrollAxis = Input.mouseScrollDelta.y * speed;

            float orthoSize = cam.orthographicSize;
            targetZoom = orthoSize - scrollAxis;

            // Calculate the fraction of the total duration that has passed.
            curZoom = smoothZoom ? Mathf.MoveTowards(orthoSize, targetZoom, zoomDuration) : targetZoom;
            curZoom = Mathf.Clamp(curZoom, minZoom, maxZoom);
        }

        private void ZoomCamera()
        {
            cam.orthographicSize = curZoom;
        }

        private void LateUpdate()
        {
            ZoomCamera();
        }

        private void CenterCamera()
        {
            camTr.position = cameraStartPos;
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

        private void PanCamera()
        {
            if (!panDrag.IsHoldingKey)
                return;

            float speed = panSpeed * (curZoom / 10f);

            if (isHoldingShift)
            {
                speed *= panSpeedMultiplier;
            }

            MoveCamera(panDrag.DragMovement * speed);
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