using UnityEngine;

public class EncyclopediaPreviewManager : MonoBehaviour
{
    [SerializeField] private GameObject[] previews;

    public void ShowByIndex(int index)
    {
        if (previews == null || previews.Length == 0) return;
        if (index < 0 || index >= previews.Length) return;

        HideAll();

        if (previews[index] != null)
            previews[index].SetActive(true);
    }

    public void HideAll()
    {
        if (previews == null) return;

        for (int i = 0; i < previews.Length; i++)
        {
            if (previews[i] != null)
                previews[i].SetActive(false);
        }
    }
}