using LineworkLite.FreeOutline;
using UnityEngine;

public class PulseEffect : MonoBehaviour
{
    [SerializeField] private GameObject obj;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float amount = 0.2f;

   
    private Vector3 originalScale;

    void Start()
    {
        if (obj == null) obj = gameObject;
        originalScale = obj.transform.localScale;
    }

    void Update()
    {
        if (!obj) return;

        float scale = 1f + Mathf.Sin(Time.time * speed) * amount;
        obj.transform.localScale = originalScale * scale;

          }
}