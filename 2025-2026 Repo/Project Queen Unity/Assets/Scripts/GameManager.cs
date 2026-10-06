using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event Action<int> OnScoreChanged;

    [Header("Score Settings")]
    [SerializeField] private int scorePerSecond = 1;
    [SerializeField] private float scoreUpdateInterval = 1f;

    private int currentScore = 0;
    private bool isGameOver = false;

    private void OnEnable() => PlayerCollision.OnGameOver += HandleGameOver;
    private void OnDisable() => PlayerCollision.OnGameOver -= HandleGameOver;

    private void Start()
    {
        currentScore = 0;

        if (PlayerPrefs.GetInt("ChallengeMode", 0) == 1)
        {
            scorePerSecond *= 2;
        }

        StartCoroutine(ScoreRoutine());
    }

    private void HandleGameOver() => isGameOver = true;

    public int GetCurrentScore() => currentScore;

    private IEnumerator ScoreRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(scoreUpdateInterval);

        while (!isGameOver)
        {
            yield return wait;
            currentScore += scorePerSecond;
            OnScoreChanged?.Invoke(currentScore);
        }
    }
}