using UnityEngine;
using UnityEngine.EventSystems;

public class PointGizmoHandler : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private GameObject target;
    private RectTransform canvasArea;
    private bool dragging = false;
    private int pointIndex;

    public void SetTarget(GameObject target, int pointIndex, RectTransform canvas)
    {
        this.target = target;
        this.pointIndex = pointIndex;
        this.canvasArea = canvas;

        UpdatePositionToColliderPoint();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
        dragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!dragging || target == null || canvasArea == null)
            return;

        Vector2 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        worldPos = target.transform.InverseTransformPoint(worldPos); // convertir a espacio local del collider

        PolygonCollider2D poly = target.GetComponent<PolygonCollider2D>();
        if (poly != null && pointIndex >= 0 && pointIndex < poly.points.Length)
        {
            Vector2[] points = poly.points;
            points[pointIndex] = worldPos;
            poly.points = points;

            PolyEdit polyScript = target.GetComponent<PolyEdit>();
            if (polyScript != null)
            {
                polyScript.ApplyPolygonChanges();
            }
            UpdatePositionToColliderPoint();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        dragging = false;
    }
    private void UpdatePositionToColliderPoint()
    {
        if (target == null || canvasArea == null)
            return;

        PolygonCollider2D poly = target.GetComponent<PolygonCollider2D>();
        if (poly == null || pointIndex < 0 || pointIndex >= poly.points.Length)
            return;

        // Obtener el punto del polígono en coordenadas de mundo
        Vector2 worldPoint = target.transform.TransformPoint(poly.points[pointIndex]);
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(worldPoint);

      

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasArea, screenPoint, canvasArea.GetComponentInParent<Canvas>().worldCamera, out localPoint);

        // Posicionar el gizmo
        GetComponent<RectTransform>().localPosition = localPoint;
    }



}
