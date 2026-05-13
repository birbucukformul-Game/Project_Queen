using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Arayüz Elemanlarý")]
    [Tooltip("Ýlk Baþla butonuna basýnca açýlacak olan isim girme paneli")]
    [SerializeField] private GameObject nameInputPanel;

    [Tooltip("Kullanýcýnýn ismini gireceði alan (Panelin içindeki)")]
    [SerializeField] private TMP_InputField nameInputField;

    [Tooltip("Ana ekranda duran ilk Baþla butonu (Panel açýlýnca gizlenecek)")]
    [SerializeField] private GameObject startButton;

    [Header("Mod Ayarlarý")]
    [Tooltip("Challenge modunu seçeceðimiz Toggle (Checkbox) arayüz elemaný")]
    [SerializeField] private Toggle challengeModeToggle;

    [Header("Ayarlar")]
    [SerializeField] private string defaultPlayerName = "Oyuncu";

    private void OnEnable()
    {
        if (nameInputField != null)
        {
            nameInputField.onSubmit.AddListener(OnInputSubmit);
        }
    }

    private void OnDisable()
    {
        if (nameInputField != null)
        {
            nameInputField.onSubmit.RemoveListener(OnInputSubmit);
        }
    }

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (nameInputPanel != null) nameInputPanel.SetActive(false);
        if (startButton != null) startButton.SetActive(true);
    }

    public void OpenNameInputPanel()
    {
        if (nameInputPanel != null) nameInputPanel.SetActive(true);
        if (startButton != null) startButton.SetActive(false);

        if (challengeModeToggle != null)
        {
            challengeModeToggle.isOn = PlayerPrefs.GetInt("ChallengeMode", 0) == 1;
        }

        if (nameInputField != null) nameInputField.Select();
    }

    public void StartGameWithPlayerName()
    {
        string playerName = nameInputField.text;

        if (string.IsNullOrWhiteSpace(playerName))
        {
            playerName = defaultPlayerName;
        }

        PlayerPrefs.SetString("CurrentPlayerName", playerName);

        int isChallengeActive = (challengeModeToggle != null && challengeModeToggle.isOn) ? 1 : 0;
        PlayerPrefs.SetInt("ChallengeMode", isChallengeActive);

        PlayerPrefs.Save();

        SceneManager.LoadScene(1);
    }

    private void OnInputSubmit(string input)
    {
        StartGameWithPlayerName();
    }
}