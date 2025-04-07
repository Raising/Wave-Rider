using UnityEngine;
using UnityEngine.EventSystems;

public class TraslationGizmo : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private GameObject referencedObject;
    private RectTransform canvasInteractionArea;
    private bool dragging = false;


    public void Update()
    {
        if (!dragging)
        {
            AdjutToGameObject();
        }
    }
    public void SetReferencedObject(GameObject go, GameObject canvasArea)
    {
        referencedObject = go;
        canvasInteractionArea = canvasArea.GetComponent<RectTransform>();

        AdjutToGameObject();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        dragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!dragging || referencedObject == null || canvasInteractionArea == null)
            return;

        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasInteractionArea, eventData.position, eventData.pressEventCamera, out localPoint))
        {
            // Mover el gizmo dentro del canvas
            GetComponent<RectTransform>().localPosition = new Vector3(localPoint.x, localPoint.y, -1);

            // Convertir posición de pantalla a mundo real
            Vector3 worldPos = eventData.pressEventCamera.ScreenToWorldPoint(eventData.position);
            worldPos.z = 0;

            // Mover el objeto en la escena
            referencedObject.transform.position = worldPos;
        }
    }

    private void AdjutToGameObject()
    {
        if (referencedObject == null || canvasInteractionArea == null)
            return;

        // Convertir posición del objeto del mundo a coordenadas locales del canvas
        Vector2 screenPos = Camera.main.WorldToScreenPoint(referencedObject.transform.position);
        Vector2 localPoint;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasInteractionArea, screenPos, Camera.main, out localPoint))
        {
            // Mover el gizmo a la posición correcta dentro del canvas
            GetComponent<RectTransform>().localPosition = new Vector3(localPoint.x, localPoint.y, -1);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        dragging = false;
    }
}
