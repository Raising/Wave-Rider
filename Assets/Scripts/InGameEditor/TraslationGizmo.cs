using UnityEngine;
using UnityEngine.EventSystems;

public class TraslationGizmo : MonoBehaviour, IGizmoHandler, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private GameObject target;
    private RectTransform canvasArea;
    private bool dragging = false;

    public void SetTarget(GameObject target, RectTransform canvas)
    {
        this.target = target;
        this.canvasArea = canvas;
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
            worldPos.x = worldPos.x - 0.4f;
            worldPos.y = worldPos.y + 0.4f;
            

            // Mover el objeto en la escena
            target.transform.position = worldPos;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        dragging = false;
    }


}
