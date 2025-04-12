using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CreationLevelScreen : MonoBehaviour
{
    private LevelMiniature selectedLevel;
    public GameObject levelMiniturePrefab;
    private LevelMiniature[] miniatures = new LevelMiniature[0];

    public GameObject PlayButton;
    public GameObject EditButton;
    public GameObject PublishButton;
    public GameObject DeleteButton;
    // Start is called before the first frame update
    void Start()
    {
        LevelData[] levels = SaveManager.Instance.Data.createdLevelList;
        foreach (LevelData level in levels)
        {
            GameObject go = Instantiate(levelMiniturePrefab, transform);
            LevelMiniature miniature = go.GetComponent<LevelMiniature>();
            miniature.AssignLevel(level);
            miniature.ConnectoToManager(this);
            miniatures = miniatures.Append(miniature).ToArray();
        }

        DeactivateButton(PlayButton);
        DeactivateButton(EditButton);
        DeactivateButton(PublishButton);
        DeactivateButton(DeleteButton);
    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void SetSelection(LevelMiniature levelMiniature)
    {
        selectedLevel = levelMiniature;
        foreach (LevelMiniature miniature in miniatures)
        {
            if (miniature != levelMiniature)
            {
                miniature.SetSelect(false);
            }
        }
        ActivateButton(PlayButton);
        ActivateButton(EditButton);
        ActivateButton(PublishButton);
        ActivateButton(DeleteButton);
    }

    private void ActivateButton(GameObject go)
    {
        go.GetComponent<Button>().enabled = true;
        go.GetComponentInChildren<Text>().enabled = true;
    }
    private void DeactivateButton(GameObject go)
    {
        go.GetComponent<Button>().enabled = false;
        go.GetComponentInChildren<Text>().enabled = false;
    }

    public void DeleteSelectedLevel()
    {
        if (selectedLevel != null)
        {
            SaveManager.DeleteLevelData(selectedLevel.levelData);
            SaveManager.SaveGame();
            Destroy(selectedLevel.gameObject);
            selectedLevel = null;
            DeactivateButton(PlayButton);
            DeactivateButton(EditButton);
            DeactivateButton(PublishButton);
            DeactivateButton(DeleteButton);
        }
    }

    public void EditSelectedLevel()
    {
        if (selectedLevel != null)
        {
            GameManager.SetTargetLevel(selectedLevel.levelData.levelId);
            GameManager.Instance.LoadScene("LevelEditor");
        }
    }

    public void PlaySelectedLevel()
    {
        if (selectedLevel != null)
        {
            GameManager.SetTargetLevel(selectedLevel.levelData.levelId);
            GameManager.Instance.LoadScene("LevelLoader");
        }
    }

    public void CreateNewLevel()
    {

        GameManager.SetTargetLevel(string.Empty);
        GameManager.Instance.LoadScene("LevelEditor");

    }
    public void GoBack()
    {

        GameManager.SetTargetLevel(string.Empty);
        GameManager.Instance.LoadScene("SeleccionActividad");

    }

}



