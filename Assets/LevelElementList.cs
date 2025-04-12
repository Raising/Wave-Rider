using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelElementList : MonoBehaviour
{
    public MetaLevel metalevel;
    public GameObject UIButonsContainer;
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject prefab in metalevel.elementPrefabs)
        {
            LevelElementBase element = prefab.GetComponent<LevelElementBase>(); 
            GameObject go = Instantiate(element.UIPrefab, Vector3.zero, Quaternion.identity, UIButonsContainer.transform);
            UIElementCreator elementCreator = go.AddComponent<UIElementCreator>();
            elementCreator.elementPrefab = prefab;
            elementCreator.canvasArea = GetComponent<RectTransform>();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
