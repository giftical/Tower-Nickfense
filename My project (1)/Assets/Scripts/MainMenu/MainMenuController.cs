using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainPanel;           // stays visible
    [SerializeField] private GameObject difficultyPanel;     // overlay popup
    [SerializeField] private GameObject settingsPanel;       // overlay popup
    [SerializeField] private GameObject encyclopediaPanel;   // overlay popup

    [Header("Gameplay Scene")]
    [SerializeField] private string gameplaySceneName = "Game";

    private void Start()
    {
        if (mainPanel) mainPanel.SetActive(true);
        CloseAllPopups();
    }

    private void CloseAllPopups()
    {
        if (difficultyPanel) difficultyPanel.SetActive(false);
        if (settingsPanel) settingsPanel.SetActive(false);
        if (encyclopediaPanel) encyclopediaPanel.SetActive(false);
    }

    // Main buttons
    public void OnPlayPressed()
    {
        CloseAllPopups();
        if (difficultyPanel) difficultyPanel.SetActive(true);
    }

    public void OnSettingsPressed()
    {
        CloseAllPopups();
        if (settingsPanel) settingsPanel.SetActive(true);
    }

    public void OnEncyclopediaPressed()
    {
        CloseAllPopups();
        if (encyclopediaPanel) encyclopediaPanel.SetActive(true);
    }

    // Popup close button(s)
    public void OnClosePopupPressed()
    {
        CloseAllPopups();
    }

    public void OnEasyPressed() => StartGame();
    public void OnNormalPressed() => StartGame();
    public void OnHardPressed() => StartGame();

    private void StartGame()
    {
        SceneManager.LoadScene(gameplaySceneName);
    }
}