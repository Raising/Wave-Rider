using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public enum ElementType
{
    obstacle,
    block,
    ball,
    exit,
    flow,
}
public class LevelEditorManager : MonoBehaviour
{
    public GameObject InteractionHolder;
    public GameObject obstaclePrefab;
    public GameObject surfacePrefab;
    public GameObject ballPrefab;
    public GameObject exitPrefab;
    public GameObject flowPrefab;
    private GameObject selectedElement;
    public GameObject CanvasInteractionArea;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void OpenMenu(GameObject go)
    {
        go.GetComponent<MenuAnimator>().ToggleMenu();
    }

    public void CreateObstacle()
    {
        GameObject go =Instantiate(obstaclePrefab, Vector3.zero, Quaternion.identity,InteractionHolder.transform);
        selectedElement = go ;
        ObstacleCollider obstacleCollider = go.GetComponent<ObstacleCollider>();
        CanvasInteractionArea.GetComponent<CanvasInteractionArea>().ShowTraslationGizmo(go,CanvasInteractionArea);
    }
    public void CreateSurface()
    {
        GameObject go = Instantiate(surfacePrefab, Vector3.zero, Quaternion.identity, InteractionHolder.transform);
        selectedElement = go;
    }
    public void CreateBall()
    {
        GameObject go = Instantiate(ballPrefab, Vector3.zero, Quaternion.identity, InteractionHolder.transform);
        selectedElement = go;
    }
    public void CreateFlow()
    {
        GameObject go = Instantiate(flowPrefab, Vector3.zero, Quaternion.identity, InteractionHolder.transform);
        selectedElement = go;
    }
    public void CreateExit()
    {
        GameObject go = Instantiate(exitPrefab, Vector3.zero, Quaternion.identity, InteractionHolder.transform);
        selectedElement = go;
    }
}
