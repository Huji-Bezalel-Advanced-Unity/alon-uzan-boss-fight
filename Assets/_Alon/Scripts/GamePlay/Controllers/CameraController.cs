using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Alon.Scripts.Gameplay.Controllers
{
    public class CameraController : MonoBehaviour
    {
        /// <summary>
        /// Serialized Fields
        /// </summary>
        [SerializeField] private float zoomSpeed = 10f;

        [SerializeField] private float minZoom = 5f;

        [SerializeField] private float maxZoom = 8f;

        [SerializeField] private Vector2 minBoundary;

        [SerializeField] private Vector2 maxBoundary;

        /// <summary>
        /// Private Fields
        /// </summary>
        private Vector3 _dragOrigin;

        private Camera _mainCamera;


        // End Of Local Variables

        private void Start()
        {
            _mainCamera = gameObject.GetComponent<Camera>();
        }

        void Update()
        {
            HandleZoom();

            HandlePanning();
        }

        private void HandlePanning()
        {
            if (Input.GetMouseButtonDown(1))
            {
                _dragOrigin = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
                return;
            }

            if (Input.GetMouseButton(1))
            {
                Vector3 difference = _dragOrigin - _mainCamera.ScreenToWorldPoint(Input.mousePosition);
                _mainCamera.transform.position += difference;
                ClampCameraPosition();
            }
        }

        private void HandleZoom()
        {
            var mainCamOrthographicSize = _mainCamera.orthographicSize;
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0.0f)
            {
                mainCamOrthographicSize -= scroll * zoomSpeed;
                _mainCamera.orthographicSize = Mathf.Clamp(mainCamOrthographicSize, minZoom, maxZoom);
            }
        }

        private void ClampCameraPosition()
        {
            Vector3 pos = _mainCamera.transform.position;
            pos.x = Mathf.Clamp(pos.x, minBoundary.x, maxBoundary.x);
            pos.y = Mathf.Clamp(pos.y, minBoundary.y, maxBoundary.y);
            _mainCamera.transform.position = pos;
        }
    }
}