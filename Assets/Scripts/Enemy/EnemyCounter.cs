using System;
using Abstract;
using TMPro;
using UnityEngine;

public class EnemyCounter : LevelBasedMonoBehaviour
{
    public static EnemyCounter instance;

    [SerializeField] private TextMeshProUGUI enemyCounterText;
    private int _enemyCount;
    private int _initialEnemyCount;
    private int _remainingEnemyCount;
    private bool inGame;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += HandleGameStateChanged;

        EnemyHealth.OnAnyDeath += HandleEnemyDeath;
    }

    private void HandleGameStateChanged(GameManager.GameState newState)
    {
        inGame = newState == GameManager.GameState.Game;

        if (!inGame) return;

        foreach (var spawn in currentLevel.enemySpawns)
        {
            _initialEnemyCount += spawn.maxEnemies;
        }

        _remainingEnemyCount = _initialEnemyCount;
        UpdateEnemyCounterText();
    }

    private void UpdateEnemyCounterText()
    {
        enemyCounterText.text = _remainingEnemyCount.ToString();
    }

    void Start()
    {
        InitializeLevelSettings();

        // Initialize with planned spawns at the start of the level
        // _initialEnemyCount = currentLevel != null ? currentLevel.enemySpawns.Count : 0;
        // _remainingEnemyCount = _initialEnemyCount;
    }

    void Update()
    {
    }

    private void HandleEnemyDeath(EnemyHealth _)
    {
        if (!inGame) return;

        if (_remainingEnemyCount > 0)
            _remainingEnemyCount = Mathf.Max(0, _remainingEnemyCount - 1);
        UpdateEnemyCounterText();

        if (_remainingEnemyCount <= 0)
        {
            GameManager.instance.SetGameState(GameManager.GameState.LevelComplete);
        }
    }

    public int EnemyCount()
    {
        // Backward compatibility: remaining enemies
        return _enemyCount = _remainingEnemyCount;
    }

    public float GetCompletion01()
    {
        // 0 when none killed, 1 when all killed
        return _initialEnemyCount <= 0 ? 1f : 1f - (_remainingEnemyCount / (float)_initialEnemyCount);
    }

    private void OnDisable()
    {
        EnemyHealth.OnAnyDeath -= HandleEnemyDeath;
    }
}