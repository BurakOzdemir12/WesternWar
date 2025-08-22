using System;
using TMPro;
using UnityEngine;


public class Barrel : MonoBehaviour, IHittableProp
{
    // const string playerString = "Player";
    public static event Action OnBreakBarrelMan;
    public static event Action<WeaponsSo> OnBreakBarrelWeapon;

    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private Transform barrelModel;

    [Header("Bonus Settings")] [SerializeField]
    private WeaponType rewardWeapon;

    [SerializeField] private RewardType rewardType;
    [SerializeField] private WeaponsSo weapon;
    [SerializeField] private int barrelBonusValue;

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
        switch (rewardType)
        {
            case RewardType.Man:
                barrelBonusValue = 3;
                break;
            case RewardType.Weapon:
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
        switch (rewardType)
        {
            case RewardType.Man:
                OnBreakBarrelMan?.Invoke();
                break;
            case RewardType.Weapon:
                OnBreakBarrelWeapon?.Invoke(weapon);
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // if (other.gameObject.CompareTag("Bullet"))
        // {
        // }
    }
}