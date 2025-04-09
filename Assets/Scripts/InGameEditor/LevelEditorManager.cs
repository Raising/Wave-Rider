using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;


public class LevelEditorManager : MonoBehaviour
{
    public GameObject InteractionHolder;
    public GameObject obstaclePrefab;
    public GameObject surfacePrefab;
    public GameObject ballPrefab;
    public GameObject exitPrefab;
    public GameObject flowPrefab;
    public LevelData levelData;
    //private GameObject 
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
        GameObject go = Instantiate(obstaclePrefab, Vector3.zero, Quaternion.identity, InteractionHolder.transform);
        
        ObstacleCollider obstacleCollider = go.GetComponent<ObstacleCollider>();
        CanvasInteractionArea.GetComponent<CanvasInteractionArea>().ShowGizmo(go);
    }
    public void CreateSurface()
    {
        GameObject go = Instantiate(surfacePrefab, Vector3.zero, Quaternion.identity, InteractionHolder.transform);
        
    }
    public void CreateBall()
    {
        GameObject go = Instantiate(ballPrefab, Vector3.zero, Quaternion.identity, InteractionHolder.transform);
        
    }
    public void CreateFlow()
    {
        GameObject go = Instantiate(flowPrefab, Vector3.zero, Quaternion.identity, InteractionHolder.transform);
        
    }
    public void CreateExit()
    {
        GameObject go = Instantiate(exitPrefab, Vector3.zero, Quaternion.identity, InteractionHolder.transform);
        
    }


    public void ShowSelection()
    {
        List<GameObject> allCurrentElements = new List<GameObject>();
        foreach (Transform child in InteractionHolder.transform)
        {
            allCurrentElements.Add(child.gameObject);

        }


        CanvasInteractionArea.GetComponent<CanvasInteractionArea>().ShowSelectGizmo(allCurrentElements);
    }

    public void SaveLevel()
    {
        List<GameObject> allCurrentElements = new List<GameObject>();
        foreach (Transform child in InteractionHolder.transform)
        {
            allCurrentElements.Add(child.gameObject);

        }

        LevelData levelData = InteractionHolder.GetComponent<MetaLevel>().PortLevelToData();
        SaveManager.SaveLevelData(levelData);
        SaveManager.SaveGame();
    }


    public void Undo()
    {

    }
}
