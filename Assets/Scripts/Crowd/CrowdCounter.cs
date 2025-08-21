using TMPro;
using UnityEngine;

public class CrowdCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI crowdCounterText;
    [SerializeField] private Transform crowdParent;

    void Start()
    {
    }

    void Update()
    {
        crowdCounterText.text = crowdParent.childCount.ToString();
    }
}