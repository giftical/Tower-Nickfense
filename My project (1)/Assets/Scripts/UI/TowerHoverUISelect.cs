using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TowerHoverAndSelectUI : MonoBehaviour
{
    [Header("Raycast")]
    [SerializeField] LayerMask towerMask;
    [SerializeField] float rayDistance = 1000f;

    [Header("Hover Tooltip UI")]
    [SerializeField] RectTransform hoverTooltipRoot;
    [SerializeField] TMP_Text hoverText;
    [SerializeField] Vector2 hoverOffset = new Vector2(16f, -16f);

    [Header("Top Right Stats Panel")]
    [SerializeField] TowerStatsPanelUI statsPanel;

    [Header("Range Preview Ring")]
    [SerializeField] TowerRangeRing rangeRing;

    Tower hovered;
    Tower selected;
    TowerHighlighter selectedHighlighter;

    Camera cam;

    void Awake()
    {
        cam = Camera.main;
        HideHover();

        statsPanel?.Hide();
        rangeRing?.Hide();
    }

    void Update()
    {
        if (BuildManager.Instance != null && BuildManager.Instance.HasPendingPurchase)
        {
            hovered = null;
            HideHover();
            return;
        }

        if (EventSystem.current && EventSystem.current.IsPointerOverGameObject())
        {
            HideHover();
            return;
        }

        UpdateHover();
        UpdateHoverPosition();

        if (Input.GetMouseButtonDown(0))
        {
            if (hovered != null)
            {
                if (hovered == selected)
                    Deselect();
                else
                    Select(hovered);
            }
        }

        // Live update (upgrades / synergies)
        if (selected != null && rangeRing != null)
            rangeRing.ShowAt(selected.transform.position, selected.Range);
    }

    void UpdateHover()
    {
        hovered = RaycastTowerUnderMouse();

        if (hovered == null)
        {
            HideHover();
            return;
        }

        ShowHover();
        string name = hovered.Data != null ? hovered.Data.displayName : hovered.gameObject.name;
        hoverText.text = $"{name}\nLv {hovered.Level}";
    }

    Tower RaycastTowerUnderMouse()
    {
        Ray r = cam.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(r, out var hit, rayDistance, towerMask))
            return null;

        return hit.collider.GetComponentInParent<Tower>();
    }

    void UpdateHoverPosition()
    {
        if (hoverTooltipRoot == null || !hoverTooltipRoot.gameObject.activeSelf)
            return;

        hoverTooltipRoot.position = (Vector2)Input.mousePosition + hoverOffset;
    }

    void Select(Tower t)
    {
        if (selectedHighlighter != null)
            selectedHighlighter.SetHighlighted(false);

        selected = t;

        selectedHighlighter =
            selected.GetComponent<TowerHighlighter>() ??
            selected.GetComponentInChildren<TowerHighlighter>();

        if (selectedHighlighter != null)
            selectedHighlighter.SetHighlighted(true);

        statsPanel?.Show(selected);
        rangeRing?.ShowAt(selected.transform.position, selected.Range);
    }

    void Deselect()
    {
        if (selectedHighlighter != null)
            selectedHighlighter.SetHighlighted(false);

        selected = null;
        selectedHighlighter = null;

        statsPanel?.Hide();
        rangeRing?.Hide();
    }

    void ShowHover()
    {
        if (hoverTooltipRoot != null && !hoverTooltipRoot.gameObject.activeSelf)
            hoverTooltipRoot.gameObject.SetActive(true);
    }

    void HideHover()
    {
        if (hoverTooltipRoot != null && hoverTooltipRoot.gameObject.activeSelf)
            hoverTooltipRoot.gameObject.SetActive(false);
    }
}
