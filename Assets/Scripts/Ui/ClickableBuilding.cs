using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ClickableBuilding : MonoBehaviour, IPointerClickHandler
{
    [Header("Pulse")] [SerializeField] private Transform pulseTarget;
    [SerializeField] float pulseScale = 1.15f;
    [SerializeField] float pulseDuration = 0.12f;

    [Header("Events")] public UnityEvent onClick;
    private Vector3 _baseScale;
    private bool _pulsing;
    private bool _canInteract = true;

    private void Awake()
    {
        if (!pulseTarget) pulseTarget = transform;
        _baseScale = pulseTarget.localScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("CLICK: " + name);
        if (!_pulsing && _canInteract) StartCoroutine(Pulse());
    }

    private IEnumerator Pulse()
    {
        _pulsing = true;
        _canInteract = false;
        float timer = 0f;
        while (timer < pulseDuration)
        {
            timer += Time.deltaTime;
            pulseTarget.localScale = Vector3.Lerp(_baseScale, _baseScale * pulseScale, timer / pulseDuration);
            yield return null;
        }

        timer = 0f;
        while (timer < pulseDuration)
        {
            timer += Time.deltaTime;
            pulseTarget.localScale = Vector3.Lerp(_baseScale * pulseScale, _baseScale, timer / pulseDuration);
            yield return null;
        }

        pulseTarget.localScale = _baseScale;
        _pulsing = false;

        yield return new WaitForSeconds(1f);
        onClick?.Invoke();
        _canInteract = true;
    }
}