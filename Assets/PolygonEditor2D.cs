using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolygonEditor2D : MonoBehaviour
{
    public float handleRadius = 0.1f;
    private PolygonCollider2D polygon;
    private BaseObstacle obsScript;
    private int selectedIndex = -1;

    void Start()
    {
        obsScript = GetComponent<BaseObstacle>();
        polygon = GetComponent<PolygonCollider2D>();
        if (polygon == null)
        {
            Debug.LogError("No PolygonCollider2D found!");
        }
    }

    void OnDrawGizmos()
    {
        if (polygon == null) return;

        Gizmos.color = Color.yellow;
        Vector2[] points = polygon.points;

        // Mostrar vértices
        for (int i = 0; i < points.Length; i++)
        {
            Vector2 worldPoint = transform.TransformPoint(points[i]);
            Gizmos.DrawSphere(worldPoint, handleRadius);
        }
    }

    void Update()
    {
        if (polygon == null) return;

        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2[] points = polygon.points;

        if (Input.GetMouseButtonDown(0))
        {
            // Detectar si se hizo clic cerca de algún vértice
            for (int i = 0; i < points.Length; i++)
            {
                Vector2 worldPoint = transform.TransformPoint(points[i]);
                if (Vector2.Distance(mouseWorldPos, worldPoint) < handleRadius)
                {
                    selectedIndex = i;
                    break;
                }
            }
        }

        if (Input.GetMouseButton(0) && selectedIndex != -1)
        {
            // Mover el vértice seleccionado
            points[selectedIndex] = transform.InverseTransformPoint(mouseWorldPos);
            polygon.points = points;
            obsScript.ApplyPolygonChanges();
        }

        if (Input.GetMouseButtonUp(0))
        {
            selectedIndex = -1;
        }
    }
}
