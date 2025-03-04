using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveOption : MonoBehaviour
{
    public GameObject WavePrefab;
    private Vector3 Origin = Vector3.zero;
    private World world;
    void Start()
    {
       world = GetComponent<World>();  
       EventManager.StartListening("WATERAREA:Touch", (hitPoint) => SetOrigin((Vector3)hitPoint));
       EventManager.StartListening("WATERAREA:UnTouch", (hitPoint) => ReleaseWave((Vector3)hitPoint));
    }

    void Update()
    {
        
    }

    void SetOrigin(Vector3 point)
    {
        Origin = new Vector3(point.x, point.y, 0);
    }

    void ReleaseWave(Vector3 point)
    {
        if (Origin != Vector3.zero)
        {
            GameObject generator = Instantiate(WavePrefab, Origin, Quaternion.identity) as GameObject;
            generator.GetComponent<PathWaveGenerator>().SetDireciton(new Vector2(point.x- Origin.x, point.y- Origin.y));
            Origin = Vector3.zero;
            EventManager.TriggerEvent("OnWaveCreation");
        }
    }
}
