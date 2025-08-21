using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public enum GameState
    {
        Menu,
        Game,
        LevelComplete,
        GameOver
    }

    private GameState _gameState;

    public static event Action<GameState> OnGameStateChanged;

//TODO Add Countwown Timer 3 2 1 Go!
    private void Awake()
    {
        PlayerPrefs.SetInt("level", 0);
        // Cursor.visible = false;
        if (instance != null)
        {
            Destroy(gameObject);
            return; // önemli
        }
        instance = this;
    }


    public void SetGameState(GameState gameState)
    {
        this._gameState = gameState;
        OnGameStateChanged?.Invoke(gameState);
        Debug.Log("Game State Changed: " + gameState);
    }

    public bool IsGameActive => _gameState == GameState.Game;

    public GameState CurrentState => _gameState;

    // ---- Yapışkan abonelik (sticky) ----
    public static void Subscribe(Action<GameState> handler, bool invokeImmediately = true)
    {
        OnGameStateChanged += handler;
        if (invokeImmediately && instance != null)
            handler(instance._gameState); // HEMEN mevcut state ile haber ver
    }

    public static void Unsubscribe(Action<GameState> handler)
    {
        OnGameStateChanged -= handler;
    }
}