using System;
using UnityEngine;

public struct PolarPoint {
    public float polar;
    public float distance;

    public PolarPoint(float polar, float distance)
    {
        this.polar = polar;
        this.distance = distance;
    }

    public override string ToString()
    {
        return "Polar: " + polar + " Degrees.  Distance: " + distance;
    }
}


public static class Polar
{
    public static PolarPoint CartesianToPolar(Vector2 point, Vector2 center = new Vector2())
    {
        Vector2 localPointCoordinates = new Vector2(point.x - center.x, point.y - center.y);
        float angle = Mathf.Atan2(localPointCoordinates.x, localPointCoordinates.y) * Mathf.Rad2Deg;
        if (angle <0)
        {
            angle += 360;
        }
        return new PolarPoint(angle, localPointCoordinates.magnitude);
    }

    public static PolarPoint CartesianToPolar(Vector2 point, Vector3 center = new Vector3())
    {
        Vector2 localPointCoordinates = new Vector2(point.x - center.x, point.y - center.z);
        return Polar.CartesianToPolar(localPointCoordinates, new Vector2());
    }

    public static PolarPoint CartesianToPolar(Vector3 point, Vector3 center = new Vector3())
    {
        Vector2 localPointCoordinates = new Vector2(point.x - center.x, point.z - center.z);
        return Polar.CartesianToPolar(localPointCoordinates,new Vector2());
       // return new PolarPoint(Mathf.Atan2(localPointCoordinates.x, localPointCoordinates.y) * Mathf.Rad2Deg, localPointCoordinates.magnitude);
    }

    public static PolarPoint CartesianToPolar(Vector3 point, Vector2 center = new Vector2())
    {
        Vector2 localPointCoordinates = new Vector2(point.x - center.x, point.z - center.y);
        return Polar.CartesianToPolar(localPointCoordinates, new Vector2());
    }

    public static Vector2 PolarToCartesian(PolarPoint polarPoint, Vector2 center = new Vector2())
    {
        return new Vector2(Mathf.Sin(polarPoint.polar * Mathf.Deg2Rad) * polarPoint.distance, Mathf.Cos(polarPoint.polar * Mathf.Deg2Rad) * polarPoint.distance) + center;
    }

    public static Vector3 PolarToCartesian(PolarPoint polarPoint, Vector3 center = new Vector3())
    {
        return new Vector3(Mathf.Sin(polarPoint.polar * Mathf.Deg2Rad) * polarPoint.distance,0,Mathf.Cos(polarPoint.polar * Mathf.Deg2Rad) * polarPoint.distance) + center;
    }
}

