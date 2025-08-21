using System;
using TMPro;
using UnityEngine;

public enum BarrelBonusType
{
    Gun,
    Man,
    HorseMan
}
public class Barrel : MonoBehaviour, IHittableProp
{
    // const string playerString = "Player";

    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private Transform barrelModel;
    [Header("Bonus Settings")]
    [SerializeField] private BarrelBonusType barrelBonusType;
    
    [Header("Counter Settings")] [SerializeField]
    private int counterValue;

    [SerializeField] private TextMeshProUGUI counterText;

    void Start()
    {
        if (counterText != null)
        {
            counterText.text = counterValue.ToString();
        }
        SetBonusType();
    }

    void Update()
    {
        HandleDestroy();
        RotateObject();
        // UpdateCounterValue();
    }

    public void OnHit()
    {
        if (counterValue > 0)
        {
            TextPulseEffect.instance?.TriggerTextEffect(counterText);
            counterValue--;
            UpdateCounterValue();
        }
    }

    private void UpdateCounterValue()
    {
        if (counterText != null)
        {
            counterText.text = counterValue.ToString();
        }
    }

    private void RotateObject()
    {
        if (barrelModel != null)
        {
            barrelModel.Rotate(0f, (rotationSpeed * Time.deltaTime), 0f);
        }
    }

    private void SetBonusType()
    {
        switch (barrelBonusType)
        {
            case BarrelBonusType.Man:
                
                break;
        }
    }

    private void HandleDestroy()
    {
        if (counterValue <= 0)
        {
            HandleBonus();
            Destroy(gameObject);
        }
    }

    private void HandleBonus()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // if (other.gameObject.CompareTag("Bullet"))
        // {
        // }
    }
}