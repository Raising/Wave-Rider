using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionGizmo : MonoBehaviour, IPointerClickHandler
{
    public GameObject target;
    private RectTransform canvasArea;
    private Action<GameObject> onSelect;

    public void SetTarget(GameObject newTarget, RectTransform canvasArea, Action<GameObject> onSelect)
    {
        this.onSelect = onSelect;
        this.canvasArea = canvasArea;
        target = newTarget;
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (onSelect != null)
        {
            onSelect.Invoke(target);
        }
    }
}
