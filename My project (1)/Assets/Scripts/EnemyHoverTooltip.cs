using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class EnemyHoverTooltip : MonoBehaviour
{
    [Header("Raycast")]
    [SerializeField] LayerMask enemyMask;
    [SerializeField] float rayDistance = 1000f;

    [Header("UI")]
    [SerializeField] RectTransform tooltipRoot;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text hpText;

    [Header("Follow Cursor")]
    [SerializeField] Vector2 screenOffset = new Vector2(16f, -16f);
    [SerializeField] bool clampToScreen = true;
    [SerializeField] Vector2 screenPadding = new Vector2(12f, 12f);

    Camera cam;
    EnemyStats currentStats;

    void Awake()
    {
        cam = Camera.main;
        Hide();
    }

    void Update()
    {
        if (EventSystem.current && EventSystem.current.IsPointerOverGameObject())
        {
            Hide();
            return;
        }

        if (BuildManager.Instance != null && BuildManager.Instance.HasPendingPurchase)
        {
            Hide();
            return;
        }

        UpdateHoverTarget();
        UpdateTooltipPosition();

        if (currentStats != null)
            UpdateTooltipText();
    }

    void UpdateHoverTarget()
    {
        Ray r = cam.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(r, out var hit, rayDistance, enemyMask, QueryTriggerInteraction.Collide))
        {
            currentStats = null;
            Hide();
            return;
        }

        var stats = hit.collider.GetComponentInParent<EnemyStats>();
        var dmg = hit.collider.GetComponentInParent<IDamageable>();

        if (stats == null || dmg == null || dmg.IsDead)
        {
            currentStats = null;
            Hide();
            return;
        }

        currentStats = stats;
        Show();
    }

    void UpdateTooltipText()
    {
        string n = !string.IsNullOrWhiteSpace(currentStats.displayName)
            ? currentStats.displayName
            : currentStats.gameObject.name;

        nameText.text = n;
        hpText.text = $"{currentStats.currentHealth} / {currentStats.maxHealth}";
    }

    void UpdateTooltipPosition()
    {
        if (!tooltipRoot.gameObject.activeSelf)
            return;

        Vector2 pos = (Vector2)Input.mousePosition + screenOffset;

        if (clampToScreen)
        {
            Vector2 size = tooltipRoot.sizeDelta;

            float minX = screenPadding.x;
            float minY = screenPadding.y;
            float maxX = Screen.width - screenPadding.x - size.x;
            float maxY = Screen.height - screenPadding.y - size.y;

            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            pos.y = Mathf.Clamp(pos.y, minY, maxY);
        }

        tooltipRoot.position = pos;
    }

    void Show()
    {
        if (!tooltipRoot.gameObject.activeSelf)
            tooltipRoot.gameObject.SetActive(true);
    }

    void Hide()
    {
        if (tooltipRoot != null && tooltipRoot.gameObject.activeSelf)
            tooltipRoot.gameObject.SetActive(false);
    }
}
