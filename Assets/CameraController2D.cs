using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController2D : MonoBehaviour
{
    public float dragSpeed = 0.01f;
    public float zoomSpeed = 0.5f;
    public float minZoom = 1f;
    private float maxZoom = 20f;

    private Camera cam;
    private Vector3 lastMousePos;
    private Vector3 lastPanFingerPos;
    private bool isDragging = false;
    private Vector2 minCameraPos = new Vector2(-10f, -10f);
    private Vector2 maxCameraPos = new Vector2(10f, 10f);

    void Start()
    {
        cam = Camera.main;
        RecalibrateZoom(cam.orthographicSize);

    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        HandleMouseDrag();
        HandleMouseZoom();
        HandleTouchDragZoom();
        ClampCameraPosition();
    }

    public void RecalibrateZoom(float ortographicSize)
    {
        maxZoom = ortographicSize;
        minCameraPos.y = -1 * maxZoom;
        maxCameraPos.y = maxZoom;
        minCameraPos.x = -2f * maxZoom;
        maxCameraPos.x = 2f * maxZoom;
    }

    // 🖱️ Mover con mouse (clic izquierdo)
    void HandleMouseDrag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePos = Input.mousePosition;
            isDragging = true;
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            Vector3 delta = cam.ScreenToWorldPoint(lastMousePos) - cam.ScreenToWorldPoint(Input.mousePosition);
            transform.position += delta;
            lastMousePos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }

    // 🖱️ Zoom con scroll del ratón
    void HandleMouseZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            cam.orthographicSize -= scroll * zoomSpeed * 10f;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        }
    }

    // 📱 Pinch zoom y drag con dedos
    void HandleTouchDragZoom()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                Vector3 delta = cam.ScreenToWorldPoint(touch.position - touch.deltaPosition) - cam.ScreenToWorldPoint(touch.position);
                transform.position += delta;
            }
        }
        else if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            // Zoom con pinch
            Vector2 prevTouch0 = touch0.position - touch0.deltaPosition;
            Vector2 prevTouch1 = touch1.position - touch1.deltaPosition;

            float prevMagnitude = (prevTouch0 - prevTouch1).magnitude;
            float currentMagnitude = (touch0.position - touch1.position).magnitude;

            float difference = prevMagnitude - currentMagnitude;

            cam.orthographicSize += difference * zoomSpeed * Time.deltaTime;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        }
    }
    void ClampCameraPosition()
    {
        float vertExtent = cam.orthographicSize;
        float horzExtent = vertExtent * cam.aspect;

        float minX = minCameraPos.x + horzExtent;
        float maxX = maxCameraPos.x - horzExtent;
        float minY = minCameraPos.y + vertExtent;
        float maxY = maxCameraPos.y - vertExtent;

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        transform.position = pos;
    }

}
