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
        private Vector3 dragOrigin;
        private Camera mainCamera;

        private void Start()
        {
            mainCamera = gameObject.GetComponent<Camera>();
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
                dragOrigin = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                return;
            }

            if (Input.GetMouseButton(1))
            {
                Vector3 difference = dragOrigin - mainCamera.ScreenToWorldPoint(Input.mousePosition);
                mainCamera.transform.position += difference;
                ClampCameraPosition();
            }
        }

        private void HandleZoom()
        {
            var mainCamOrthoSize = mainCamera.orthographicSize;
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0.0f)
            {
                mainCamOrthoSize -= scroll * zoomSpeed;
                mainCamera.orthographicSize = Mathf.Clamp(mainCamOrthoSize, minZoom, maxZoom);
            }
        }

        private void ClampCameraPosition()
        {
            Vector3 pos = mainCamera.transform.position;
            pos.x = Mathf.Clamp(pos.x, minBoundary.x, maxBoundary.x);
            pos.y = Mathf.Clamp(pos.y, minBoundary.y, maxBoundary.y);
            mainCamera.transform.position = pos;
        }
    }
}

