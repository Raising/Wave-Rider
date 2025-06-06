using Newtonsoft.Json;
using System.Collections;
using UnityEngine;
using UnityEngine.Purchasing.MiniJSON;

[System.Serializable]
public class NoCLickZoneData 
{
    public SerializableVector2 size = new SerializableVector2();
    public SerializableVector3 position = new SerializableVector3();
    public SerializableVector2 scale = new SerializableVector2();
    public float rotation = 0;
}

public class NoClickArea : LevelElementBase
{
    public Shapes.Rectangle Rectangle;
    private Color originalColor;
    private float originalAlpha;

    public override string Type()
    {
        return "NoClickArea";
    }

    void Start()
    {
        originalColor = Rectangle.Color; // Store the original color
        originalAlpha = originalColor.a; // Store the original alpha value
        EventManager.StartListening("WATERBLOCK:Touch", (hit) => StartCoroutine(Flash()));
    }

    IEnumerator Flash()
    {
        Debug.Log("Flash");

        float duration = 0.45f;
        float targetAlpha = 0.4f; // Target opacity (40%)
        float fadeInTime = duration * 0.3f;  // Faster rise (30% of duration)
        float fadeOutTime = duration * 0.7f; // Slower fade-out (70% of duration)
        float elapsedTime = 0f;

        Color startColor = new Color(originalColor.r, originalColor.g, originalColor.b, originalAlpha);
        Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, targetAlpha);

        // Fade in (original alpha to 40%) - Quick rise
        while (elapsedTime < fadeInTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsedTime / fadeInTime);
            Rectangle.Color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }

        elapsedTime = 0f;

        // Fade out (40% back to original alpha) - Slower degradation
        while (elapsedTime < fadeOutTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsedTime / fadeOutTime);
            Rectangle.Color = Color.Lerp(targetColor, startColor, t);
            yield return null;
        }

        Rectangle.Color = originalColor; // Restore the original color
    }



    public override ElementData AsLevelData()
    {
        return new ElementData
        {
            type = this.Type(),
            data = JsonConvert.SerializeObject(new NoCLickZoneData()
            {
                size = new SerializableVector2(this.Rectangle.Width, this.Rectangle.Height),
                position = new SerializableVector3(this.transform.position),
                scale = new SerializableVector2(this.transform.localScale),
                rotation = this.transform.eulerAngles.z
            })
        };
    }

    public override void LoadFromLevelData(ElementData elementData)
    {

        NoCLickZoneData data = JsonUtility.FromJson<NoCLickZoneData>(elementData.data);
        this.Rectangle.Width = data.size.x;
        this.Rectangle.Height = data.size.y;
        this.transform.position = data.position.ToVector3();
        this.transform.localScale = data.scale.ToVector2();
        this.transform.rotation = Quaternion.Euler(0, 0, data.rotation);
        PolygonCollider2D collider = GetComponent<PolygonCollider2D>();
        collider.points = new Vector2[4] {
            new Vector2(data.size.x / (-2), data.size.y / (2)),
            new Vector2(data.size.x / (2), data.size.y / (2)),
            new Vector2(data.size.x / (2), data.size.y / (-2)),
            new Vector2(data.size.x / (-2), data.size.y / (-2)), };
    }

    public override void SetInert()
    {
        GetComponent<PolygonCollider2D>().enabled = false;
    }
}
