using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject difficultyPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject encyclopediaPanel;

    [Header("Encyclopedia Sections")]
    [SerializeField] private GameObject towersPanel;
    [SerializeField] private GameObject enemiesPanel;

    [Header("Gameplay Scene")]
    [SerializeField] private string gameplaySceneName = "Game";

    [Header("Audio Settings")]
    [SerializeField] private Slider volumeSlider;

    private const string PrefMusicVol = "music_volume";

    private void Start()
    {
        if (mainPanel) mainPanel.SetActive(true);
        CloseAllPopups();
        CloseEncyclopediaSections();
    }

    private void OnEnable()
    {
        if (volumeSlider != null)
        {
            float saved = PlayerPrefs.GetFloat(PrefMusicVol, 1f);
            volumeSlider.SetValueWithoutNotify(saved);
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }
    }

    private void OnDisable()
    {
        if (volumeSlider != null)
            volumeSlider.onValueChanged.RemoveListener(OnVolumeChanged);
    }

    private void OnVolumeChanged(float value)
    {
        value = Mathf.Clamp01(value);
        PlayerPrefs.SetFloat(PrefMusicVol, value);
        PlayerPrefs.Save();
    }

    private void CloseAllPopups()
    {
        if (difficultyPanel) difficultyPanel.SetActive(false);
        if (settingsPanel) settingsPanel.SetActive(false);
        if (encyclopediaPanel) encyclopediaPanel.SetActive(false);
    }

    private void CloseEncyclopediaSections()
    {
        if (towersPanel) towersPanel.SetActive(false);
        if (enemiesPanel) enemiesPanel.SetActive(false);
    }

    public void OnPlayPressed()
    {
        CloseAllPopups();
        CloseEncyclopediaSections();

        if (difficultyPanel) difficultyPanel.SetActive(true);
    }

    public void OnSettingsPressed()
    {
        CloseAllPopups();
        CloseEncyclopediaSections();

        if (settingsPanel) settingsPanel.SetActive(true);
    }

    public void OnEncyclopediaPressed()
    {
        CloseAllPopups();
        CloseEncyclopediaSections();

        if (encyclopediaPanel) encyclopediaPanel.SetActive(true);
    }

    public void OnOpenTowersPressed()
    {
        if (towersPanel) towersPanel.SetActive(true);
    }

    public void OnCloseTowersPressed()
    {
        if (towersPanel) towersPanel.SetActive(false);
    }

    public void OnOpenEnemiesPressed()
    {
        if (enemiesPanel) enemiesPanel.SetActive(true);
    }

    public void OnCloseEnemiesPressed()
    {
        if (enemiesPanel) enemiesPanel.SetActive(false);
    }

    public void OnClosePopupPressed()
    {
        CloseAllPopups();
        CloseEncyclopediaSections();
    }

    public void OnEasyPressed() => StartGame();
    public void OnNormalPressed() => StartGame();
    public void OnHardPressed() => StartGame();

    private void StartGame()
    {
        SceneManager.LoadScene(gameplaySceneName);
    }
}