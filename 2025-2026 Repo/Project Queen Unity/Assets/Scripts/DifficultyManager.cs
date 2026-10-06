using System;
using System.Collections;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static event Action<float, float> OnDifficultyIncreased;

    [Header("Normal Mode Settings")]
    [SerializeField] private float normalIncreaseInterval = 5f;
    [SerializeField] private float normalStartingSpeedMultiplier = 1f;
    [SerializeField] private float normalStartingSpawnInterval = 2f;

    [Header("Challenge Mode Settings")]
    [Tooltip("Zorluðun ne kadar sürede bir artacaðý. 5f yerine 2f yaparsan oyun çok daha hýzlý zorlaþýr.")]
    [SerializeField] private float challengeIncreaseInterval = 2f;
    [SerializeField] private float challengeStartingSpeedMultiplier = 1.5f;
    [SerializeField] private float challengeStartingSpawnInterval = 1.5f;

    [Header("Shared Limits (Ortak Limitler)")]
    [SerializeField] private float speedIncreaseAmount = 0.2f;
    [SerializeField] private float maxSpeedMultiplier = 10f;
    [SerializeField] private float spawnIntervalDecreaseAmount = 0.15f;
    [SerializeField] private float minSpawnInterval = 0.2f;

    private float currentSpeedMultiplier;
    private float currentSpawnInterval;
    private float activeIncreaseInterval;
    private bool isGameOver = false;

    private void OnEnable() => PlayerCollision.OnGameOver += HandleGameOver;
    private void OnDisable() => PlayerCollision.OnGameOver -= HandleGameOver;

    private void Start()
    {
        if (PlayerPrefs.GetInt("ChallengeMode", 0) == 1)
        {
            activeIncreaseInterval = challengeIncreaseInterval;
            currentSpeedMultiplier = challengeStartingSpeedMultiplier;
            currentSpawnInterval = challengeStartingSpawnInterval;
        }
        else
        {
            activeIncreaseInterval = normalIncreaseInterval;
            currentSpeedMultiplier = normalStartingSpeedMultiplier;
            currentSpawnInterval = normalStartingSpawnInterval;
        }

        OnDifficultyIncreased?.Invoke(currentSpeedMultiplier, currentSpawnInterval);

        StartCoroutine(DifficultyRoutine());
    }

    private void HandleGameOver() => isGameOver = true;

    private IEnumerator DifficultyRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(activeIncreaseInterval);

        while (!isGameOver)
        {
            yield return wait;

            currentSpeedMultiplier = Mathf.Min(currentSpeedMultiplier + speedIncreaseAmount, maxSpeedMultiplier);
            currentSpawnInterval = Mathf.Max(currentSpawnInterval - spawnIntervalDecreaseAmount, minSpawnInterval);

            OnDifficultyIncreased?.Invoke(currentSpeedMultiplier, currentSpawnInterval);
        }
    }
}