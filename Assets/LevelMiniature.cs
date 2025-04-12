using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class LevelMiniature : MonoBehaviour, IPointerClickHandler
{
    public Text text;
    public Shapes.Rectangle SelectedFrame;
    public LevelData levelData;
    private CreationLevelScreen manager;
    private bool isSelected = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        manager.SetSelection(this);
        SetSelect(true);
    }

    public void SetSelect(bool isSelected)
    {
        this.isSelected = isSelected;
        SelectedFrame.enabled = isSelected;
    }

    public void AssignLevel(LevelData levelData)
    {
        this.levelData = levelData;
        text.text = levelData.name;
    }

    public void ConnectoToManager(CreationLevelScreen manager)
    {
        this.manager = manager;

    }

}
