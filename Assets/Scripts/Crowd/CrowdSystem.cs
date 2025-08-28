using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CrowdSystem : MonoBehaviour
{
    public static CrowdSystem Instance;
    public static event Action OnRunnersChanged;
    public static event Action<WeaponsSo> OnWeaponChanged;

    [Header("Weapon State")] [SerializeField]
    private WeaponsSo startingWeapon;

    private WeaponsSo _currentWeaponSo;
    [SerializeField] private AudioSource[] gunAudioSources;

    [Header("References")] [SerializeField]
    private PlayerController playerController;

    [SerializeField] private PlayerAnimator playerAnimator;

    [Header("Elements")] [SerializeField] private Transform runnerParent;
    [SerializeField] private GameObject runnerPrefab;
    [Header("Settings")] [SerializeField] private float angle;
    [SerializeField] private float radius = 5f;

    [SerializeField] List<GameObject> _runners = new List<GameObject>();
    [SerializeField] private int runnerCount;
    private bool _isInGame = false;

    private void Awake()
    {
        Instance = this;
        _currentWeaponSo = startingWeapon;

        playerController = GetComponent<PlayerController>();
    }

    private void OnEnable()
    {
        GameManager.Subscribe(HandleGameStateChange);
        PlayerHealth.OnAnyPlayerDeath += HandlePlayerDeathByEnemy;
        Barrel.OnBreakBarrelWeapon += HandleBarrelWeaponReward;
        Barrel.OnBreakBarrelMan += HandleBarrelManReward;
    }

    private void HandleBarrelManReward(int amount)
    {
        ApplyBonus(amount, BonusType.Addition);
    }

    private void HandleBarrelWeaponReward(WeaponsSo weapons)
    {
        _currentWeaponSo = weapons;
        OnWeaponChanged?.Invoke(weapons);
    }

    public WeaponsSo GetCurrentWeapon() => _currentWeaponSo;
    public WeaponsSo GetStartingWeapon() => startingWeapon;

    private void HandlePlayerDeathByEnemy(PlayerHealth playerHealth)
    {
        if (playerHealth == null) return;

        _runners.Remove(playerHealth.gameObject);

        if (playerHealth.transform.parent == runnerParent)
            playerHealth.transform.SetParent(null);

        StartCoroutine(AnimationDelay(playerHealth.gameObject, playerHealth.deathAnimDuration));

        runnerCount = runnerParent.childCount;
        OnRunnersChanged?.Invoke();

        // if (runnerCount <= 0 && _isInGame)
        // {
        //     GameManager.instance.SetGameState(GameManager.GameState.GameOver);
        // }
    }

    private IEnumerator AnimationDelay(GameObject go, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(go);
    }

    private void HandleGameStateChange(GameManager.GameState newState)
    {
        _isInGame = newState == GameManager.GameState.Game;
    }

    void Start()
    {
        runnerCount = runnerParent.childCount;

        for (int i = 0; i < runnerParent.childCount; i++)
        {
            Transform child = runnerParent.GetChild(i);
            if (child != null)
            {
                _runners.Add(child.gameObject);
            }

            var pwc = child.GetComponent<PlayerWeaponController>();
            if (pwc && _currentWeaponSo) pwc.ForceSetWeapon(_currentWeaponSo);
        }

        // _runners.Add(runnerPrefab);
    }

    void Update()
    {
        PlaceRunners();


        if (runnerCount <= 0 && _isInGame)
        {
            GameManager.Instance.SetGameState(GameManager.GameState.GameOver);
        }
    }

    private void PlaceRunners()
    {
        for (int i = 0; i < runnerParent.childCount; i++)
        {
            Vector3 childLocalPos = GetRunnerLocalPosition(i);
            runnerParent.GetChild(i).localPosition = childLocalPos;
        }
    }

    private Vector3 GetRunnerLocalPosition(int index)
    {
        float x = radius * Mathf.Sqrt(index) * Mathf.Cos(Mathf.Deg2Rad * index * angle);
        float z = radius * Mathf.Sqrt(index) * Mathf.Sin(Mathf.Deg2Rad * index * angle);


        return new Vector3(x, 0, z);
    }

    private void ChangeRadiusByCount(float amount)
    {
        // if (radius > 0.3f) radius -= 0.005f;
    }

    public float GetCrowdRadius()
    {
        return radius * Mathf.Sqrt(runnerParent.childCount);
    }

    public void ApplyBonus(int amountOfBonus, BonusType bonusType)
    {
        switch (bonusType)
        {
            case BonusType.Addition:
                SpawnRunners(amountOfBonus);
                break;
            case BonusType.Multiply:
                var runnersToAdd = (amountOfBonus * runnerCount) - runnerCount;
                SpawnRunners(runnersToAdd);
                break;
            case BonusType.Subtract:
                StartCoroutine(RemoveRunners(Mathf.Abs(amountOfBonus)));
                break;
            case BonusType.Divide:
                var runnersToRemove = runnerCount - (runnerCount / amountOfBonus);
                StartCoroutine(RemoveRunners(runnersToRemove));

                break;
        }
    }

    private void SpawnRunners(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            var newRunner = Instantiate(runnerPrefab, runnerParent.position, Quaternion.identity, runnerParent);
            var pwc = newRunner.GetComponent<PlayerWeaponController>();
            if (pwc && _currentWeaponSo) pwc.ForceSetWeapon(_currentWeaponSo);
            ChangeRadiusByCount(amount);
            _runners.Add(newRunner);
        }

        runnerCount = runnerParent.childCount;
        OnRunnersChanged?.Invoke();
    }

    private IEnumerator RemoveRunners(int amount)
    {
        int actualCount = runnerParent.childCount;
        int toRemove = Mathf.Min(amount, actualCount);

        for (int i = actualCount - 1; i >= actualCount - toRemove; i--)
        {
            if (i < 0) break;

            Transform runnerToRemove = runnerParent.GetChild(i);
            if (runnerToRemove != null)
            {
                var playerHealth = runnerToRemove.GetComponent<PlayerHealth>();

                if (playerHealth != null)
                {
                    playerHealth.DisableWDeath();
                }

                playerAnimator.SetMultipleDeathState(runnerToRemove);

                _runners.Remove(runnerToRemove.gameObject);

                runnerToRemove.SetParent(null);

                StartCoroutine(DelayedDestroy(runnerToRemove.gameObject, 3f));
            }
        }

        runnerCount = runnerParent.childCount;
        OnRunnersChanged?.Invoke();

        yield return null;
    }

    private IEnumerator DelayedDestroy(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(obj);
    }

    // private IEnumerator RemoveRunners(int amount)
    // {
    //     int actualCount = runnerParent.childCount;
    //     int toRemove = Mathf.Min(amount, actualCount);
    //     WaitForSeconds wait = new WaitForSeconds(3f);
    //     for (int i = actualCount - 1; i >= actualCount - toRemove; i--)
    //     {
    //         if (i < 0) break;
    //
    //         Transform runnerToDestroy = runnerParent.GetChild(i);
    //         if (runnerToDestroy != null)
    //         {
    //             playerAnimator.SetMultipleDeathState(runnerToDestroy);
    //             runnerToDestroy.SetParent(null);
    //
    //             _runners.Remove(runnerToDestroy.gameObject);
    //             yield return wait;
    //             Destroy(runnerToDestroy.gameObject);
    //         }
    //     }
    //
    //     _runnerCount = runnerParent.childCount;
    //     OnRunnersChanged?.Invoke();
    // }

    public int GetRunnerCount()
    {
        return runnerCount;
    }

    private void OnDisable()
    {
        GameManager.Unsubscribe(HandleGameStateChange);
        PlayerHealth.OnAnyPlayerDeath -= HandlePlayerDeathByEnemy;
        Barrel.OnBreakBarrelWeapon -= HandleBarrelWeaponReward;
        Barrel.OnBreakBarrelMan -= HandleBarrelManReward;
    }
}