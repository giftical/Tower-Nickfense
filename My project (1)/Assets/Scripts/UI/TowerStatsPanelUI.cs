// TowerStatsPanelUI.cs (FULL, CanvasGroup version) — unchanged, included for completeness.
// Keep your existing working version if you already have it.
using TMPro;
using UnityEngine;

public class TowerStatsPanelUI : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;

    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text levelText;
    [SerializeField] TMP_Text dmgText;
    [SerializeField] TMP_Text atkSpdText;
    [SerializeField] TMP_Text rangeText;

    Tower current;

    void Awake()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        Hide();
    }

    public void Show(Tower t)
    {
        current = t;
        transform.SetAsLastSibling();
        SetVisible(true);
        Refresh();
    }

    public void Hide()
    {
        current = null;
        SetVisible(false);
    }

    void Update()
    {
        if (current != null)
            Refresh();
    }

    void Refresh()
    {
        if (current == null) return;

        string n = (current.Data != null) ? current.Data.displayName : current.gameObject.name;

        nameText.text = n;
        levelText.text = $"Level: {current.Level}";
        dmgText.text = $"Damage: {current.Damage:0.##}";
        atkSpdText.text = $"Attack Speed: {current.AttackSpeed:0.##}";
        rangeText.text = $"Range: {current.Range:0.##}";
    }

    void SetVisible(bool on)
    {
        if (canvasGroup == null)
        {
            Debug.LogError("[TowerStatsPanelUI] Missing CanvasGroup on stats panel root.");
            return;
        }

        canvasGroup.alpha = on ? 1f : 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
