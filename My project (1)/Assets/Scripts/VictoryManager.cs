using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryManager : MonoBehaviour
{
    public static VictoryManager I { get; private set; }

    [Header("UI")]
    [SerializeField] private GameObject victoryPopup;

    [Header("Scene")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    bool hasWon = false;

    public bool HasWon => hasWon;

    void Awake()
    {
        if (I != null && I != this)
        {
            Destroy(gameObject);
            return;
        }

        I = this;
        Time.timeScale = 1f;
    }

    void Start()
    {
        if (victoryPopup != null)
            victoryPopup.SetActive(false);
    }

    public void Win()
    {
        if (hasWon) return;
        hasWon = true;

        if (victoryPopup != null)
            victoryPopup.SetActive(true);

        Time.timeScale = 0f;
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}