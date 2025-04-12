using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;

public class UIElementCreator : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public GameObject elementPrefab;
    public LevelEditorManager levelEditorManager;
    private GameObject target;
    public RectTransform canvasArea;
    private bool dragging = false;
    // Start is called before the first frame update
    void Start()
    {
        levelEditorManager = FindObjectOfType<LevelEditorManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        target = levelEditorManager.CreateElement(elementPrefab, eventData);
        dragging = true;
    }


    public void SetTarget(GameObject target, RectTransform canvas)
    {
        this.target = target;
        this.canvasArea = canvas;
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
        dragging = false;
    }

}
