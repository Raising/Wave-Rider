using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasInteractionArea : MonoBehaviour
{
    // Start is called before the first frame update
    //public GameObject TraslationGizmoPrefab;
    //public GameObject RotationGizmoPrefab;
    //public GameObject ScaleGizmoPrefab;
    public GameObject SelectionGizmoPrefab;
    public GameObject ElementGizmoPrefab;
    public GameObject PointGizmoPrefab;

    private List<GameObject> gizmoList = new List<GameObject>();
    private RectTransform rt;
    private GameObject latestIndividualSelection;
    void Start()
    {
        rt = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CleanGizmos()
    {
        foreach (GameObject item in gizmoList)
        {
            Destroy(item);
        }
        gizmoList.Clear();
    }


    public void ShowGizmo(GameObject go)
    {
        CleanGizmos();
        GameObject gizmo = Instantiate(ElementGizmoPrefab, this.transform);
        gizmo.GetComponent<TransformGizmoController>().SetTarget(go, rt);
        gizmoList.Add(gizmo);
        latestIndividualSelection = go;
    }

    public void ChangeToTransformGizmo()
    {
        if (latestIndividualSelection != null)
        {
            CleanGizmos();
            GameObject gizmo = Instantiate(ElementGizmoPrefab, this.transform);

            gizmo.GetComponent<TransformGizmoController>().SetTarget(latestIndividualSelection, rt);
            gizmoList.Add(gizmo);
        }

    }
    public void ChangeToPointsGizmo()
    {
        if (latestIndividualSelection != null)
        {
            CleanGizmos();
            GameObject gizmo = Instantiate(PointGizmoPrefab, this.transform);

            gizmo.GetComponent<PointGizmoController>().SetTarget(latestIndividualSelection, rt);
            gizmoList.Add(gizmo);
        }

    }



    public void ShowSelectGizmo(List<GameObject> gos)
    {
        CleanGizmos();
        latestIndividualSelection = null;
        foreach (GameObject go in gos)
        {
            GameObject gizmo = Instantiate(SelectionGizmoPrefab, this.transform);
            gizmo.GetComponent<SelectionGizmo>().SetTarget(go, rt, (go) => { ShowGizmo(go); });
            gizmoList.Add(gizmo);
        }
    }

    //public void ShowTraslationGizmo(GameObject go)
    //{
    //    CleanGizmos();
    //    GameObject gizmo = Instantiate(TraslationGizmoPrefab, this.transform);
    //    gizmo.GetComponent<TraslationGizmo>().SetReferencedObject(go, this.gameObject);
    //    gizmoList.Add(gizmo);
    //}
    //public void ShowScaleGizmo(GameObject go)
    //{
    //    CleanGizmos();
    //    GameObject gizmo = Instantiate(ScaleGizmoPrefab, this.transform);
    //    gizmo.GetComponent<ScaleGizmo>().SetReferencedObject(go, this.gameObject);
    //    gizmoList.Add(gizmo);
    //}
}
