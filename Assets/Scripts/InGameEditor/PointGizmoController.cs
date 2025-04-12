using System.Reflection;
using UnityEngine;

public class PointGizmoController : MonoBehaviour
{
    public GameObject target;
    private RectTransform canvasArea;
    public GameObject PointHandlerPrefab;

    public void SetTarget(GameObject newTarget, RectTransform canvasArea)
    {
        this.canvasArea = canvasArea;

        target = newTarget;

        // Notificar a todos los handlers hijos
        PolygonCollider2D poligon = target.GetComponent<PolygonCollider2D>();

        for (int i = 0; i < poligon.points.Length; i++)

        {
            GameObject go = Instantiate(PointHandlerPrefab, this.transform);
            PointGizmoHandler handler = go.GetComponent<PointGizmoHandler>();
            handler.SetTarget(target, i, canvasArea);
        }
    }

    private void Update()
    {

        //UpdatePosition();

    }

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
