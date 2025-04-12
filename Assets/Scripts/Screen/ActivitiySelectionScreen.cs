using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivitiySelectionScreen : MonoBehaviour
{
    public void LoadMainGame()
    {
        GameManager.Instance.LoadScene("SeleccionNivel");
    }

    public void LoadCreationMenu()
    {
        GameManager.Instance.LoadScene("CreationInterface");
    }

    public void LoadComunityMenu()
    {
        //GameManager.Instance.LoadScene("ComunityMenu");
    }
}
