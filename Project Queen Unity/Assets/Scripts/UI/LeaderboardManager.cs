using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ScoreEntry
{
    public string playerName;
    public int score;
}

[Serializable]
public class LeaderboardData
{
    public List<ScoreEntry> scores = new List<ScoreEntry>();
}

public class LeaderboardManager : MonoBehaviour
{
    [Header("UI Referanslarý")]
    [SerializeField] private List<LeaderboardRowUI> rowList;

    [Header("Ayarlar")]
    [SerializeField] private int maxEntryCount = 10;

    private void OnEnable() => PlayerCollision.OnGameOver += HandleLeaderboard;
    private void OnDisable() => PlayerCollision.OnGameOver -= HandleLeaderboard;

    private void Start()
    {
        DisplayLeaderboard(LoadLeaderboard());
    }

    private void HandleLeaderboard()
    {
        GameManager gm = FindFirstObjectByType<GameManager>();
        if (gm == null) return;

        int finalScore = gm.GetCurrentScore();
        string currentName = PlayerPrefs.GetString("CurrentPlayerName", "Oyuncu");

        LeaderboardData data = LoadLeaderboard();
        ScoreEntry existingEntry = data.scores.FirstOrDefault(s => s.playerName == currentName);

        if (existingEntry != null)
        {
            if (finalScore > existingEntry.score)
            {
                existingEntry.score = finalScore;
            }
        }
        else
        {
            data.scores.Add(new ScoreEntry { playerName = currentName, score = finalScore });
        }

        data.scores = data.scores.OrderByDescending(s => s.score).Take(maxEntryCount).ToList();

        SaveLeaderboard(data);
        DisplayLeaderboard(data);
    }

    private LeaderboardData LoadLeaderboard()
    {
        string json = PlayerPrefs.GetString("LocalLeaderboard", "{}");
        return JsonUtility.FromJson<LeaderboardData>(json) ?? new LeaderboardData();
    }

    private void SaveLeaderboard(LeaderboardData data)
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("LocalLeaderboard", json);
        PlayerPrefs.Save();
    }

    private void DisplayLeaderboard(LeaderboardData data)
    {
        if (rowList == null || rowList.Count == 0) return;

        for (int i = 0; i < rowList.Count; i++)
        {
            if (i < data.scores.Count)
            {
                rowList[i].gameObject.SetActive(true);
                rowList[i].SetData(i + 1, data.scores[i].playerName, data.scores[i].score);
            }
            else
            {
                rowList[i].gameObject.SetActive(false);
            }
        }
    }
}