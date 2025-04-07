using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasInteractionArea : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject TraslationGizmo;
    public GameObject RotationGizmo;
    public GameObject ScaleGizmo;
    public GameObject SelectionGizmo;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowTraslationGizmo(GameObject go)
    {
        Vector3 canvasPosition = transform.TransformPoint(go.transform.position);
        GameObject gizmo = Instantiate(TraslationGizmo, this.transform);
        gizmo.transform.position = canvasPosition;
    }
}
