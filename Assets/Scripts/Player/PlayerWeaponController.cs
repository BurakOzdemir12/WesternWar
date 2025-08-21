using System;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private Transform weaponHolder;
    [SerializeField] private LayerMask obstacleLayers;
    [SerializeField] private WeaponsSo weaponsSo;

    private Weapon _currentWeapon;
    private bool _isInGame = false;
    private Coroutine _waitCoro;

    private float _fireTimer = 0f;
    private float FireInterval => 1f / weaponsSo.fireRate;

    private void Awake()
    {
    }

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += HandleGameStateChanged;
        CrowdSystem.OnRunnersChanged += HandleCrowdChanged;
        if (GameManager.instance != null)
            _isInGame = GameManager.instance.IsGameActive;
        else
            _waitCoro = StartCoroutine(WaitForGameManager());
    }


    private System.Collections.IEnumerator WaitForGameManager()
    {
        while (GameManager.instance == null) yield return null;
        _isInGame = GameManager.instance.IsGameActive;
        if (_isInGame) _fireTimer = 0f;
    }

    private void Start()
    {
        InitializeWeapon(weaponsSo);
    }

    void Update()
    {
        if (!_isInGame) return;

        _fireTimer += Time.deltaTime;
        if (_fireTimer >= FireInterval)
        {
            _currentWeapon.Fire();
            // HandleFire();
            _fireTimer = 0f; // catch-up YOK: tek mermi, sıfırla
            // Eğer kare düşüşlerinde atış kaçırmak istemezsen:
            // _fireTimer -= FireInterval;  // (ama bu da birikme telafisi yapar)
        }
    }

    private void InitializeWeapon(WeaponsSo sO)
    {
        Weapon weaponGo = Instantiate(weaponsSo.weaponModel, weaponHolder).GetComponent<Weapon>();
        weaponGo.transform.localPosition = Vector3.zero;
        weaponGo.transform.localRotation = Quaternion.identity;
        weaponGo.transform.localScale = Vector3.one;
        _currentWeapon = weaponGo;
        this.weaponsSo = sO;

        _currentWeapon.Initialize(sO);
        // _currentWeapon.Initialize(weaponsSo);
        _fireTimer = 0f;
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
        if (!_isInGame) weaponHolder.gameObject.SetActive(false);
    }

    // private void HandleFire()
    // {
    //     if (!_isInGame) return;
    //
    //     FireFx();
    //
    //     var bulletGo = Instantiate(
    //         weaponsSo.bulletPrefab,
    //         weaponHolder.position,
    //         Quaternion.LookRotation(weaponHolder.forward)
    //         // projectileParent.transform
    //     );
    //
    //     var rb = bulletGo.GetComponent<Rigidbody>();
    //     rb.linearVelocity = weaponHolder.forward * weaponsSo.bulletSpeed;
    // }

    // private void FireFx()
    // {
    //     var vfx = muzzleVfx;
    //     // var muzzle = Instantiate(vfx, weaponsSo.muzzleEnd, false);
    //     if (vfx != null)
    //         vfx.Play();
    //     var clip = weaponsSo.shotSfx;
    //     AudioSource.PlayClipAtPoint(clip,
    //         transform.position,
    //         1f
    //     );
    // }

    private void OnDrawGizmos()
    {
        if (!_isInGame) return;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(weaponHolder.position, weaponHolder.position + weaponHolder.forward * weaponsSo.range);
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
        CrowdSystem.OnRunnersChanged -= HandleCrowdChanged;
    }
}