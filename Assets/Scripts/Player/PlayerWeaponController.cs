using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    public static PlayerWeaponController instance;


    [SerializeField] private WeaponsSo startingWeapon;
    [SerializeField] private Transform weaponHolder;
    [SerializeField] private WeaponsSo currentSo;
    [SerializeField] private PlayerHealth playerHealth;

    private readonly Dictionary<WeaponsSo, Weapon> _cache = new();
    private Weapon _currentWeapon;
    private bool _isInGame = false;
    private Coroutine _waitCoro;

    private float _fireTimer = 0f;
    private float FireInterval => 1f / currentSo.fireRate;


    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += HandleGameStateChanged;
        CrowdSystem.OnRunnersChanged += HandleCrowdChanged;
        CrowdSystem.OnWeaponChanged += HandleWeaponChanged;
        playerHealth.OnPlayerDeath += HandlePlayerDeath;

        if (GameManager.instance != null)
            _isInGame = GameManager.instance.IsGameActive;
        else
            _waitCoro = StartCoroutine(WaitForGameManager());

        var current = CrowdSystem.instance ? CrowdSystem.instance.GetCurrentWeapon() : null;
        if (current) ForceSetWeapon(current);
        else if (!currentSo && startingWeapon) ForceSetWeapon(startingWeapon);
    }


    private System.Collections.IEnumerator WaitForGameManager()
    {
        while (GameManager.instance == null) yield return null;
        _isInGame = GameManager.instance.IsGameActive;
        if (_isInGame) _fireTimer = 0f;
    }

    private void Start()
    {
    }

    void Update()
    {
        if (!_isInGame || !_currentWeapon || !currentSo) return;

        _fireTimer += Time.deltaTime;
        if (_fireTimer >= FireInterval)
        {
            _currentWeapon.Fire(currentSo);

            _fireTimer = 0f;

        }
    }

    private void HandlePlayerDeath(PlayerHealth obj)
    {
        SetWeaponVisibility(false);
    }

    private void HandleWeaponChanged(WeaponsSo so)
    {
        ForceSetWeapon(so);
    }

    public void ForceSetWeapon(WeaponsSo so)
    {
        if (!so) return;
        InitializeWeapon(so);
    }

    private void InitializeWeapon(WeaponsSo so)
    {
        if (currentSo == so && _currentWeapon)
        {
            _fireTimer = 0f;
            return;
        }

        if (_currentWeapon) Destroy(_currentWeapon.gameObject);

        var go = Instantiate(so.weaponModel, weaponHolder);
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
        go.transform.localScale = Vector3.one;

        _currentWeapon = go.GetComponent<Weapon>();
        _currentWeapon.Initialize(so);

        currentSo = so;
        _fireTimer = 0f;
        
        SetWeaponVisibility(true);
    }

    private void SetWeaponVisibility(bool shown)
    {
        weaponHolder.gameObject.SetActive(shown);
    }

    private void HandleCrowdChanged()
    {
        if (_isInGame) _fireTimer = 0f;
    }

    private void HandleGameStateChanged(GameManager.GameState newState)
    {
        _isInGame = newState == GameManager.GameState.Game;
        if (_isInGame)
            _fireTimer = 0f;
        SetWeaponVisibility(_isInGame);
        // weaponHolder.gameObject.SetActive(_isInGame);
    }


    private void OnDrawGizmos()
    {
        if (!_isInGame || !currentSo || !weaponHolder) return;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(weaponHolder.position, weaponHolder.position + weaponHolder.forward * currentSo.range);
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
        CrowdSystem.OnRunnersChanged -= HandleCrowdChanged;
        CrowdSystem.OnWeaponChanged -= HandleWeaponChanged;
        playerHealth.OnPlayerDeath -= HandlePlayerDeath;
    }
}