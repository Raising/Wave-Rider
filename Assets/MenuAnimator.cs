using UnityEngine;
using System.Collections;

public class MenuAnimator : MonoBehaviour
{
    private float animationDuration = 0.35f; // duración de la animación en segundos
    public Vector2 targetPosition = new Vector2();
    private Vector2 basePosition = new Vector2(-800, 0); // qué tan fuera de pantalla empieza (ajusta según tu canvas)
    private Vector2 currentstartPosition = new Vector2();
    private RectTransform rt;
    private bool open = false;
    private void Start()
    {
        rt = GetComponent<RectTransform>();
        basePosition = rt.anchoredPosition;
    }

    public void ToggleMenu()
    {
        if (open)
        {
            CloseMenu();
        }
        else
        {
            OpenMenu();
        }
    }
    public void OpenMenu()
    {
        open = true;
        currentstartPosition = rt.anchoredPosition;
        StartCoroutine(AnimateToTarget());
    }

    public void CloseMenu()
    {
        open = false;
        currentstartPosition = rt.anchoredPosition;
        StartCoroutine(AnimateToBase());
    }

    private IEnumerator AnimateToTarget()
    {
        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = 1 - Mathf.Clamp01(elapsed / animationDuration);
            float easedT = 1 - t * t * t;
            rt.anchoredPosition = Vector2.Lerp(currentstartPosition, targetPosition, easedT);
            yield return null;
        }

        rt.anchoredPosition = targetPosition;
    }

    private IEnumerator AnimateToBase()
    {
        RectTransform rt = GetComponent<RectTransform>();

        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / animationDuration);
            rt.anchoredPosition = Vector2.Lerp(currentstartPosition, basePosition, t);
            yield return null;
        }

        rt.anchoredPosition = basePosition;
    }
}
