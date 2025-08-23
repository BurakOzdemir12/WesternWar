using System;
using System.Collections;
using TMPro;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour, IGetHit
{
    private static readonly int Dead = Animator.StringToHash("IsDead");
    public event Action<EnemyHealth> OnEnemyDeath;
    public static event Action<EnemyHealth> OnAnyDeath;
    public static event Action<EnemyHealth> OnAnySpawned;

    [SerializeField] private EnemySo enemySo;

    [SerializeField] private Animator animator;
    [SerializeField] private Collider enemyCollider;

    [Header("UI Settings")] [SerializeField]
    private int maxHealth;

    [SerializeField] private Image healthBarSprite;
    [SerializeField] private Canvas healthBarCanvas;
    [Space] [SerializeField] private float deathAnimDuration;
    [SerializeField] private BehaviorGraphAgent behaviorGraphAgent;
    [SerializeField] private string isDeadVarName = "IsDead";

    private float _currentHealth;


    private void Awake()
    {
        maxHealth = enemySo.health;
        _currentHealth = maxHealth;
        UpdateUI();
    }

    private void LateUpdate()
    {
        healthBarCanvas.transform.rotation =
            Quaternion.LookRotation(transform.position - Camera.main.transform.position);
    }

    public void GetHit(DamageInfo info)
    {
        if (IsDead) return;
        // info.attacker.CompareTag("Player");
        _currentHealth -= Mathf.RoundToInt(info.damageAmount);
        UpdateUI();
        if (IsDead) StartCoroutine(HandleDeath());
    }

    private void UpdateUI()
    {
        if (!healthBarSprite) return;
        healthBarSprite.fillAmount = _currentHealth / maxHealth;
    }

    private IEnumerator HandleDeath()
    {
        OnEnemyDeath?.Invoke(this);
        OnAnyDeath?.Invoke(this);
        enemyCollider.enabled = false;
        healthBarCanvas.enabled = false;
        behaviorGraphAgent.BlackboardReference.SetVariableValue(isDeadVarName, IsDead);
        if (animator) animator.SetTrigger(Dead);
        yield return new WaitForSeconds(deathAnimDuration);
        Destroy(this.gameObject);
    }

    public bool IsDead => _currentHealth <= 0;
}