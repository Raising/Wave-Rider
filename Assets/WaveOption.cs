using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveOption : MonoBehaviour
{
    public GameObject WavePrefab;
    // Start is called before the first frame update
    void Start()
    {
        VerificableAction unListenAction = EventManager.StartListening("WATERAREA:Touch", (hitPoint) => OnPlacement((Vector3)hitPoint));
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnPlacement(Vector3 point)
    {
        GameObject generator = Instantiate(WavePrefab, new Vector3(point.x, point.y, 0), Quaternion.identity) as GameObject;
    }
}
