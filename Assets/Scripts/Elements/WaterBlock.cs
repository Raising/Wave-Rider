using System.Collections;
using UnityEngine;

[System.Serializable]
public class BlockData
{
    public Vector2 size = new Vector2();
    public Vector2 position = new Vector2();
    public Vector2 scale = new Vector2();
    public float rotation = 0;
}

public class WaterBlock : MonoBehaviour
{
    public Shapes.Rectangle Rectangle;
    private Color originalColor;
    private float originalAlpha;

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

    internal BlockData AsLevelData()
    {
        return new BlockData
        {
            size = new Vector2(this.Rectangle.Width, this.Rectangle.Height),
            position = this.transform.position,
            scale = this.transform.localScale,
            rotation = this.transform.eulerAngles.z
        };
    }

    internal void LoadFromLevelData(BlockData data)
    {
        this.Rectangle.Width = data.size.x;
        this.Rectangle.Height = data.size.y;
        this.transform.position = data.position;
        this.transform.localScale = data.scale;
        this.transform.rotation = Quaternion.Euler(0, 0, data.rotation);
        PolygonCollider2D collider = GetComponent<PolygonCollider2D>();
        collider.points = new Vector2[4] {
            new Vector2(data.size.x / (-2), data.size.y / (2)),
            new Vector2(data.size.x / (2), data.size.y / (2)),
            new Vector2(data.size.x / (2), data.size.y / (-2)),
            new Vector2(data.size.x / (-2), data.size.y / (-2)), };

    }
}
