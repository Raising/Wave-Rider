using UnityEngine;

public static class PolygonUtils
{
    /// <summary>
    /// Finds the closest point outside the PolygonCollider2D from a given point inside.
    /// </summary>
    public static Vector2 GetClosestExitPoint(PolygonCollider2D polygon, Vector2 insidePoint)
    {
        if (polygon == null) return insidePoint;

        Vector2 closestEdgeNormal = Vector2.zero;
        Vector2 closestEdgePoint = Vector2.zero;
        float minDistance = float.MaxValue;

        // Iterate over each edge of the PolygonCollider2D
        for (int i = 0; i < polygon.points.Length; i++)
        {
            // Get the two points forming an edge
            Vector2 worldPointA = polygon.transform.TransformPoint(polygon.points[i]);
            Vector2 worldPointB = polygon.transform.TransformPoint(polygon.points[(i + 1) % polygon.points.Length]);

            // Find the closest point on this edge to the given point
            Vector2 closestPoint = GetClosestPointOnLineSegment(worldPointA, worldPointB, insidePoint);
            float distance = Vector2.Distance(insidePoint, closestPoint);

            // Check if this is the closest edge so far
            if (distance < minDistance)
            {
                minDistance = distance;
                closestEdgePoint = closestPoint;

                // Compute the edge normal (perpendicular vector)
               // Vector2 edgeDirection = (worldPointB - worldPointA).normalized;
               // closestEdgeNormal = new Vector2(-edgeDirection.y, edgeDirection.x); // Perpendicular to the edge
            }
        }

        // Ensure the normal points outward (by checking direction from the polygon center)
        //Vector2 polygonCenter = (Vector2)polygon.bounds.center;
        //if (Vector2.Dot(closestEdgeNormal, (insidePoint - polygonCenter)) < 0)
        //{
        //    closestEdgeNormal *= -1; // Flip normal if it's pointing inward
        //}

        // Move outward along the edge normal until we exit the collider
        Vector2 exitPoint = closestEdgePoint;
        Vector2 direction = (closestEdgePoint - insidePoint).normalized;
        float displacement = 0f;
        while (polygon.OverlapPoint(exitPoint) && displacement <= 50f)
        {
            exitPoint += direction * 0.05f;
            displacement += 0.05f;
        }

        if (!polygon.OverlapPoint(exitPoint))
        {
            exitPoint += closestEdgeNormal * 0.1f; // Ensure it's fully outside
        }

        return exitPoint;
    }

    /// <summary>
    /// Finds the closest point on a line segment [A, B] to a given point P.
    /// </summary>
    public static Vector2 GetClosestPointOnLineSegment(Vector2 A, Vector2 B, Vector2 P)
    {
        Vector2 AP = P - A;
        Vector2 AB = B - A;
        float magnitudeAB = AB.sqrMagnitude;
        float ABAPproduct = Vector2.Dot(AP, AB);
        float distance = ABAPproduct / magnitudeAB;

        if (distance < 0) return A; // Closer to A
        if (distance > 1) return B; // Closer to B
        return A + AB * distance;   // Projection on segment
    }
}
