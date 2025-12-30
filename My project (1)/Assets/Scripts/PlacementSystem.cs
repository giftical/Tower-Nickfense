using UnityEngine;
using UnityEngine.EventSystems;

public class PlacementSystem : MonoBehaviour
{
    [Header("Masks")]
    [SerializeField] LayerMask baseMask;     // buildable tiles
    [SerializeField] LayerMask blockedMask;  // no-placement (Path, Enemy, Tower)
    [SerializeField] LayerMask towerMask;    // for upgrades

    [Header("Grid")]
    [SerializeField] bool snapToGrid = true;
    [SerializeField] float cellSize = 1f;

    [Header("Preview")]
    [SerializeField] Material okMat;
    [SerializeField] Material badMat;

    Camera cam;

    GameObject preview;
    Renderer[] previewRenderers;

    TowerData current;   // tower being placed

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (EventSystem.current && EventSystem.current.IsPointerOverGameObject())
            return;

        if (BuildManager.Instance == null)
            return;

        // Sync with BuildManager's selected tower
        var sel = BuildManager.Instance.Current;
        if (sel != current)
            SpawnOrSwapPreview(sel);

        // Only run placement when we actually bought a tower
        if (!BuildManager.Instance.HasPendingPurchase || !current)
            return;

        // Position preview on the ground
        if (TryGetPlacementPoint(out var pos))
        {
            if (snapToGrid)
                pos = Snap(pos);

            if (preview != null)
                preview.transform.position = pos;

            bool canPlace = CanPlaceAt(pos, current.footprintRadius);
            SetPreviewOK(canPlace);

            // LEFT CLICK
            if (Input.GetMouseButtonDown(0))
            {
                // (1) Upgrade if clicking on an identical existing tower
                if (TryUpgradeAtMouse())
                {
                    BuildManager.Instance.OnPurchaseConsumed();
                    ClearPreview();
                    return;
                }

                // (2) Place if valid tile
                if (canPlace)
                {
                    Place(pos);
                    BuildManager.Instance.OnPurchaseConsumed();
                    ClearPreview();
                    return;
                }
            }
        }

        // RIGHT CLICK = cancel + refund
        if (Input.GetMouseButtonDown(1))
        {
            BuildManager.Instance.CancelPurchase();
            ClearPreview();
        }
    }

    // ---------------------------
    // Preview logic
    // ---------------------------

    void SpawnOrSwapPreview(TowerData sel)
    {
        // Remove previous visual only (do NOT wipe current!)
        ClearPreview();

        current = sel;

        if (current == null || current.prefab == null)
            return;

        preview = Instantiate(current.prefab);

        // Disable tower behaviour in preview
        foreach (var mb in preview.GetComponentsInChildren<MonoBehaviour>())
            mb.enabled = false;

        foreach (var col in preview.GetComponentsInChildren<Collider>())
            col.enabled = false;

        previewRenderers = preview.GetComponentsInChildren<Renderer>();
        SetPreviewOK(false);
    }

    void ClearPreview()
    {
        if (preview != null)
            Destroy(preview);

        preview = null;
        previewRenderers = null;
        // IMPORTANT: we do NOT set current = null here
    }

    // ---------------------------
    // Placement helpers
    // ---------------------------

    bool TryGetPlacementPoint(out Vector3 point)
    {
        point = default;
        Ray r = cam.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(r, out var hit, 500f, baseMask))
            return false;

        point = hit.point;
        return true;
    }

    Vector3 Snap(Vector3 p)
    {
        p.x = Mathf.Round(p.x / cellSize) * cellSize;
        p.z = Mathf.Round(p.z / cellSize) * cellSize;
        return p;
    }

    bool CanPlaceAt(Vector3 pos, float radius)
    {
        return !Physics.CheckSphere(pos, radius, blockedMask, QueryTriggerInteraction.Collide);
    }

    void SetPreviewOK(bool ok)
    {
        if (previewRenderers == null) return;

        var mat = ok ? okMat : badMat;
        foreach (var r in previewRenderers)
            r.sharedMaterial = mat;
    }

    // ---------------------------
    // Actually placing a tower
    // ---------------------------

    void Place(Vector3 pos)
    {
        if (current == null || current.prefab == null)
            return;

        var go = Instantiate(current.prefab, pos, preview.transform.rotation);
        go.layer = LayerMask.NameToLayer("Tower");

        // Keep scripts disabled until data is assigned
        var mbs = go.GetComponentsInChildren<MonoBehaviour>();
        foreach (var mb in mbs)
            mb.enabled = false;

        var tower = go.GetComponentInChildren<Tower>();
        if (tower != null)
            tower.InitFromData(current);

        // Now enable everything (SynergyAgent will register with correct data)
        foreach (var mb in mbs)
            mb.enabled = true;
    }


    // ---------------------------
    // Upgrading existing tower
    // ---------------------------

    bool TryUpgradeAtMouse()
    {
        Ray r = cam.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(r, out var hit, 1000f, towerMask))
            return false;

        var tower = hit.collider.GetComponentInParent<Tower>();
        if (tower == null)
            return false;

        if (tower.Data != current)
            return false;

        return tower.TryUpgrade();
    }
}
