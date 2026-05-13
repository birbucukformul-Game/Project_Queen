using UnityEngine;
using TMPro;

public class LeaderboardRowUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI rankText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI scoreText;

    public void SetData(int rank, string playerName, int score)
    {
        rankText.text = $"{rank}.";
        nameText.text = playerName;
        scoreText.text = $"{score} Points";
    }
}