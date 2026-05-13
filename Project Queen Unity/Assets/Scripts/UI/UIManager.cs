using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Skor Arayüzü")]
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Paneller")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject nameInputPanel;

    [Header("Girdi Alaný")]
    [SerializeField] private TMP_InputField nameInputField;

    [Tooltip("Game Over panelinde mevcut skoru gösterecek metin")]
    [SerializeField] private TextMeshProUGUI gameOverScoreText;

    [Tooltip("Challenge modu aktifse görünecek x2 simgesi")]
    [SerializeField] private GameObject x2MultiplierIcon;

    private bool isPaused = false;
    private bool isGameOver = false;

    private void OnEnable()
    {
        GameManager.OnScoreChanged += UpdateScoreUI;
        PlayerCollision.OnGameOver += ShowGameOverUI;
        PlayerInputHandler.OnPause += TogglePause;
    }

    private void OnDisable()
    {
        GameManager.OnScoreChanged -= UpdateScoreUI;
        PlayerCollision.OnGameOver -= ShowGameOverUI;
        PlayerInputHandler.OnPause -= TogglePause;
    }

    private void Start()
    {
        Time.timeScale = 1f;
        isPaused = false;
        isGameOver = false;

        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);
        if (nameInputPanel != null) nameInputPanel.SetActive(false);

        bool isChallengeMode = PlayerPrefs.GetInt("ChallengeMode", 0) == 1;

        if (scoreText != null)
        {
            scoreText.color = isChallengeMode ? Color.red : Color.white;
        }

        if (x2MultiplierIcon != null)
        {
            x2MultiplierIcon.SetActive(isChallengeMode);
        }

        UpdateScoreUI(0);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void UpdateScoreUI(int newScore) => scoreText.text = newScore.ToString();

    private void ShowGameOverUI()
    {
        isGameOver = true;
        if (gameOverPanel != null) gameOverPanel.SetActive(true);

        if (gameOverScoreText != null)
        {
            GameManager gm = FindFirstObjectByType<GameManager>();
            if (gm != null)
            {
                gameOverScoreText.text = "Score: " + gm.GetCurrentScore().ToString();
            }
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void TogglePause()
    {
        if (isGameOver) return;

        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            if (pausePanel != null) pausePanel.SetActive(true);

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Time.timeScale = 1f;
            if (pausePanel != null) pausePanel.SetActive(false);

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void RestartGame()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (nameInputPanel != null) nameInputPanel.SetActive(true);
    }

    public void ConfirmNameAndRestart()
    {
        if (nameInputField != null && !string.IsNullOrEmpty(nameInputField.text))
        {
            PlayerPrefs.SetString("PlayerName", nameInputField.text);
            PlayerPrefs.Save();
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}