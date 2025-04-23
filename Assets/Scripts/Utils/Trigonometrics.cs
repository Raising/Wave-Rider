using System.CodeDom;
using UnityEngine;

public struct Segmento
{
    public Vector2 start;
    public Vector2 end;

    public Segmento(Vector2 a, Vector2 b)
    {
        start = a;
        end = b;
    }

    public bool EqualsReversed(Segmento other)
    {
        return start == other.end && end == other.start;
    }
    public bool IsParallel(Segmento other)
    {
        Vector2 dir1 = end - start;
        Vector2 dir2 = other.end - other.start;

        float cross = dir1.x * dir2.y - dir1.y * dir2.x;
        return Mathf.Abs(cross) < 1e-6f;
    }

}
public static class Trigonometrics
{
    // Angle counts from twelve o'clock and clock wise
    public static Vector2 GetCircunferencePoint(Vector2 center, float radius, float angleRad)
    {
        return new Vector2(center.x + (radius * Mathf.Sin(angleRad)), center.y + (radius * Mathf.Cos(angleRad)));
    }

    public static Vector2 localPointToGlobal(GameObject referent, Vector2 point)
    {

        return referent.transform.TransformPoint(point);
    }

    public static Vector2 globalPointToLocal(GameObject referent, Vector2 point)
    {
        return referent.transform.InverseTransformPoint(point);

    }

    public static Vector2 getPosition2D(GameObject referent)
    {
        return new Vector2(referent.transform.position.x, referent.transform.position.y);
    }

    public static Vector2 rotatePointFromCenter(Vector2 point, float angle)
    {
        float newX = Mathf.Cos(angle * Mathf.Deg2Rad) * (point.x) - Mathf.Sin(angle * Mathf.Deg2Rad) * (point.y);
        float newY = Mathf.Sin(angle * Mathf.Deg2Rad) * (point.x) + Mathf.Cos(angle * Mathf.Deg2Rad) * (point.y);

        return new Vector2(newX, newY);

    }

    public static Vector2 linesIntersection(Vector2 point1, Vector2 vector1, Vector2 point2, Vector2 vector2)
    {

        float A1 = vector1.y;
        float B1 = -1 * vector1.x;
        float C1 = point1.x * A1 + point1.y * B1;

        float A2 = vector2.y;
        float B2 = -1 * vector2.x;
        float C2 = point2.x * A2 + point2.y * B2;

        float delta = A1 * B2 - A2 * B1;

        float x = (B2 * C1 - B1 * C2) / delta;
        float y = (A1 * C2 - A2 * C1) / delta;

        return new Vector2(x, y);
    }

    public static bool pointIsInSemiSegment(Vector2 segmentStartPoint, Vector2 segmentDirection, Vector2 checkedPoint)
    {
        if (segmentDirection.x > 0 && segmentStartPoint.x < checkedPoint.x)
        {
            return true;
        }
        else if (segmentDirection.x < 0 && segmentStartPoint.x > checkedPoint.x)
        {
            return true;
        }
        else if (segmentDirection.y > 0 && segmentStartPoint.y < checkedPoint.y)
        {
            return true;
        }
        else if (segmentDirection.y < 0 && segmentStartPoint.y > checkedPoint.y)
        {
            return true;
        }
        return false;
    }

    public static bool pointIsInSegment(Vector2 segmentStartPoint, Vector2 segmentEndPoint, Vector2 checkedPoint)
    {
        float minY = Mathf.Min(segmentStartPoint.y, segmentEndPoint.y);
        float maxY = Mathf.Max(segmentStartPoint.y, segmentEndPoint.y);
        float minX = Mathf.Min(segmentStartPoint.x, segmentEndPoint.x);
        float maxX = Mathf.Max(segmentStartPoint.x, segmentEndPoint.x);

        if ((checkedPoint.x < maxX && checkedPoint.x > minX) || (checkedPoint.y > minY && checkedPoint.y < maxY))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool areParallels(Vector2 vectorA, Vector2 vectorB)
    {
        Vector2 normalicedA = vectorA.normalized;
        Vector2 normalicedB = vectorB.normalized;

        if (normalicedA == normalicedB || normalicedA == -1 * normalicedB)
        {
            return true;
        }
        return false;
    }

    public static int CompareByTurnAndDistance(Segmento a, Segmento b, Segmento current)
    {
        Vector2 dirCurrent = (current.end - current.start).normalized;
        Vector2 dirA = (a.end - a.start).normalized;
        Vector2 dirB = (b.end - b.start).normalized;

        float angleA = Vector2.SignedAngle(dirCurrent, dirA);
        float angleB = Vector2.SignedAngle(dirCurrent, dirB);

        bool leftA = angleA > 0;
        bool leftB = angleB > 0;

        if (leftA && !leftB) return -1;
        if (!leftA && leftB) return 1;

        float distA = Vector2.SqrMagnitude(a.start - current.start);
        float distB = Vector2.SqrMagnitude(b.start - current.start);

        if (leftA) // giro a la izquierda: más cercano primero
            return distA.CompareTo(distB);
        else       // giro a la derecha: más lejano primero
            return distB.CompareTo(distA);
    }
    //return negative if the points are clockwise
    public static float SignedArea(Vector2[] poly)
    {
        float area = 0f;
        for (int i = 0; i < poly.Length; i++)
        {
            Vector2 p0 = poly[i];
            Vector2 p1 = poly[(i + 1) % poly.Length];
            area += (p0.x * p1.y - p1.x * p0.y);
        }
        return area * 0.5f;
    }
    // Returns false if sinple colision, true if they are tooo parallel ( 15 degrees aprox)
    public static bool LineIntersection(Vector2 p1, Vector2 p2, Vector2 q1, Vector2 q2, float inset, out Vector3 intersection)
    {
        Vector2 r = (p2 - p1).normalized;
        Vector2 s = (q2 - q1).normalized;

        float rxs = r.x * s.y - r.y * s.x;
        float angle = Vector2.SignedAngle(r, s);
        if (angle > 0 && angle > 135f)
        {
            Vector2 bisectriz = (r - s).normalized;
            // Líneas casi paralelas — devolvemos punto promedio como aproximación robusta
            intersection = (p2 + q1) * 0.5f + inset * bisectriz * 0.66f;
            return true;
        }

        // Nota: este cálculo de t es válido para líneas infinitas
        float t = ((q1.x - p1.x) * s.y - (q1.y - p1.y) * s.x) / rxs;
        intersection = p1 + t * r;
        return false;
    }
}