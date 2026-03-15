using UnityEngine;

public class TutorialMenuController : MonoBehaviour
{
    [Header("Main Tutorial Panel")]
    [SerializeField] private GameObject tutorialPanel;

    [Header("Individual Tutorial Pages")]
    [SerializeField] private GameObject obchodPage;
    [SerializeField] private GameObject vezePage;
    [SerializeField] private GameObject penizePage;
    [SerializeField] private GameObject nepratelePage;
    [SerializeField] private GameObject infoTabulkyPage;
    [SerializeField] private GameObject konecHryPage;

    private void Start()
    {
        if (tutorialPanel)
            tutorialPanel.SetActive(false);

        HideAllPages();
    }

    private void HideAllPages()
    {
        if (obchodPage) obchodPage.SetActive(false);
        if (vezePage) vezePage.SetActive(false);
        if (penizePage) penizePage.SetActive(false);
        if (nepratelePage) nepratelePage.SetActive(false);
        if (infoTabulkyPage) infoTabulkyPage.SetActive(false);
        if (konecHryPage) konecHryPage.SetActive(false);
    }

    public void ToggleTutorial()
    {
        if (tutorialPanel == null) return;

        bool newState = !tutorialPanel.activeSelf;
        tutorialPanel.SetActive(newState);

        if (newState)
            ShowObchod();
        else
            HideAllPages();
    }

    public void ShowObchod()
    {
        HideAllPages();
        if (obchodPage) obchodPage.SetActive(true);
    }

    public void ShowVeze()
    {
        HideAllPages();
        if (vezePage) vezePage.SetActive(true);
    }

    public void ShowPenize()
    {
        HideAllPages();
        if (penizePage) penizePage.SetActive(true);
    }

    public void ShowNepratele()
    {
        HideAllPages();
        if (nepratelePage) nepratelePage.SetActive(true);
    }

    public void ShowInfoTabulky()
    {
        HideAllPages();
        if (infoTabulkyPage) infoTabulkyPage.SetActive(true);
    }

    public void ShowKonecHry()
    {
        HideAllPages();
        if (konecHryPage) konecHryPage.SetActive(true);
    }
}