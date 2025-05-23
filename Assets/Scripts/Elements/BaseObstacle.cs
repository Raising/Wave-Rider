using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.UIElements;
using Newtonsoft.Json;


#if UNITY_EDITOR
using static UnityEditor.PlayerSettings;
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

public enum ObstacleType
{
    normal,
    killer,
    dumper,
    banger,
}

[System.Serializable]
public class ObstacleData
{
    public SerializableVector2[] points = new SerializableVector2[0];
    public SerializableVector3 position = new SerializableVector3();
    public SerializableVector2 scale = new SerializableVector2();
    public float rotation = 0;
}

public interface PolyEdit
{
    void ApplyPolygonChanges();

}
public abstract class Obstacle : LevelElementBase, PolyEdit
{
    public PolygonCollider2D polygonCollider;
    private ObstacleRenderer obstacleRender;
    public void ApplyPolygonChanges()
    {
        if (polygonCollider == null)
        {
            Debug.LogError("PolygonCollider2D is missing!");
            return;
        }
        if (obstacleRender == null)
        {
            obstacleRender = FindObjectOfType<ObstacleRenderer>();
        }

        obstacleRender.UpdateRender();
    }
}

public class BaseObstacle : Obstacle 
{
    public Shapes.Polygon polygonBody;
    public Shapes.Polygon baseBorderColor;
    public Shapes.Polygon whiteBorderColor;
    public ObstacleType type = ObstacleType.normal;
    
    // Start is called before the first frame update

    public override string Type()
    {
        return "BaseObstacle";
    }
    void Awake()
    {
        ApplyPolygonChanges();
    }

    void Start()
    {

    }

  
    float SineBetweenVectors(Vector2 v1, Vector2 v2)
    {
        float crossProduct = v1.x * v2.y - v1.y * v2.x; // Producto cruzado en 2D
        float magnitudeProduct = v1.magnitude * v2.magnitude;

        if (magnitudeProduct == 0) return 0; // Evitar divisi�n por cero

        return crossProduct / magnitudeProduct; // Devuelve el seno del �ngulo
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
            // Obtener los puntos anterior y siguiente en el array (conectando el �ltimo con el primero)
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
                scaledDirection *= -1; // Invertir la direcci�n si se aleja
            }

            // Calcular los vectores de los segmentos


            newPoints[i] = points[i] - scaledDirection * adjustedShrink / sin;
        }

        return newPoints;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        //ApplyPolygonChanges();
    }

#endif

    public override ElementData AsLevelData()
    {
        return new ElementData
        {
            type = this.Type(),
            data = JsonConvert.SerializeObject(new ObstacleData()
            {
                points = this.polygonCollider.points.Select(p => new SerializableVector2(p)).ToArray(),
                position = new SerializableVector3(this.transform.position),
                scale = new SerializableVector2(this.transform.localScale),
                rotation = this.transform.rotation.z,
            }),

        };
    }

    public override void LoadFromLevelData(ElementData elementData)
    {
        ObstacleData data = JsonUtility.FromJson<ObstacleData>(elementData.data); 
        this.polygonCollider.points = data.points.Select(p => p.ToVector2()).ToArray();
        this.transform.position = data.position.ToVector3();
        this.transform.rotation = Quaternion.Euler(0, 0, data.rotation);
        this.transform.localScale = data.scale.ToVector2();

        this.ApplyPolygonChanges();
    }

    public override void SetInert()
    {
        GetComponent<PolygonCollider2D>().enabled = false;
    }
}