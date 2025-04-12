using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Purchasing;


public class LevelEditorManager : MonoBehaviour
{
    public GameObject InteractionHolder;
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

    internal GameObject CreateElement(GameObject elementPrefab, PointerEventData eventData)
    {
        GameObject go = Instantiate(elementPrefab, Vector3.zero, Quaternion.identity, InteractionHolder.transform);

        CanvasInteractionArea.GetComponent<CanvasInteractionArea>().ShowGizmo(go);
        go.GetComponent<LevelElementBase>().SetInert();
        return go;
    }
}
