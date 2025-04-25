using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CameraMode
{
    Static,
    Follow,
    Moving,
}
public class CameraController : MonoBehaviour
{
    private Camera cam;
    private float baseSize;
    private float bumpAmount = 0f;
    private Coroutine pushRoutine;
    private Coroutine pullRoutine;

    void Start()
    {
        cam = GetComponent<Camera>();
        baseSize = cam.orthographicSize;
        EventManager.StartListening("OnWaterPush", (hitPoint) => CameraPush());
        EventManager.StartListening("OnWaterRelease", (hitPoint) => CameraPull());
    }

    void CameraPush()
    {
        bumpAmount += baseSize * 0.008f;

        if (pushRoutine != null)
            StopCoroutine(pushRoutine);

        pushRoutine = StartCoroutine(PushCoroutine());
    }

    void CameraPull()
    {
        if (pullRoutine != null)
            StopCoroutine(pullRoutine);

        pullRoutine = StartCoroutine(PullCoroutine());
    }

    IEnumerator PushCoroutine()
    {
        float duration = 0.05f;
        float startSize = cam.orthographicSize;
        float targetSize = baseSize + bumpAmount;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            cam.orthographicSize = Mathf.Lerp(startSize, targetSize, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        cam.orthographicSize = targetSize;
    }

    IEnumerator PullCoroutine()
    {
        float duration = 0.05f;
        float startSize = cam.orthographicSize;
        float targetSize = baseSize;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            cam.orthographicSize = Mathf.Lerp(startSize, targetSize, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        cam.orthographicSize = targetSize;

        bumpAmount = 0f; // Reset bump when release is complete
    }

}
