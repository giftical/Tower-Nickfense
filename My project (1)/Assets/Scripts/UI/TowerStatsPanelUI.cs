using TMPro;
using UnityEngine;

public class TowerStatsPanelUI : MonoBehaviour
{
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text levelText;
    [SerializeField] TMP_Text dmgText;
    [SerializeField] TMP_Text atkSpdText;
    [SerializeField] TMP_Text rangeText;

    Tower current;

    void Awake()
    {
        Hide();
    }

    public void Show(Tower t)
    {
        current = t;
        gameObject.SetActive(true);
        Refresh();
    }

    public void Hide()
    {
        current = null;
        gameObject.SetActive(false);
    }

    void Update()
    {
        // Live update while selected (upgrades/traits)
        if (current != null)
            Refresh();
    }

    void Refresh()
    {
        if (current == null) { Hide(); return; }

        string n = (current.Data != null) ? current.Data.displayName : current.gameObject.name;

        nameText.text = n;
        levelText.text = $"Level: {current.Level}";
        dmgText.text = $"Damage: {current.Damage:0.##}";
        atkSpdText.text = $"Attack Speed: {current.AttackSpeed:0.##}";
        rangeText.text = $"Range: {current.Range:0.##}";
    }
}
