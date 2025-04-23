using System.Collections.Generic;
using UnityEngine;
using Shapes;
using System;
using Unity.Mathematics;

[ExecuteAlways]
public class ObstacleRenderer : MonoBehaviour
{
    public GameObject quadPrefab; // Debe contener un componente Shapes.Quad
    public GameObject surfacePrefab;
    public Vector2[][] contours;
    public float inset = 0.2f;
    private List<List<Vector2>> debugPoints = new List<List<Vector2>>();

    private List<GameObject> instances = new List<GameObject>();


    private void OnDrawGizmos()
    {
        for (int j = 0; j < debugPoints.Count; j++)
        {
            List<Vector2> points = debugPoints[j];
            for (int i = 0; i < points.Count; i++)
            {
                Gizmos.DrawLine(points[i], points[(i + 1) % points.Count]); // solo borde
            }
        }
    }
    public void UpdateRender()
    {
        ClearInstances();
        bool skip = false;
        if (quadPrefab == null || contours == null || contours.Length < 1)
            return;
        for (int j = 0; j < contours.Length; j++)
        {
            List<Vector2> groupDebugPoints = new List<Vector2>();
            List<Vector2> surfacePoints = new List<Vector2>();

            Vector2[] contour = contours[j];

            for (int i = 0; i < contour.Length; i++)
            {
                Vector2 prev = contour[(i - 1 + contour.Length) % contour.Length];
                Vector2 A = contour[i];
                Vector2 B = contour[(i + 1) % contour.Length];
                Vector2 next = contour[(i + 2) % contour.Length];

                // Normales a los segmentos
                Vector2 dirPrev = (A - prev).normalized;
                Vector2 dirThis = (B - A).normalized;
                Vector2 dirNext = (next - B).normalized;

                Vector2 normalPrev = new Vector2(dirPrev.y, -dirPrev.x);
                Vector2 normalThis = new Vector2(dirThis.y, -dirThis.x);
                Vector2 normalNext = new Vector2(dirNext.y, -dirNext.x);

                //// Asegurar que las normales apuntan hacia dentro (verificando contra normal del segmento actual)
                //Vector2 outwardNormal = new Vector2(-dirThis.y, dirThis.x);
                //if (Vector2.Dot(normalPrev, outwardNormal) > 0) normalPrev = -normalPrev;
                //if (Vector2.Dot(normalThis, outwardNormal) > 0) normalThis = -normalThis;
                //if (Vector2.Dot(normalNext, outwardNormal) > 0) normalNext = -normalNext;

                // Líneas desplazadas
                Vector2 A1 = prev + normalPrev * inset;
                Vector2 A2 = A + normalPrev * inset;

                Vector2 B1 = A + normalThis * inset;
                Vector2 B2 = B + normalThis * inset;

                Vector2 C1 = B + normalNext * inset;
                Vector2 C2 = next + normalNext * inset;

                Vector3 q0 = A;
                Vector3 q1 = B;
                // Intersecciones: desplazadas hacia dentro
                bool toSteepQ3 = Trigonometrics.LineIntersection(A1, A2, B1, B2, inset, out Vector3 q3); // interior en A
                bool toSteepQ2 = Trigonometrics.LineIntersection(B1, B2, C1, C2, inset, out Vector3 q2); // interior en B, q0



                if (toSteepQ3)
                {
                    DrawGradientQuad(q0, q0, B1, q3);
                    surfacePoints.Add(A2);
                    surfacePoints.Add(q3);
                    //debugPoints.Add(new List<Vector2>() { q3, B1 });
                    q3 = B1;
                }

                surfacePoints.Add(q3);
                if (toSteepQ2)
                {
                    DrawGradientQuad(q1, q1, q2, B2);
                    //surfacePoints.Add(q2);
                    //debugPoints.Add(new List<Vector2>() { q2, B2 });
                    q2 = B2;
                }
                if (!skip)
                {

                    //groupDebugPoints.Add(q3);
                    //groupDebugPoints.Add(q2);
                }
                skip = skip ? false : true;
                DrawGradientQuad(q0, q1, q2, q3);

            }
            debugPoints.Add(groupDebugPoints);
            DrawSurfacePolygon(surfacePoints);



        }
    }
    void DrawSurfacePolygon(List<Vector2> surfacePoints)

    {
        GameObject surfaceObj = Instantiate(surfacePrefab, transform);
        var polygon = surfaceObj.GetComponent<Shapes.Polygon>();
        polygon.points = surfacePoints;
        instances.Add(surfaceObj);
    }

    void DrawGradientQuad(Vector2 q0, Vector2 q1, Vector2 q2, Vector2 q3)

    {
        GameObject quadObj = Instantiate(quadPrefab, transform);
        var quads = quadObj.GetComponentsInChildren<Shapes.Quad>();
        var quad = quads[0];
        var shadow = quads[1];
        Vector2 shadowDisplacement = new Vector2(0.1f, -0.1f);
        if (quad != null)
        {
            quad.SetQuadVertex(0, q0);
            quad.SetQuadVertex(1, q1);
            quad.SetQuadVertex(2, q2);
            quad.SetQuadVertex(3, q3);


            Vector2 lightDir = new Vector2(-1f, -1f).normalized;

            // Dirección del borde iluminado (q3 ← q2)
            Vector2 edgeDir = (q3 - q2).normalized;

            // Ángulo relativo entre la dirección de luz y el borde
            float dot = Vector2.Dot(edgeDir, lightDir); // -1 (opuesto) a +1 (alineado)

            // Mapear dot [-1, 1] a factor de brillo [-0.4, 0.2]
            float brightnessFactor = Mathf.Lerp(-0.1f, 0.05f, (dot + 1f) / 2f);

            // Aplicar el factor a un color base
            // Aplicar color al quad (misma luz en todas las esquinas)

            quad.ColorA = OKLabColor.AdjustLightness(quad.ColorA, brightnessFactor);
            quad.ColorB = OKLabColor.AdjustLightness(quad.ColorA, brightnessFactor);
            quad.ColorC = OKLabColor.AdjustLightness(quad.ColorC, brightnessFactor);
            quad.ColorD = OKLabColor.AdjustLightness(quad.ColorC, brightnessFactor);
            float intensitiColor = 0.3f + brightnessFactor * 3;
            Color shadowColorA = new Color(intensitiColor, intensitiColor, intensitiColor, 0);
            Color shadowColorB = new Color(intensitiColor, intensitiColor, intensitiColor, 0.5f);
            shadow.ColorA = shadowColorA;
            shadow.ColorB = shadowColorA;
            shadow.ColorC = shadowColorB;
            shadow.ColorD = shadowColorB;
            shadow.SetQuadVertex(0, q0 + (q0 - q3).normalized * 0.1f);
            shadow.SetQuadVertex(1, q1 + (q1 - q2).normalized * 0.1f);
            shadow.SetQuadVertex(2, q1);
            shadow.SetQuadVertex(3, q0);
        }

        instances.Add(quadObj);
    }

    void ClearInstances()
    {
        foreach (var go in instances)
        {
            if (go != null)
                DestroyImmediate(go);
        }
        debugPoints.Clear();
        instances.Clear();
    }
}
