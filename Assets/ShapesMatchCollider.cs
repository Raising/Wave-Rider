using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

public class ShapesMatchCollider : MonoBehaviour
{
    public PolygonCollider2D polygonCollider;
    public Shapes.Polygon polygonBody;
    public Shapes.Polygon baseBorderColor;
    public Shapes.Polygon whiteBorderColor;
    private Boolean privateDirty = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Vector2[] points = polygonCollider.points;

        polygonBody.SetPoints(points.Select(point => point - (point.normalized * 0.08f)));
        baseBorderColor.SetPoints(points.Select(point => point - (point.normalized * 0.04f)).ToArray());
        whiteBorderColor.SetPoints(points);
        if (privateDirty == false)
        {
            privateDirty = true;
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            EditorUtility.SetDirty(polygonBody);
            EditorUtility.SetDirty(baseBorderColor);
            EditorUtility.SetDirty(whiteBorderColor);
        }
    }
#endif
}
