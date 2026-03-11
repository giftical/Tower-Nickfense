using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth I { get; private set; }

    [Header("Health")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth = 100;

    [Header("UI")]
    [SerializeField] private TMP_Text healthText;

    [Header("Defeat Popup")]
    [SerializeField] private GameObject defeatPopup;
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    bool isDefeated;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    void Awake()
    {
        if (I != null && I != this)
        {
            Destroy(gameObject);
            return;
        }

        I = this;
        currentHealth = maxHealth;
        Time.timeScale = 1f;
    }

    void Start()
    {
        RefreshUI();

        if (defeatPopup != null)
            defeatPopup.SetActive(false);
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0 || isDefeated)
            return;

        currentHealth -= amount;

        if (currentHealth < 0)
            currentHealth = 0;

        RefreshUI();

        if (currentHealth <= 0)
            Defeat();
    }

    void RefreshUI()
    {
        if (healthText != null)
            healthText.text = $"Health: {currentHealth}";
    }

    void Defeat()
    {
        if (isDefeated)
            return;

        isDefeated = true;

        if (defeatPopup != null)
            defeatPopup.SetActive(true);

        Time.timeScale = 0f;
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}