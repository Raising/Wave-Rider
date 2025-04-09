using UnityEngine;
using UnityEngine.EventSystems;

public class RotationGizmo : MonoBehaviour, IGizmoHandler, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private GameObject target;
    private RectTransform canvasArea;
    private bool dragging = false;

    private Vector3 initialDirection;
    private float initialAngle;

    public void SetTarget(GameObject target, RectTransform canvas)
    {
        this.target = target;
        this.canvasArea = canvas;
        //UpdatePosition();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Rotation Down");
            
        if (target == null || canvasArea == null) return;

        dragging = true;

        Vector3 targetScreenPos = Camera.main.WorldToScreenPoint(target.transform.position);
        initialDirection = (eventData.position - new Vector2(targetScreenPos.x, targetScreenPos.y)).normalized;
        initialAngle = target.transform.eulerAngles.z;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!dragging || target == null || canvasArea == null)
            return;

        Vector3 targetScreenPos = Camera.main.WorldToScreenPoint(target.transform.position);
        Vector2 currentDirection = (eventData.position - new Vector2(targetScreenPos.x, targetScreenPos.y)).normalized;

        float angleDiff = Vector2.SignedAngle(initialDirection, currentDirection);
        target.transform.rotation = Quaternion.Euler(0, 0, initialAngle + angleDiff);

        //UpdatePosition(); // mantener el gizmo centrado en el objeto
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        dragging = false;
    }

    //private void Update()
    //{
    //    if (!dragging)
    //    {
    //        UpdatePosition();
    //    }
    //}

    //private void UpdatePosition()
    //{
    //    if (target == null || canvasArea == null)
    //        return;

    //    Vector2 screenPos = Camera.main.WorldToScreenPoint(target.transform.position);
    //    Vector2 localPoint;

    //    if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
    //        canvasArea, screenPos, Camera.main, out localPoint))
    //    {
    //        GetComponent<RectTransform>().localPosition = new Vector3(localPoint.x, localPoint.y, -1);
    //    }
    //}
}
