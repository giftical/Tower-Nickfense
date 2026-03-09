using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EncyclopediaController : MonoBehaviour
{
    [System.Serializable]
    public class TowerEncyclopediaEntry
    {
        public string displayName;

        [TextArea(3, 8)]
        public string description;

        public GameObject previewPrefab;
    }

    [Header("Panels")]
    [SerializeField] private GameObject categoryPanel;
    [SerializeField] private GameObject towersPanel;
    [SerializeField] private GameObject enemiesPanel;

    [Header("Tower Entries")]
    [SerializeField] private List<TowerEncyclopediaEntry> towerEntries = new();

    [Header("Tower UI")]
    [SerializeField] private TMP_Text towerNameText;
    [SerializeField] private TMP_Text towerDescriptionText;

    [Header("3D Preview")]
    [SerializeField] private Transform previewSpawnPoint;

    private int currentTowerIndex = 0;
    private GameObject currentPreviewInstance;

    private void Start()
    {
        ShowCategoryPanel();
    }

    public void ShowCategoryPanel()
    {
        if (categoryPanel) categoryPanel.SetActive(true);
        if (towersPanel) towersPanel.SetActive(false);
        if (enemiesPanel) enemiesPanel.SetActive(false);

        ClearPreview();
    }

    public void OpenTowers()
    {
        if (categoryPanel) categoryPanel.SetActive(false);
        if (towersPanel) towersPanel.SetActive(true);
        if (enemiesPanel) enemiesPanel.SetActive(false);

        if (towerEntries.Count == 0)
        {
            if (towerNameText) towerNameText.text = "No Tower";
            if (towerDescriptionText) towerDescriptionText.text = "No tower entries assigned.";
            ClearPreview();
            return;
        }

        currentTowerIndex = Mathf.Clamp(currentTowerIndex, 0, towerEntries.Count - 1);
        RefreshTowerView();
    }

    public void OpenEnemies()
    {
        if (categoryPanel) categoryPanel.SetActive(false);
        if (towersPanel) towersPanel.SetActive(false);
        if (enemiesPanel) enemiesPanel.SetActive(true);

        ClearPreview();
    }

    public void ShowNextTower()
    {
        if (towerEntries.Count == 0) return;

        currentTowerIndex++;
        if (currentTowerIndex >= towerEntries.Count)
            currentTowerIndex = 0;

        RefreshTowerView();
    }

    public void ShowPreviousTower()
    {
        if (towerEntries.Count == 0) return;

        currentTowerIndex--;
        if (currentTowerIndex < 0)
            currentTowerIndex = towerEntries.Count - 1;

        RefreshTowerView();
    }

    private void RefreshTowerView()
    {
        TowerEncyclopediaEntry entry = towerEntries[currentTowerIndex];

        if (towerNameText)
            towerNameText.text = entry.displayName;

        if (towerDescriptionText)
            towerDescriptionText.text = entry.description;

        SpawnPreview(entry.previewPrefab);
    }

    private void SpawnPreview(GameObject prefab)
    {
        ClearPreview();

        if (prefab == null || previewSpawnPoint == null)
            return;

        currentPreviewInstance = Instantiate(
            prefab,
            previewSpawnPoint.position,
            Quaternion.identity,
            previewSpawnPoint
        );

        currentPreviewInstance.transform.localPosition = Vector3.zero;
        currentPreviewInstance.transform.localRotation = Quaternion.identity;

        if (currentPreviewInstance.GetComponent<EncyclopediaPreviewSpinner>() == null)
            currentPreviewInstance.AddComponent<EncyclopediaPreviewSpinner>();
    }

    private void ClearPreview()
    {
        if (currentPreviewInstance != null)
        {
            Destroy(currentPreviewInstance);
            currentPreviewInstance = null;
        }
    }
}