using LineworkLite.FreeOutline;
using UnityEngine;

public class OutlinePulseEffect : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float amount;
     private float _currentAmount;

    [Tooltip("Outline settings for the pulsing effect, if applicable.")] [SerializeField]
    private FreeOutlineSettings outlineSettings;

    void Start()
    {
        if (outlineSettings == null) return;
         _currentAmount= outlineSettings.Outlines[0].width;
    }

    // Update is called once per frame
    void Update()
    {
        if (!outlineSettings) return;
        outlineSettings.Outlines[0].width = Mathf.Abs(Mathf.Sin(Time.time * speed) * amount);
    }
}