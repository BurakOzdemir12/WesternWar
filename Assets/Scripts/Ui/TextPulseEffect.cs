using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TextPulseEffect : MonoBehaviour
{
    public static TextPulseEffect instance;

    private void Awake()
    {
        instance = this;
    }

    private static bool IsAlive => instance != null && instance;

    public void TriggerObjectEffect(GameObject obj)
    {
        if (!IsAlive || obj == null) return;
        instance.StartCoroutine(instance.Pulse(obj));
    }

    private IEnumerator Pulse(GameObject obj)
    {
        float duration = 0.3f;
        float timer = 0f;

        Vector3 startScale = obj.transform.localScale;
        Vector3 peakScale = Vector3.one * 1.2f;

        // Scale up
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            obj.transform.localScale = Vector3.Lerp(startScale, peakScale, t);
            yield return null;
        }

        // Scale down
        timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            obj.transform.localScale = Vector3.Lerp(peakScale, Vector3.one, t);
            yield return null;
        }
    }

    public void TriggerTextEffect(TextMeshProUGUI doorText)
    {
        if (doorText == null) return;
        StartCoroutine(PulseText(doorText));
    }

    private IEnumerator PulseText(TextMeshProUGUI text)
    {
        if (text == null) yield break;
        float duration = 0.1f;
        float timer = 0f;

        Vector3 startScale = text.rectTransform.localScale;
        Vector3 peakScale = Vector3.one * 1.2f;

        // Scale up
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            text.rectTransform.localScale = Vector3.Lerp(startScale, peakScale, t);
            yield return null;
        }

        // Scale down
        timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            text.rectTransform.localScale = Vector3.Lerp(peakScale, Vector3.one, t);
            yield return null;
        }

        text.rectTransform.localScale = Vector3.one; // tam sıfır hatası giderme
    }

    void OnDestroy()
    {
        if (instance == this) instance = null; // asılı referansı temizle
    }
}