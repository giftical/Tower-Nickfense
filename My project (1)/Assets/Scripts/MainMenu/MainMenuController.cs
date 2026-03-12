using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainPanel;         // stays visible
    [SerializeField] private GameObject playPanel;         // overlay popup
    [SerializeField] private GameObject settingsPanel;     // overlay popup
    [SerializeField] private GameObject encyclopediaPanel; // overlay popup

    [Header("Gameplay Scene")]
    [SerializeField] private string gameplaySceneName = "GameScene";

    private void Start()
    {
        if (mainPanel) mainPanel.SetActive(true);
        CloseAllPopups();
    }

    private void CloseAllPopups()
    {
        if (playPanel) playPanel.SetActive(false);
        if (settingsPanel) settingsPanel.SetActive(false);
        if (encyclopediaPanel) encyclopediaPanel.SetActive(false);
    }

    // Main buttons
    public void OnPlayPressed()
    {
        CloseAllPopups();
        if (playPanel) playPanel.SetActive(true);
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

    // Play panel button
    public void OnStartGamePressed()
    {
        SceneManager.LoadScene(gameplaySceneName);
    }

    // Generic close button for popups
    public void OnClosePopupPressed()
    {
        CloseAllPopups();
    }

    public void OnQuitPressed()
    {
        Application.Quit();
    }
}