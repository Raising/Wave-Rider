#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ColorPalette : MonoBehaviour
{
    // Start is called before the first frame update
    public Color background;
    public Color primaryDark;
    public Color primaryMedium;
    public Color primaryLight;
    public Color secondaryDark;
    public Color secondaryMedium;
    public Color colorText;

    public Material waveMaterial;

    public GameObject nutShellPrefab;
    public GameObject protoObstacle;
    public GameObject protoObstacle2;
    public GameObject backgroundPrefab;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RefreshColors()
    {
        waveMaterial.SetColor("_BaseColor", primaryLight);

        Shapes.Disc nutshellShape = nutShellPrefab.GetComponentInChildren<Shapes.Disc>();

        nutshellShape.ColorOuter = primaryMedium;
        nutshellShape.ColorInner = primaryDark;

        ShapesMatchCollider obstacleScript = protoObstacle.GetComponent<ShapesMatchCollider>();

        obstacleScript.polygonBody.Color = primaryDark;
        obstacleScript.baseBorderColor.Color = primaryLight;
        obstacleScript.whiteBorderColor.Color = primaryMedium;

        ShapesMatchCollider obstacleScript2 = protoObstacle2.GetComponent<ShapesMatchCollider>();

        obstacleScript2.polygonBody.Color = primaryDark;
        obstacleScript2.baseBorderColor.Color = primaryLight;
        obstacleScript2.whiteBorderColor.Color = primaryMedium;

        Shapes.Rectangle backgroundShape = backgroundPrefab.GetComponent<Shapes.Rectangle>();

        backgroundShape.Color = background;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ColorPalette))]
public class ColorPaletteEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();  // Draws the default inspector

        ColorPalette myComponent = (ColorPalette)target;

        // Create a button in the inspector
        if (GUILayout.Button("RefreshColors"))
        {
            myComponent.RefreshColors();  // Triggers the function when button is pressed
        }
    }
}
#endif