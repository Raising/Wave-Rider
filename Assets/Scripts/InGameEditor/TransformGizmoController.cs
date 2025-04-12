using UnityEngine;

public class TransformGizmoController : MonoBehaviour
{
    public GameObject target;
    private RectTransform canvasArea;

    public void SetTarget(GameObject newTarget, RectTransform canvasArea)
    {
        this.canvasArea = canvasArea;
        
        target = newTarget;

        // Notificar a todos los handlers hijos
        foreach (var handler in GetComponentsInChildren<IGizmoHandler>())
        {
            handler.SetTarget(target, canvasArea);
        }
    }

    private void Update()
    {

        UpdatePosition();

    }

    private void UpdatePosition()
    {
        if (target == null || canvasArea == null)
            return;

        Vector2 screenPos = Camera.main.WorldToScreenPoint(target.transform.position);
        Vector2 localPoint;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasArea, screenPos, Camera.main, out localPoint))
        {
            GetComponent<RectTransform>().localPosition = new Vector3(localPoint.x, localPoint.y, -1);
        }
    }
}

public interface IGizmoHandler
{
    void SetTarget(GameObject target, RectTransform canvasArea);
}