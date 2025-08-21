using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public int Score { get; private set; } = 0;
    [SerializeField] private float scoreMultiplier = 1f;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this.gameObject);
    }

    public void SetMultiplier(float multiplier)
    {
        scoreMultiplier = multiplier;
    }

    public void AddScore(int amount)
    {
        Score += Mathf.RoundToInt(amount * scoreMultiplier);
    }

    public int GetScore()
    {
        return Score;
    }
}