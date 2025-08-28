using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
            
            if (!PlayerPrefs.HasKey("level"))
                PlayerPrefs.SetInt("level", 0);
            if (!PlayerPrefs.HasKey("unlockedLevels"))
                PlayerPrefs.SetInt("unlockedLevels", 0);
            
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }


    private void OnEnable()
    {
        GameManager.OnGameStateChanged += HandleGameState;
    }

    private void HandleGameState(GameManager.GameState obj)
    {
        if (obj == GameManager.GameState.LevelComplete)
        {
            AdvanceLevelProgress();
        }
    }

    public int GetCurrentLevelIndex()
    {
        return PlayerPrefs.GetInt("level", 0);
    }

    public void AdvanceLevelProgress()
    {
        int nextLevelIndex = GetCurrentLevelIndex() + 1;
        PlayerPrefs.SetInt("level", nextLevelIndex);
        
        int currentUnlocked = PlayerPrefs.GetInt("unlockedLevels", 0);
        int newUnlocked = Mathf.Max(currentUnlocked, nextLevelIndex);
        PlayerPrefs.SetInt("unlockedLevels", newUnlocked);
        
        PlayerPrefs.Save();
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= HandleGameState;
    }
}