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
        EventManager.StartListening("OBSTACLE:Touch", (hit) => CalculateObstacleOrigin((RaycastHit2D)hit));
        EventManager.StartListening("WATERAREA:Touch", (hitPoint) => SetOrigin((Vector2)hitPoint));
        EventManager.StartListening("WATERAREA:UnTouch", (hitPoint) => ReleaseWave((Vector2)hitPoint));
    }

    void Update()
    {

    }

    void SetOrigin(Vector3 point)
    {
        Origin = new Vector3(point.x, point.y, 0);
    }

    void CalculateObstacleOrigin(RaycastHit2D hit)
    {
        Collider2D obstacleCollider = hit.collider;
        Vector2 exitpoint = PolygonUtils.GetClosestExitPoint(obstacleCollider as PolygonCollider2D, hit.point);
        float distance = (exitpoint - hit.point).magnitude;
        if (distance < 0.5f)
        {
            Vector2 direction = (exitpoint - hit.point).normalized;
            exitpoint += direction * 0.1f; // Mueve poco a poco hasta salir
            Origin = new Vector3(exitpoint.x, exitpoint.y, 0);
        }
    }


    void ReleaseWave(Vector3 point)
    {
        if (Origin != Vector3.zero)
        {
            LevelManager.StartTimer();
            GameObject generator = Instantiate(WavePrefab, Origin, Quaternion.identity) as GameObject;
            generator.GetComponent<PathWaveGenerator>().SetDireciton(new Vector2(point.x - Origin.x, point.y - Origin.y));
            Origin = Vector3.zero;
            EventManager.TriggerEvent("OnWaveCreation");
        }
    }
}
