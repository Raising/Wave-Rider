
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using static UnityEngine.Rendering.HableCurve;




[ExecuteAlways]
public class ObstacleCanvas : MonoBehaviour
{
    public float insetDistance = 0.2f;

    private List<BaseObstacle> allObstacles = new List<BaseObstacle>();
    private List<Vector2[]> drawnPolygons = new List<Vector2[]>();
    private Dictionary<BaseObstacle, Vector2[]> previousPoints = new Dictionary<BaseObstacle, Vector2[]>();
    public ObstacleRenderer obstacleRenderer;
    public int maxLength = 4;
    void Update()
    {


        RefreshObstacleList();
        if (AnyObstacleChanged())
        {
            Rebuild();
            obstacleRenderer.contours = drawnPolygons.ToArray();
            obstacleRenderer.UpdateRender();
        }

    }

    void RefreshObstacleList()
    {
        allObstacles = FindObjectsOfType<BaseObstacle>().ToList();
    }

    bool AnyObstacleChanged()
    {
        foreach (var obs in allObstacles)
        {
            var points = obs.polygonCollider.points;
            if (!previousPoints.ContainsKey(obs) || !PointsEqual(previousPoints[obs], points) || obs.transform.hasChanged)
            {
                previousPoints[obs] = (Vector2[])points.Clone();
                obs.transform.hasChanged = false; // reset the flag
                return true;
            }
        }
        return false;
    }
    //void OnDrawGizmos()
    //{
    //    if (drawnPolygons == null) return;
    //    Gizmos.color = Color.red;

    //    foreach (var poly in drawnPolygons)
    //    {
    //        for (int i = 0; i < poly.Length; i++)
    //        {
    //            Vector2 p0 = poly[i];
    //            Vector2 p1 = poly[(i + 1) % poly.Length];

    //            Gizmos.DrawLine(p0, p1); // solo borde
    //        }
    //    }
    //}

    bool PointsEqual(Vector2[] a, Vector2[] b)
    {
        if (a.Length != b.Length) return false;
        for (int i = 0; i < a.Length; i++)
        {
            if (Vector2.Distance(a[i], b[i]) > 0.001f) return false;
        }
        return true;
    }

    void Rebuild()
    {
        drawnPolygons.Clear();
        var grupos = AgruparObstaculos();
        //Debug.Log($"ObstáculoCanvas: {grupos.Count} grupos encontrados.");
        foreach (var grupo in grupos)
        {
            var union = CalcularUnion(grupo);
            //Debug.Log($"Grupo tiene {union.Length} puntos en el contorno.");
            if (union.Length > 0)
                drawnPolygons.Add(union);
        }
    }

    List<List<BaseObstacle>> AgruparObstaculos()
    {
        var grupos = new List<List<BaseObstacle>>();
        var visitados = new HashSet<BaseObstacle>();

        foreach (var obs in allObstacles)
        {
            if (visitados.Contains(obs)) continue;
            var grupo = new List<BaseObstacle>();
            var cola = new Queue<BaseObstacle>();
            cola.Enqueue(obs);

            while (cola.Count > 0)
            {
                var actual = cola.Dequeue();
                if (visitados.Contains(actual)) continue;

                visitados.Add(actual);
                grupo.Add(actual);

                foreach (var otro in allObstacles)
                {
                    if (actual == otro || visitados.Contains(otro)) continue;
                    if (Intersectan(actual, otro))
                    {
                        cola.Enqueue(otro);
                    }
                }
            }
            grupos.Add(grupo);
        }
        return grupos;
    }

    bool Intersectan(BaseObstacle a, BaseObstacle b)
    {
        var worldA = GetWorldPoints(a);
        var worldB = GetWorldPoints(b);

        foreach (var p in worldA)
            if (PointInPolygon(p, worldB)) return true;

        foreach (var p in worldB)
            if (PointInPolygon(p, worldA)) return true;

        for (int i = 0; i < worldA.Length; i++)
        {
            Vector2 a1 = worldA[i];
            Vector2 a2 = worldA[(i + 1) % worldA.Length];
            for (int j = 0; j < worldB.Length; j++)
            {
                Vector2 b1 = worldB[j];
                Vector2 b2 = worldB[(j + 1) % worldB.Length];
                if (SegmentsIntersect(a1, a2, b1, b2)) return true;
            }
        }

        return false;
    }

    Vector2[] GetWorldPoints(BaseObstacle obs)
    {
        return obs.polygonCollider.points.Select(p => (Vector2)obs.transform.TransformPoint(p)).ToArray();
    }

    bool PointInPolygon(Vector2 point, Vector2[] polygon)
    {
        bool inside = false;
        for (int i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)
        {
            if ((polygon[i].y > point.y) != (polygon[j].y > point.y) &&
                point.x < (polygon[j].x - polygon[i].x) * (point.y - polygon[i].y) /
                (polygon[j].y - polygon[i].y) + polygon[i].x)
            {
                inside = !inside;
            }
        }
        return inside;
    }

    bool SegmentsIntersect(Vector2 p1, Vector2 p2, Vector2 q1, Vector2 q2)
    {
        return Orientation(p1, p2, q1) != Orientation(p1, p2, q2) &&
               Orientation(q1, q2, p1) != Orientation(q1, q2, p2);
    }

    int Orientation(Vector2 a, Vector2 b, Vector2 c)
    {
        float val = (b.y - a.y) * (c.x - b.x) - (b.x - a.x) * (c.y - b.y);
        if (Mathf.Abs(val) < 1e-6f) return 0;
        return (val > 0) ? 1 : 2;
    }
    bool TryIntersect(Vector2 p1, Vector2 p2, Vector2 q1, Vector2 q2, out Vector2 intersection, float epsilon = 1e-6f)
    {
        intersection = Vector2.zero;

        Vector2 r = p2 - p1;
        Vector2 s = q2 - q1;
        float rxs = r.x * s.y - r.y * s.x;
        Vector2 qp = q1 - p1;

        // 1. Colineales
        if (Mathf.Abs(rxs) < epsilon)
        {
            float qpxr = qp.x * r.y - qp.y * r.x;

            if (Mathf.Abs(qpxr) < epsilon)
            {
                float rDotR = Vector2.Dot(r, r);

                float t0 = Vector2.Dot(q1 - p1, r) / rDotR;
                float t1 = Vector2.Dot(q2 - p1, r) / rDotR;

                float tMin = Mathf.Min(t0, t1);
                float tMax = Mathf.Max(t0, t1);

                // Hay solapamiento
                if (tMax >= 0 && tMin <= 1)
                {
                    // Solo extender si q2 está más lejos que p2
                    Vector2 extension = q2 - p1;
                    if (Vector2.Dot(extension, r) > rDotR)
                    {
                        intersection = q2;
                        return true;
                    }
                }
            }

            return false;
        }

        // 2. Intersección estándar
        float t = (qp.x * s.y - qp.y * s.x) / rxs;
        float u = (qp.x * r.y - qp.y * r.x) / rxs;

        if (t >= 0 && t <= 1 && u >= 0 && u <= 1)
        {
            intersection = p1 + t * r;
            return true;
        }

        return false;
    }

    Segmento[] GetSegmentChain(Segmento starter, List<BaseObstacle> grupo)
    {

        List<Segmento> rawSegments = new List<Segmento>();
        List<Segmento> segmentChain = new List<Segmento>();
        foreach (var obs in grupo)
        {
            var points = GetWorldPoints(obs);
            for (int i = 0; i < points.Length; i++)
            {
                rawSegments.Add(new Segmento(points[i], points[(i + 1) % points.Length]));
            }
        }
        Segmento current = new Segmento(starter.start, starter.end);

        while (segmentChain.Count < maxLength && (segmentChain.Count == 0 || Vector2.SqrMagnitude(current.start - segmentChain[0].start) > 1e-6f))
        {
            List<Segmento> interSegment = new List<Segmento> { };
            foreach (var seg in rawSegments)
            {
                if (Vector2.SqrMagnitude(current.end - seg.end) < 1e-6f || Vector2.SqrMagnitude(current.start - seg.end) < 1e-6f)
                    continue;
                if (Vector2.SqrMagnitude(current.end - seg.start) < 1e-6f)
                {
                    interSegment.Add(seg);
                    continue;
                }
                if (current.IsParallel(seg))
                {
                    if (TryIntersect(current.start, current.end, seg.start, seg.end, out Vector2 t))
                    {
                        Vector2 midpoint = (current.end + seg.end + current.start + seg.start) / 4;
                        Vector2 furtherEnd = new Vector2[] { current.end, seg.end }.OrderBy(x => Vector2.SqrMagnitude(midpoint - x)).FirstOrDefault();


                        if (segmentChain.Count == 0)
                        {
                            Vector2 furtherStart = new Vector2[] { current.start, seg.start }.OrderBy(x => Vector2.SqrMagnitude(midpoint - x)).FirstOrDefault();
                            float currentLength = Vector2.SqrMagnitude(current.end - current.start);
                            float posibleLength = Vector2.SqrMagnitude(furtherEnd - furtherStart);
                            if (posibleLength > currentLength)
                            {
                                current = new Segmento(furtherStart, furtherEnd);
                                interSegment.Clear();
                                break;
                            }
                        }
                        else
                        {
                            float currentLength = Vector2.SqrMagnitude(current.end - current.start);
                            float posibleLength = Vector2.SqrMagnitude(furtherEnd - current.start);
                            if (posibleLength > currentLength)
                            {
                                current = new Segmento(current.start, furtherEnd);
                                interSegment.Clear();
                                break;
                            }
                        }
                    }
                }

                if (TryIntersect(current.start, current.end, seg.start, seg.end, out Vector2 p))
                {
                    if (Vector2.SqrMagnitude(current.start - p) < 1e-6f)
                    {
                        continue;
                    }
                    interSegment.Add(new Segmento(p, seg.end));
                }
            }
            interSegment = interSegment.OrderBy(p => p, Comparer<Segmento>.Create((a, b) => Trigonometrics.CompareByTurnAndDistance(a, b, current))).ToList();

            if (interSegment.Count == 0)
            {
                Debug.Break();
            }
            else
            {
                segmentChain.Add(new Segmento(current.start, interSegment[0].start));
                current = interSegment[0];
            }
        }


        return segmentChain.ToArray();
    }

    Vector2[] CalcularUnion(List<BaseObstacle> grupo)
    {
        List<Segmento> rawSegments = new List<Segmento>();

        foreach (var obs in grupo)
        {
            var points = GetWorldPoints(obs);
            for (int i = 0; i < points.Length; i++)
            {
                rawSegments.Add(new Segmento(points[i], points[(i + 1) % points.Length]));
            }
        }
        //search for a segment whom start isnt inside of any of the group obs geometries
        Segmento startSegment = rawSegments.FirstOrDefault(seg => !grupo.Any(obs => PointInPolygon(seg.start, GetWorldPoints(obs))));
        Segmento[] perimeter = GetSegmentChain(startSegment, grupo);

        return perimeter.Select(seg => seg.start).ToArray();

    }

}
