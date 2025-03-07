using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


#if UNITY_EDITOR
using static UnityEditor.PlayerSettings;
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

public class ShapesMatchCollider : MonoBehaviour
{
    public PolygonCollider2D polygonCollider;
    public Shapes.Polygon polygonBody;
    public Shapes.Polygon baseBorderColor;
    public Shapes.Polygon whiteBorderColor;
    private Boolean privateDirty = false;
    // Start is called before the first frame update

    void Awake()
    {
        //ApplyPolygonChanges();
    }

    void Start()
    {

    }

    void ApplyPolygonChanges()
    {
        if (polygonCollider == null)
        {
            Debug.LogError("PolygonCollider2D is missing!");
            return;
        }

        Vector2[] points = polygonCollider.points;
        Vector2 center = CalculateCenter(points);
        Vector2[] modifiedPoints = ModifyPoints(points, center, 0.2f); // Desplazamiento base 0.1f
        Vector2[] borderPoints = ModifyPoints(points, center, 0.1f); // Un poco menos para el borde

        polygonBody.SetPoints(modifiedPoints);
        baseBorderColor.SetPoints(borderPoints);
        whiteBorderColor.SetPoints(points);

        if (!privateDirty)
        {
            privateDirty = true;
#if UNITY_EDITOR
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            EditorUtility.SetDirty(polygonBody);
            EditorUtility.SetDirty(baseBorderColor);
            EditorUtility.SetDirty(whiteBorderColor);
#endif
        }
    }
    float SineBetweenVectors(Vector2 v1, Vector2 v2)
    {
        float crossProduct = v1.x * v2.y - v1.y * v2.x; // Producto cruzado en 2D
        float magnitudeProduct = v1.magnitude * v2.magnitude;

        if (magnitudeProduct == 0) return 0; // Evitar división por cero

        return crossProduct / magnitudeProduct; // Devuelve el seno del ángulo
    }
    Vector2 CalculateCenter(Vector2[] points)
    {
        float minX = float.MaxValue, maxX = float.MinValue;
        float minY = float.MaxValue, maxY = float.MinValue;

        foreach (Vector2 point in points)
        {
            if (point.x < minX) minX = point.x;
            if (point.x > maxX) maxX = point.x;
            if (point.y < minY) minY = point.y;
            if (point.y > maxY) maxY = point.y;
        }

        return new Vector2((minX + maxX) / 2f, (minY + maxY) / 2f);
    }

    Vector2[] ModifyPoints(Vector2[] points, Vector2 center, float shrinkAmount)
    {
        Vector3 scale = new Vector2(Math.Abs(transform.lossyScale.x), Math.Abs(transform.lossyScale.y));
        float adjustedShrink = shrinkAmount; // / ((scale.x + scale.y) / 2f); // Escala promedio

        Vector2[] newPoints = new Vector2[points.Length];

        for (int i = 0; i < points.Length; i++)
        {
            // Obtener los puntos anterior y siguiente en el array (conectando el último con el primero)
            Vector2 prevPoint = points[(i - 1 + points.Length) % points.Length];
            Vector2 nextPoint = points[(i + 1) % points.Length];
            Vector2 edge1 = (prevPoint - points[i]).normalized;
            Vector2 edge2 = (nextPoint - points[i]).normalized;
            Vector2 direction = edge1 + edge2;
            Vector2 scaledDirection = new Vector2(direction.x / scale.x, direction.y / scale.y);
            float sin = Math.Abs(SineBetweenVectors(edge1, edge2));
            if (sin == 0)
            {
                sin = 1;
            }
            // Verificar si el movimiento aleja el punto del centro
            Vector2 movedPoint = points[i] - scaledDirection * adjustedShrink / sin;
            if (Vector2.Distance(movedPoint, center) > Vector2.Distance(points[i], center))
            {
                scaledDirection *= -1; // Invertir la dirección si se aleja
            }

            // Calcular los vectores de los segmentos


            newPoints[i] = points[i] - scaledDirection * adjustedShrink / sin;
        }

        return newPoints;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        ApplyPolygonChanges();
    }
#endif
}