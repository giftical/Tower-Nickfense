using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TowerHoverUISelect : MonoBehaviour
{
    [Header("Raycast")]
    [SerializeField] LayerMask towerMask;
    [SerializeField] float rayDistance = 1000f;

    [Header("Hover Tooltip UI (follows cursor)")]
    [SerializeField] RectTransform hoverTooltipRoot;
    [SerializeField] TMP_Text hoverText;
    [SerializeField] Vector2 hoverOffset = new Vector2(16f, -16f);

    [Header("Top Right Panel UI (click to open)")]
    [SerializeField] TowerStatsPanelUI statsPanel;

    Tower hovered;
    Tower selected;
    TowerHighlighter selectedHighlighter;

    Camera cam;

    void Awake()
    {
        cam = Camera.main;
        HideHover();
        statsPanel?.Hide();
    }

    void Update()
    {
        // Never show tower UI while placing/upgrading
        if (BuildManager.Instance != null && BuildManager.Instance.HasPendingPurchase)
        {
            hovered = null;
            HideHover();
            return;
        }

        // Don’t interact through UI
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
                // Clicked a tower: toggle selection
                if (hovered == selected)
                {
                    Deselect();
                }
                else
                {
                    Select(hovered);
                }
            }
            else
            {
                // Optional behavior: click empty space deselects
                // Comment out if you want selection to persist.
                Deselect();
            }
        }
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
        string name = (hovered.Data != null) ? hovered.Data.displayName : hovered.gameObject.name;
        hoverText.text = $"{name}\nLv {hovered.Level}";
    }

    Tower RaycastTowerUnderMouse()
    {
        Ray r = cam.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(r, out var hit, rayDistance, towerMask, QueryTriggerInteraction.Collide))
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
        // Unhighlight previous
        if (selectedHighlighter != null)
            selectedHighlighter.SetHighlighted(false);

        selected = t;
        selectedHighlighter = null;

        if (selected != null)
        {
            selectedHighlighter =
                selected.GetComponent<TowerHighlighter>() ??
                selected.GetComponentInChildren<TowerHighlighter>() ??
                selected.GetComponentInParent<TowerHighlighter>();

            if (selectedHighlighter != null)
                selectedHighlighter.SetHighlighted(true);

            statsPanel?.Show(selected);
        }
    }

    void Deselect()
    {
        if (selectedHighlighter != null)
            selectedHighlighter.SetHighlighted(false);

        selected = null;
        selectedHighlighter = null;

        statsPanel?.Hide();
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
