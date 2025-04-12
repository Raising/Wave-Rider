using UnityEngine;
using UnityEngine.EventSystems;

public class ScaleGizmo: MonoBehaviour, IGizmoHandler, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private GameObject target;
    private RectTransform canvasArea;
    private float initialMouseY;
    private Vector3 initialScale;
    private bool dragging;

    public void SetTarget(GameObject target, RectTransform canvas)
    {
        this.target = target;
        this.canvasArea = canvas;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Scale Down");
        if (target == null) return;
        dragging = true;
        initialMouseY = eventData.position.y;
        initialScale = target.transform.localScale;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!dragging || target == null) return;

        float deltaY = eventData.position.y - initialMouseY;
        int steps = Mathf.FloorToInt(deltaY / 10f);
        float factor = Mathf.Pow(1.05f, steps);

        target.transform.localScale = new Vector3(initialScale.x * factor, initialScale.y * factor, initialScale.z);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        dragging = false;
    }
}
