using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IGetHit
{
    public static PlayerHealth instance;

    [Header("So's")] [SerializeField] private PlayerSo playerSo;

    [Header("References")] [SerializeField]
    private PlayerAnimator playerAnimator;

    [SerializeField] private Animator animator;
    [SerializeField] private Collider playerCollider;

    [Space] [Header("UI Settings")] [SerializeField]
    private Image healthBarSprite;

    [SerializeField] private Canvas healthBarCanvas;
    public float deathAnimDuration;

    [Header("Health Settings")] [SerializeField]
    private float maxHealth = 100f;

    private float _currentHealth;
    public bool IsDead => _currentHealth <= 0;
    [Tooltip("Event Actions")] public static event Action<PlayerHealth> OnPlayerDeath;


    void Start()
    {
        maxHealth = playerSo.health;
        _currentHealth = maxHealth;
        healthBarSprite.fillAmount = _currentHealth / maxHealth;
    }


    private void OnEnable()
    {
        StartCoroutine(SpawnFx());
    }

    private IEnumerator SpawnFx()
    {
        yield return new WaitForSeconds(0.1f);
        Instantiate(playerSo.spawnVFx, transform.position, Quaternion.identity);
    }

    public void GetHit(DamageInfo info)
    {
        if (IsDead) return;
        if (info.attacker.CompareTag("Enemy"))
        {
            _currentHealth -= Mathf.RoundToInt(info.damageAmount);
            // Debug.Log("Player Get hit by " + info.attacker.name + " for " + info.damageAmount + " damage.");
            UpdateUI();
        }

        if (IsDead) HandleDeath();
    }

    private void HandleDeath()
    {
        DisableWDeath();
        playerAnimator.SetSingleDeathState(animator);

        OnPlayerDeath?.Invoke(this);
    }

    public void DisableWDeath()
    {
        healthBarCanvas.enabled = false;
        playerCollider.enabled = false;
    }

    private void UpdateUI()
    {
        if (!healthBarSprite) return;

        healthBarSprite.fillAmount = _currentHealth / maxHealth;
    }
}