using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RotateLoadingImage : MonoBehaviour
{
    [SerializeField] private Image radialImage;
    private float fillDuration = 2.0f;
    private float currentFill;
    private Coroutine fillCoroutine;

    void OnEnable()
    {
        StartFillCoroutine();
    }

    void OnDisable()
    {
        if (fillCoroutine != null)
        {
            StopCoroutine(fillCoroutine);
        }
    }

    void StartFillCoroutine()
    {
        fillCoroutine = StartCoroutine(FillRadialOverTime());
    }

    IEnumerator FillRadialOverTime()
    {
        float timer = 0f;

        while (timer < fillDuration)
        {
            timer += Time.deltaTime;
            currentFill = Mathf.Lerp(0f, 1f, timer / fillDuration);
            radialImage.fillAmount = currentFill;
            yield return null;
        }
        radialImage.fillAmount = 1f;
        fillCoroutine = StartCoroutine(FillRadialOverTime());
    }
}