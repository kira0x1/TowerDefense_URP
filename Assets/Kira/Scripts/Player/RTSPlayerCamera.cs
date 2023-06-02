using Cinemachine;
using UnityEngine;

namespace Kira
{
    public class RTSPlayerCamera : MonoBehaviour
    {
        #region Serialized Variables

        [Header("Move")]
        [SerializeField]
        private float moveSpeed = 150f;

        [SerializeField]
        private float boostMultiplier = 2f;

        [SerializeField]
        private Transform cameraTarget;

        [SerializeField]
        private CinemachineVirtualCamera virtualCam;

        [Header("Zoom"), SerializeField]
        private float zoomSpeed = 6f;

        [SerializeField]
        private float zoomSmoothTime = 8f;

        [SerializeField]
        private float minZoom = 35f;

        [SerializeField]
        private float maxZoom = 200f;

        [SerializeField]
        private float zoomSpeedMultiplier = 4.0f;

        [SerializeField]
        private float screenSidesZoneSize = 60f;

        [Header("Pan")]
        [SerializeField]
        private float panSpeed = 150f;

        [SerializeField]
        private float panSpeedMultiplier = 4.0f;

        [SerializeField]
        private MouseDrag panDrag;

        #endregion

        private float curZoom;
        private float zoomVelocity;

        private bool isHoldingShift;
        private bool isDragging;
        private bool isMovingWithWASD;


        private Camera cam;
        private CinemachineFramingTransposer framingTransposer;
        private Transform camTr;

        private void Awake()
        {
            cam = GetComponent<Camera>();
            camTr = virtualCam.transform;
            framingTransposer = virtualCam.GetCinemachineComponent<CinemachineFramingTransposer>();
            panDrag.Init(cam);
            curZoom = virtualCam.m_Lens.OrthographicSize;
        }

        private void Update()
        {
            isHoldingShift = Input.GetKey(KeyCode.LeftShift);
            panDrag.Update();
            ZoomCamera();
            PanCamera();
        }

        private void ZoomCamera()
        {
            float zoomDelta = Input.mouseScrollDelta.y * zoomSpeed;

            if (isHoldingShift)
            {
                zoomDelta *= zoomSpeedMultiplier;
            }

            curZoom -= zoomDelta;
            curZoom = Mathf.Clamp(curZoom, minZoom, maxZoom);

            float targetZoom = Mathf.SmoothDamp(virtualCam.m_Lens.OrthographicSize, curZoom, ref zoomVelocity, zoomSmoothTime / 100);
            virtualCam.m_Lens.OrthographicSize = targetZoom;
        }

        private void GetMouseScreenSide(Vector3 mousePosition, out int width, out int height)
        {
            int heightPos = 0;
            int widthPos = 0;
            if (mousePosition.x >= 0 && mousePosition.x <= screenSidesZoneSize)
            {
                widthPos = -1;
            }
            else if (mousePosition.x >= Screen.width - screenSidesZoneSize && mousePosition.x <= Screen.width)
            {
                widthPos = 1;
            }

            if (mousePosition.y >= 0 && mousePosition.y <= screenSidesZoneSize)
            {
                heightPos = -1;
            }
            else if (mousePosition.y >= Screen.height - screenSidesZoneSize && mousePosition.y <= Screen.height)
            {
                heightPos = 1;
            }

            width = widthPos;
            height = heightPos;
        }


        private void MoveTargetRelativeToCamera(Vector3 direction, float speed)
        {
            float relativeZoomCameraMoveSpeed = speed * curZoom / 10f;
            Vector3 camForward = camTr.forward;
            Vector3 camRight = camTr.right;
            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();
            Vector3 relativeDir = camForward * direction.z + camRight * direction.x;
            cameraTarget.Translate(relativeDir * (relativeZoomCameraMoveSpeed * speed * Time.deltaTime));
        }


        private void MoveCamera(Vector3 direction)
        {
            Vector3 position = cameraTarget.position + direction;
            cameraTarget.position = position;
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
            // mov *= speed * Time.deltaTime;

            // MoveCamera(mov);
            MoveTargetRelativeToCamera(mov, speed);
        }


        private void PanCamera()
        {
            if (!panDrag.IsHoldingKey)
                return;

            float speed = panSpeed * (curZoom / minZoom);

            if (isHoldingShift)
            {
                speed *= panSpeedMultiplier;
            }

            MoveCamera(panDrag.DragMovement * speed);
        }
    }
}