using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeshCreator
{
    static float angleResolution = 2;

    public static Mesh WaveFragment (float width, float height)
    {
        Mesh waveFramgment = new Mesh
        {
            vertices = new Vector3[] { new Vector3(width/-2,height/-2, 0) , new Vector3(0,height/2, 0) , new Vector3(width / 2 , height/-2, 0) },
            uv = new Vector2[] { new Vector2(0, 0) , new Vector2(0.5f,1) , new Vector2(1, 0) },
            triangles = new int[] {0,1,2}
        };


        return waveFramgment;
    }

    public static Mesh Water(float width,float height,int horizontalResolution,int verticalResolution)
    {
        List<Vector3> vertices = CreateGridPoints(width, height, horizontalResolution, verticalResolution);
        Mesh water = new Mesh
        {
            vertices = vertices.ToArray(),
            uv = CalculateGridUV(horizontalResolution, verticalResolution),
            triangles = CalculateGridTriangles(horizontalResolution, verticalResolution)
        };
        //water.normals = vertices.Select(vertex => new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(-0.5f, 0.5f), -1)).ToArray();
        water.normals = Enumerable.Repeat(Vector3.back, water.vertices.Length). ToArray();

        return water;
    }

    private static Vector2[] CalculateGridUV(int horizontalResolution, int verticalResolution) { 

         Vector2[] uv = new Vector2[(horizontalResolution + 1) * (verticalResolution + 1)];

        float horizontalStep = 1f / horizontalResolution;
        float verticalStep = 1f / verticalResolution;

        for (int y = 0; y <= verticalResolution; y++)
        {
            for (int x = 0; x <= horizontalResolution; x++)
            {
                uv[x + ((horizontalResolution + 1) * y)] = new Vector2(x * horizontalStep, y * verticalStep);
            }
        }
        return uv;
    }

    private static int[] CalculateGridTriangles(int horizontalResolution, int verticalResolution)
    {
        int[] triangles = new int[horizontalResolution * verticalResolution * 6];

        int triStart,
            alternateTriangleDirectionA, 
            alternateTriangleDirectionB, 
            firstRow, 
            secondRow;

        for (int y = 0; y < verticalResolution; y++)
        {
            firstRow = y * (horizontalResolution + 1);
            secondRow = (y + 1) * (horizontalResolution + 1);
            for (int x = 0; x < horizontalResolution; x++)
            {
                triStart = (x  + (y* horizontalResolution)) * 6;
                alternateTriangleDirectionA = (x + y) % 2 ;
                alternateTriangleDirectionB = (x + y + 1) % 2 ;


                triangles[triStart + 0] = firstRow + x;
                triangles[triStart + 1] = secondRow + x;
                triangles[triStart + 2] = firstRow + x + 1;

                triangles[triStart + 3 ] = firstRow + x + 1;
                triangles[triStart + 4 ] = secondRow + x;
                triangles[triStart + 5 ] = secondRow + x + 1;
            }
        }

        return triangles;
    }
    
    private static List<Vector3> CreateGridPoints(float width, float height, int horizontalResolution, int verticalResolution)
    {
        List<Vector3> points = new List<Vector3>();
        
        Vector3 firstPoint = new Vector3(-1 * width / 2, -1 * height / 2, 0);
        Vector3 horizontalPointStep = new Vector3(width/horizontalResolution, 0, 0);
        Vector3 verticalPointStep = new Vector3(0, height / verticalResolution, 0);
       
        for (int y = 0; y <= verticalResolution; y++)
        {
            for (int x = 0; x <= horizontalResolution; x++)
            {
                points.Add(firstPoint + horizontalPointStep * x + verticalPointStep * y );
            }
        }
        return points;
    }

    public static Mesh Sector(Vector3 center, float angleMin, float angleMax, float distMin, float distMax)
    {
        Mesh sector = new Mesh();
        
        List<Vector3> furtherPoints = CreateArcPoints(center, angleMin, angleMax, distMax);
        List<Vector3> closePoints = CreateArcPoints(center, angleMin, angleMax, distMin);
        
        sector.vertices = furtherPoints.Concat(closePoints).ToArray();
        sector.normals = Enumerable.Repeat(Vector3.up, sector.vertices.Length).ToArray();
        sector.triangles = CalculateTriangles(closePoints.Count);
        sector.uv = CalculateUV(closePoints.Count);

        return sector;
    }

    private static Vector2[] CalculateUV(int rowSize)
    {
        Vector2[] uv = new Vector2[rowSize * 2];

        float step = 1 / Mathf.Max(1, (rowSize -1));
        for (int i = 0; i < rowSize; i++)
        {
            uv[i] = new Vector2(i * step, 1);
            uv[i] = new Vector2(i * step, 0);
        }
        return uv;

    }

    private static int[] CalculateTriangles(int pointsPerRow)
    {
        int[] triangles = new int[(pointsPerRow - 1) * 6];

        int secondRow = pointsPerRow;
        for (int i = 0; i < pointsPerRow - 1; i++)
        {
            int triStart = i * 6;
            triangles[triStart + 0] = i;
            triangles[triStart + 1] = i + 1;
            triangles[triStart + 2] = secondRow + i;

            triangles[triStart + 3] = i + 1;
            triangles[triStart + 4] = secondRow + i + 1;
            triangles[triStart + 5] = secondRow + i;
        }
        return triangles;
    }

    private static List<Vector3> CreateArcPoints(Vector3 center, float angleMin, float angleMax, float distance)
    {
        List<Vector3> points = new List<Vector3>();
        float currentAngle = angleMin;
        Vector3 point;
        while (currentAngle < angleMax)
        {
            point = Polar.PolarToCartesian(new PolarPoint(currentAngle, distance), center);
            points.Add(point);
            currentAngle += angleResolution;
        }
        point = Polar.PolarToCartesian(new PolarPoint(angleMax, distance), center);
        points.Add(point);

        return points;
    }
}