using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VisualFeedBack : MonoBehaviour
{
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private Material material;
    private static GameObject visualFeedBackPrefab;

    public static VisualFeedBack Instantiante()
    {
        if (visualFeedBackPrefab == null) {
            visualFeedBackPrefab = Resources.Load("Utils/VisualFeedBack") as GameObject;
        }
        return Instantiate(visualFeedBackPrefab, new Vector3(0, 0.001f, 0), Quaternion.identity).GetComponent<VisualFeedBack>();
    }

    private void Awake()
    {

        meshFilter = FindObjectOfType<MeshFilter>();
        meshRenderer = FindObjectOfType<MeshRenderer>();
        material = meshRenderer.material;
    }

    public void SetAnchor(Transform anchor)
    {
        transform.parent = anchor;
        transform.localPosition = new Vector3(0, 0.001f, 0);
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;

        material.color = new Color(UnityEngine.Random.Range(0.0f,1.0f)  , UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f), 0.5f);
    }

    public void SetColor(Color newColor)
    {
        material.color = newColor;
    }

    public void SetAsSector(Vector3 center, float minAngle, float maxAngle, float minDist, float maxDist)
    {
        Mesh sector = MeshCreator.Sector(center, minAngle, maxAngle, minDist, maxDist);
        meshFilter.mesh = sector;
    }

    public void SetMesh(Mesh mesh)
    {
       meshFilter.mesh = mesh;
    }

    public void Show()
    {
        meshRenderer.enabled = true;  
    } 
    
    public void Hide()
    {
        meshRenderer.enabled = false;
    }

    public bool IsVisible()
    {
        return meshRenderer.enabled;
    }
}