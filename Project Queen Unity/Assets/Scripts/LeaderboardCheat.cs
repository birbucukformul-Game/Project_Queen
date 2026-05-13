using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Linq;

public class LeaderboardCheat : MonoBehaviour
{
    [Header("Þifre Ayarlarý")]
    [Tooltip("Tabloyu sýfýrlayacak gizli þifre")]
    [SerializeField] private string resetCheatCode = "mertbey";

    [Tooltip("Tabloya 'Yazýlýmcýlar - 237' ekleyecek gizli þifre")]
    [SerializeField] private string developerCheatCode = "yazilim";

    private int resetIndex = 0;
    private int developerIndex = 0;

    private void OnEnable()
    {
        if (Keyboard.current != null)
        {
            Keyboard.current.onTextInput += OnTextInput;
        }
    }

    private void OnDisable()
    {
        if (Keyboard.current != null)
        {
            Keyboard.current.onTextInput -= OnTextInput;
        }
    }

    private void OnTextInput(char c)
    {
        char input = char.ToLower(c);

        if (input == resetCheatCode[resetIndex])
        {
            resetIndex++;
            if (resetIndex >= resetCheatCode.Length)
            {
                ResetLeaderboard();
                resetIndex = 0;
            }
        }
        else
        {
            resetIndex = (input == resetCheatCode[0]) ? 1 : 0;
        }

        if (input == developerCheatCode[developerIndex])
        {
            developerIndex++;
            if (developerIndex >= developerCheatCode.Length)
            {
                AddDeveloperScore();
                developerIndex = 0;
            }
        }
        else
        {
            developerIndex = (input == developerCheatCode[0]) ? 1 : 0;
        }
    }

    private void ResetLeaderboard()
    {
        PlayerPrefs.DeleteKey("LocalLeaderboard");
        PlayerPrefs.DeleteKey("HighScores");
        PlayerPrefs.Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void AddDeveloperScore()
    {
        string json = PlayerPrefs.GetString("LocalLeaderboard", "{}");
        LeaderboardData data = JsonUtility.FromJson<LeaderboardData>(json) ?? new LeaderboardData();

        ScoreEntry existingEntry = data.scores.FirstOrDefault(s => s.playerName == "Yazilimcilar");

        if (existingEntry != null)
        {
            existingEntry.score = 237;
        }
        else
        {
            data.scores.Add(new ScoreEntry { playerName = "Yazilimcilar", score = 237 });
        }

        data.scores = data.scores.OrderByDescending(s => s.score).ToList();

        string newJson = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("LocalLeaderboard", newJson);
        PlayerPrefs.Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}