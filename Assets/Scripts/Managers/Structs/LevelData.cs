using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[System.Serializable]
public class LevelData
{
    public string name;
    public string world;
    public string levelId;

    public float size = 7;
    public CameraMode cameraMode = CameraMode.Static;
    public int waveTarget;
    public float timeTarget;
    public ElementData[] levelElements;
}



[System.Serializable]
public class SerializableVector2
{
    public float x;
    public float y;

    public SerializableVector2() { }

    public SerializableVector2(float x,float y)
    {
        this.x = x;
        this.y = y;
    }
    public SerializableVector2(Vector2 v)
    {
        x = v.x;
        y = v.y;
    }

    public SerializableVector2(Vector3 v)
    {
        x = v.x;
        y = v.y;
    }

    public Vector2 ToVector2() => new Vector2(x, y);
}



[System.Serializable]
public class SerializableVector3
{
    public float x;
    public float y;
    public float z;

    public SerializableVector3() { }

    public SerializableVector3(Vector3 v)
    {
        x = v.x;
        y = v.y;
        z = v.z;
    }

    public SerializableVector3(Vector2 v)
    {
        x = v.x;
        y = v.y;
        z = 0;
    }

    public Vector3 ToVector3() => new Vector3(x, y, z);
}