using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionGizmo : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private GameObject target;
    private RectTransform canvasArea;
    private Action<GameObject> onSelect;
    private bool dragging = false;
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

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Traslation Down");
        dragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!dragging || target == null || canvasArea == null)
            return;

        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasArea, eventData.position, eventData.pressEventCamera, out localPoint))
        {

            // Convertir posición de pantalla a mundo real
            Vector3 worldPos = eventData.pressEventCamera.ScreenToWorldPoint(eventData.position);
            worldPos.z = target.transform.position.z;



            // Mover el objeto en la escena
            target.transform.position = worldPos;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (onSelect != null)
        {
            onSelect.Invoke(target);
        }
        dragging = false;
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
