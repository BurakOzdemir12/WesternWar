using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")] [SerializeField]
    private GameObject menuPanel;

    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject winPanel;

    [SerializeField] private Slider progressBar;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI scoreText;
    private bool _shouldUpdateProgress = false;

    [Tooltip("Game over Panel Text")]
    private void OnEnable()
    {
        GameManager.OnGameStateChanged += HandleGameStateChange;
    }

    void Start()
    {
        progressBar.value = 0;
        gamePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        levelText.text = "Level " + (LevelManager.Instance.GetCurrentLevelIndex() + 1);
    }

    void Update()
    {
        if (_shouldUpdateProgress)
        {
            UpdateProgressBar();
        }

        scoreText.text = "Score " + ScoreManager.instance.GetScore().ToString("0000");
    }


    private void HandleGameStateChange(GameManager.GameState state)
    {
        _shouldUpdateProgress = state == GameManager.GameState.Game;
        switch (state)
        {
            case GameManager.GameState.Menu:
                menuPanel.SetActive(true);
                gamePanel.SetActive(false);
                gameOverPanel.SetActive(false);
                break;
            case GameManager.GameState.Game:
                menuPanel.SetActive(false);
                gamePanel.SetActive(true);
                gameOverPanel.SetActive(false);
                break;
            case GameManager.GameState.LevelComplete:
                winPanel.SetActive(true);
                break;
            case GameManager.GameState.GameOver:
                ShowGameOverPanel();
                break;
        }
    }

    public void PlayButtonPressed()
    {
        GameManager.Instance.SetGameState(GameManager.GameState.Game);
        menuPanel.SetActive(false);
        gamePanel.SetActive(true);
    }

    public void RetryButtonPressed()
    {
        SceneManager.LoadScene(sceneBuildIndex: SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnTowneButtonPressed()
    {
        SceneManager.LoadScene(sceneBuildIndex: 1);
    }

    // public void NextLevelButtonPressed()
    // {
    //     int nextLevelIndex = LevelGenerator.instance.GetCurrentLevelIndex() + 1;
    //     if (nextLevelIndex < LevelGenerator.instance.GetTotalLevels())
    //     {
    //         SceneManager.LoadScene(nextLevelIndex);
    //     }
    //     else
    //     {
    //         Debug.Log("No more levels available, going back to menu.");
    //         GameManager.instance.SetGameState(GameManager.GameState.Menu);
    //     }
    // }
    private void ShowGameOverPanel()
    {
        gamePanel.SetActive(false);
        gameOverPanel.SetActive(true);
        TextPulseEffect.instance.TriggerObjectEffect(gameOverPanel);
    }

    private float _simulatedPlayerZ = 0f;
    private float _progressDisplay = 0f;

    public void UpdateProgressBar()
    {
        // _simulatedPlayerZ += LevelGenerator.instance.GetMoveSpeed() * Time.deltaTime;
        // float finishZ = LevelGenerator.instance.IsFinishSpawned
        //     ? LevelGenerator.instance.GetFinishZ()
        //     : LevelGenerator.instance.EstimateFinishZ();
        //
        // float progress = Mathf.InverseLerp(0f, finishZ, _simulatedPlayerZ);
        // progressBar.value = Mathf.Clamp01(progress);

        // Smoothly fill based on enemies defeated
        float target = EnemyCounter.instance ? EnemyCounter.instance.GetCompletion01() : 0f;

        // Smooth movement toward target (tweak speed as desired)
        float fillSpeed = 1.5f; // units per second
        _progressDisplay = Mathf.MoveTowards(_progressDisplay, target, fillSpeed * Time.deltaTime);

        progressBar.value = Mathf.Clamp01(_progressDisplay);
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= HandleGameStateChange;
    }
}