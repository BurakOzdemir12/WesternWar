using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public enum BonusType
{
    Addition,

    // Difference,
    Subtract,
    Multiply,
    Divide
}

public class Doors : MonoBehaviour, IHittableProp
{
    [Header("Door Elements")] [SerializeField]
    private SpriteRenderer doorRenderer;

    // [SerializeField] private SpriteRenderer leftDoorRenderer;
    [Space] [SerializeField] private TextMeshProUGUI doorText;

    // [SerializeField] private TextMeshProUGUI leftDoorText;
    private Collider _doorsCollider;

    [Header("Bonus Settings")] [SerializeField]
    private BonusType doorBonusType;

    [SerializeField] private int doorValue;
    // [Space] [SerializeField] private BonusType leftDoorBonusType;
    // [SerializeField] private int leftDoorBonusValue;

    [Space] [Tooltip("The color of the right door when the player chooses it")] [SerializeField]
    private Color bonusColor;

    [SerializeField] private Color penaltyColor;

    // [Range(1, 200)] [SerializeField] private int minusDoorValues;
    // [Range(1, 200)] [SerializeField] private int plusDoorValues;
    // [Range(1, 200)] [SerializeField] private int divideDoorValues;
    // [Range(1, 200)] [SerializeField] private int xDoorValues;
    private void Awake()
    {
        _doorsCollider = GetComponent<Collider>();
    }

    void Start()
    {
        SetBonusType();
    }

    private void SetBonusType()
    {
        doorBonusType = doorValue switch
        {
            >= 0 => BonusType.Addition,
            < 0 => BonusType.Subtract
        };

        switch (doorBonusType)
        {
            case BonusType.Addition:
                doorRenderer.color = bonusColor;
                doorText.text = $"+{doorValue}";
                break;
            // case BonusType.Difference:
            case BonusType.Subtract:
                doorRenderer.color = penaltyColor;
                doorText.text = $"{doorValue}";
                break;
            case BonusType.Multiply:
                doorRenderer.color = bonusColor;
                doorText.text = $"x{doorValue}";
                break;
            case BonusType.Divide:
                doorRenderer.color = penaltyColor;
                doorText.text = $"/{doorValue}";
                break;
        }
    }

    public void SetPenaltyByCrowdSize(int crowdSize)
    {
        if (crowdSize <= 3)
        {
            doorValue = Random.Range(-3, 2);
        }
        else
        {
            int min = Mathf.CeilToInt(crowdSize * 0.2f);
            int max = Mathf.CeilToInt(crowdSize * 0.5f);
            doorValue = Random.Range(-min, -max + 1);
        }

        if (doorValue > 0)
        {
            doorBonusType = BonusType.Addition;
        }

        if (doorValue < 0)
        {
            doorBonusType = BonusType.Subtract;
        }
    }

    public int GetBonusAmount(float positionX)
    {
        return doorValue;
    }

    public BonusType GetBonusType(float positionX)
    {
        return doorBonusType;
    }

    public void DisableDoor()
    {
        _doorsCollider.enabled = false;
    }

    public void OnHit()
    {
            TextPulseEffect.instance?.TriggerTextEffect(doorText);
            doorValue++;
            UpdateCounterValue();
            SetBonusType();
    }

    private void UpdateCounterValue()
    {
        doorText.text = doorValue.ToString();
    }
}