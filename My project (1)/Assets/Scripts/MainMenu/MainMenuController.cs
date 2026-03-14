using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject playPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject encyclopediaPanel;

    [Header("Encyclopedia Subpanels")]
    [SerializeField] private GameObject towerPanel;
    [SerializeField] private GameObject enemyPanel;

    [Header("Tower Pages (order matters)")]
    [SerializeField] private GameObject[] towerPages;

    [Header("Enemy Pages (order matters)")]
    [SerializeField] private GameObject[] enemyPages;

    [Header("Preview Managers")]
    [SerializeField] private EncyclopediaPreviewManager towerPreviewManager;
    [SerializeField] private EnemyEncyclopediaPreviewManager enemyPreviewManager;

    [Header("Gameplay Scene")]
    [SerializeField] private string gameplaySceneName = "GameScene";

    private int currentTowerPageIndex = 0;
    private int currentEnemyPageIndex = 0;

    private void Start()
    {
        if (mainPanel) mainPanel.SetActive(true);

        CloseAllPopups();
        CloseEncyclopediaSubpanels();
        HideAllTowerPages();
        HideAllEnemyPages();

        if (towerPreviewManager != null)
            towerPreviewManager.HideAll();

        if (enemyPreviewManager != null)
            enemyPreviewManager.HideAll();
    }

    private void CloseAllPopups()
    {
        if (playPanel) playPanel.SetActive(false);
        if (settingsPanel) settingsPanel.SetActive(false);
        if (encyclopediaPanel) encyclopediaPanel.SetActive(false);
    }

    private void CloseEncyclopediaSubpanels()
    {
        if (towerPanel) towerPanel.SetActive(false);
        if (enemyPanel) enemyPanel.SetActive(false);
    }

    private void HideAllTowerPages()
    {
        if (towerPages == null) return;

        for (int i = 0; i < towerPages.Length; i++)
        {
            if (towerPages[i] != null)
                towerPages[i].SetActive(false);
        }
    }

    private void HideAllEnemyPages()
    {
        if (enemyPages == null) return;

        for (int i = 0; i < enemyPages.Length; i++)
        {
            if (enemyPages[i] != null)
                enemyPages[i].SetActive(false);
        }
    }

    private void ShowTowerPage(int index)
    {
        if (towerPages == null || towerPages.Length == 0) return;
        if (index < 0 || index >= towerPages.Length) return;

        HideAllTowerPages();
        currentTowerPageIndex = index;

        if (towerPages[currentTowerPageIndex] != null)
            towerPages[currentTowerPageIndex].SetActive(true);

        if (towerPreviewManager != null)
            towerPreviewManager.ShowByIndex(currentTowerPageIndex);
    }

    private void ShowEnemyPage(int index)
    {
        if (enemyPages == null || enemyPages.Length == 0) return;
        if (index < 0 || index >= enemyPages.Length) return;

        HideAllEnemyPages();
        currentEnemyPageIndex = index;

        if (enemyPages[currentEnemyPageIndex] != null)
            enemyPages[currentEnemyPageIndex].SetActive(true);

        if (enemyPreviewManager != null)
            enemyPreviewManager.ShowByIndex(currentEnemyPageIndex);
    }

    public void OnPlayPressed()
    {
        CloseAllPopups();
        CloseEncyclopediaSubpanels();
        HideAllTowerPages();
        HideAllEnemyPages();

        if (towerPreviewManager != null)
            towerPreviewManager.HideAll();

        if (enemyPreviewManager != null)
            enemyPreviewManager.HideAll();

        if (playPanel) playPanel.SetActive(true);
    }

    public void OnSettingsPressed()
    {
        CloseAllPopups();
        CloseEncyclopediaSubpanels();
        HideAllTowerPages();
        HideAllEnemyPages();

        if (towerPreviewManager != null)
            towerPreviewManager.HideAll();

        if (enemyPreviewManager != null)
            enemyPreviewManager.HideAll();

        if (settingsPanel) settingsPanel.SetActive(true);
    }

    public void OnEncyclopediaPressed()
    {
        CloseAllPopups();
        CloseEncyclopediaSubpanels();
        HideAllTowerPages();
        HideAllEnemyPages();

        if (towerPreviewManager != null)
            towerPreviewManager.HideAll();

        if (enemyPreviewManager != null)
            enemyPreviewManager.HideAll();

        if (encyclopediaPanel) encyclopediaPanel.SetActive(true);
    }

    public void OnTowerButtonPressed()
    {
        if (towerPanel) towerPanel.SetActive(true);
        if (enemyPanel) enemyPanel.SetActive(false);

        HideAllEnemyPages();

        if (enemyPreviewManager != null)
            enemyPreviewManager.HideAll();

        currentTowerPageIndex = 0;
        ShowTowerPage(currentTowerPageIndex);
    }

    public void OnEnemyButtonPressed()
    {
        if (enemyPanel) enemyPanel.SetActive(true);
        if (towerPanel) towerPanel.SetActive(false);

        HideAllTowerPages();

        if (towerPreviewManager != null)
            towerPreviewManager.HideAll();

        currentEnemyPageIndex = 0;
        ShowEnemyPage(currentEnemyPageIndex);
    }

    public void OnTowerNextPressed()
    {
        if (towerPages == null || towerPages.Length == 0) return;

        currentTowerPageIndex++;
        if (currentTowerPageIndex >= towerPages.Length)
            currentTowerPageIndex = 0;

        ShowTowerPage(currentTowerPageIndex);
    }

    public void OnTowerPreviousPressed()
    {
        if (towerPages == null || towerPages.Length == 0) return;

        currentTowerPageIndex--;
        if (currentTowerPageIndex < 0)
            currentTowerPageIndex = towerPages.Length - 1;

        ShowTowerPage(currentTowerPageIndex);
    }

    public void OnEnemyNextPressed()
    {
        if (enemyPages == null || enemyPages.Length == 0) return;

        currentEnemyPageIndex++;
        if (currentEnemyPageIndex >= enemyPages.Length)
            currentEnemyPageIndex = 0;

        ShowEnemyPage(currentEnemyPageIndex);
    }

    public void OnEnemyPreviousPressed()
    {
        if (enemyPages == null || enemyPages.Length == 0) return;

        currentEnemyPageIndex--;
        if (currentEnemyPageIndex < 0)
            currentEnemyPageIndex = enemyPages.Length - 1;

        ShowEnemyPage(currentEnemyPageIndex);
    }

    public void OnCloseTowerPanelPressed()
    {
        HideAllTowerPages();

        if (towerPanel) towerPanel.SetActive(false);

        if (towerPreviewManager != null)
            towerPreviewManager.HideAll();
    }

    public void OnCloseEnemyPanelPressed()
    {
        HideAllEnemyPages();

        if (enemyPanel) enemyPanel.SetActive(false);

        if (enemyPreviewManager != null)
            enemyPreviewManager.HideAll();
    }

    public void OnStartGamePressed()
    {
        SceneManager.LoadScene(gameplaySceneName);
    }

    public void OnClosePopupPressed()
    {
        CloseAllPopups();
        CloseEncyclopediaSubpanels();
        HideAllTowerPages();
        HideAllEnemyPages();

        if (towerPreviewManager != null)
            towerPreviewManager.HideAll();

        if (enemyPreviewManager != null)
            enemyPreviewManager.HideAll();
    }

    public void OnQuitPressed()
    {
        Application.Quit();
    }
}