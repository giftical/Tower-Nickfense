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

    [Header("Preview Materials")]
    [SerializeField] Material okMat;       // green
    [SerializeField] Material badMat;      // red
    [SerializeField] Material upgradeMat;  // yellow

    [Header("Upgrade Input")]
    [SerializeField] KeyCode upgradeKey = KeyCode.E;

    Camera cam;

    GameObject preview;
    Renderer[] previewRenderers;

    TowerData current;   // tower being placed

    enum PlacementState
    {
        Invalid,
        PlaceValid,
        UpgradeValid
    }

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


        var sel = BuildManager.Instance.Current;
        if (sel != current)
            SpawnOrSwapPreview(sel);

        // only run placement when tower actually bought
        if (!BuildManager.Instance.HasPendingPurchase || !current)
            return;

        bool holdingUpgrade = Input.GetKey(upgradeKey);

        Tower upgradeTarget = null;
        bool canUpgrade = false;

        if (holdingUpgrade && TryGetUpgradeTarget(out upgradeTarget))
        {
            if (upgradeTarget != null && upgradeTarget.Data == current)
            {
                canUpgrade = true;

                var pos = upgradeTarget.transform.position;
                if (preview != null)
                    preview.transform.position = pos;

                SetPreviewState(PlacementState.UpgradeValid);
            }
            else
            {
                if (upgradeTarget != null && preview != null)
                    preview.transform.position = upgradeTarget.transform.position;

                SetPreviewState(PlacementState.Invalid);
            }
        }

        bool canPlace = false;
        Vector3 placePos = default;

        if (!canUpgrade)
        {
            if (TryGetPlacementPoint(out var pos))
            {
                if (snapToGrid)
                    pos = Snap(pos);

                placePos = pos;

                if (preview != null)
                    preview.transform.position = pos;

                canPlace = CanPlaceAt(pos, current.footprintRadius);
                SetPreviewState(canPlace ? PlacementState.PlaceValid : PlacementState.Invalid);
            }
            else
            {
                SetPreviewState(PlacementState.Invalid);
            }
        }


        if (Input.GetMouseButtonDown(0))
        {
            if (canUpgrade && upgradeTarget != null)
            {
                if (upgradeTarget.TryUpgrade())
                {
                    BuildManager.Instance.OnPurchaseConsumed();
                    ClearPreview();
                    return;
                }
            }

            // Otherwise normal placement
            if (!canUpgrade && canPlace)
            {
                Place(placePos);
                BuildManager.Instance.OnPurchaseConsumed();
                ClearPreview();
                return;
            }
        }

        // RIGHT CLICK = cancel + refund
        if (Input.GetMouseButtonDown(1))
        {
            BuildManager.Instance.CancelPurchase();
            ClearPreview();
        }
    }

    void SpawnOrSwapPreview(TowerData sel)
    {
        ClearPreview();

        current = sel;

        if (current == null || current.prefab == null)
            return;

        preview = Instantiate(current.prefab);

        foreach (var mb in preview.GetComponentsInChildren<MonoBehaviour>())
            mb.enabled = false;

        foreach (var col in preview.GetComponentsInChildren<Collider>())
            col.enabled = false;

        previewRenderers = preview.GetComponentsInChildren<Renderer>();
        SetPreviewState(PlacementState.Invalid);
    }

    void ClearPreview()
    {
        if (preview != null)
            Destroy(preview);

        preview = null;
        previewRenderers = null;
    }

    void SetPreviewState(PlacementState state)
    {
        if (previewRenderers == null) return;

        Material mat = badMat;

        switch (state)
        {
            case PlacementState.PlaceValid:
                mat = okMat;
                break;
            case PlacementState.UpgradeValid:
                mat = upgradeMat != null ? upgradeMat : okMat; // fallback if not assigned
                break;
            default:
                mat = badMat;
                break;
        }

        foreach (var r in previewRenderers)
            r.material = mat;

    }

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

    bool TryGetUpgradeTarget(out Tower tower)
    {
        tower = null;

        Ray r = cam.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(r, out var hit, 1000f, towerMask))
            return false;

        tower = hit.collider.GetComponentInParent<Tower>();
        return tower != null;
    }
}
