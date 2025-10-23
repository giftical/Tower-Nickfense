// PlacementSystem.cs
using UnityEngine;
using UnityEngine.EventSystems;

public class PlacementSystem : MonoBehaviour
{
    [Header("Masks")]
    [SerializeField] LayerMask baseMask;
    [SerializeField] LayerMask blockedMask;

    [Header("Grid")]
    [SerializeField] bool snapToGrid = true;
    [SerializeField] float cellSize = 1f;

    [Header("Preview")]
    [SerializeField] Material okMat;
    [SerializeField] Material badMat;

    Camera cam;
    GameObject preview;
    Renderer[] previewRenderers;
    TowerData current;

    void Start() => cam = Camera.main;

    void Update()
    {
        if (EventSystem.current && EventSystem.current.IsPointerOverGameObject()) return;

        var sel = BuildManager.I.GetSelection();
        if (sel != current) SpawnOrSwapPreview(sel);

        if (!current) return;

        if (TryGetPlacementPoint(out var pos))
        {
            if (snapToGrid) pos = Snap(pos);
            preview.transform.position = pos;
            bool canPlace = CanPlaceAt(pos, current.footprintRadius);
            SetPreviewOK(canPlace);

            if (canPlace && Input.GetMouseButtonDown(0))
            {
                Place(pos);
            }
        }

        if (Input.GetMouseButtonDown(1)) CancelBuild();
    }

    void SpawnOrSwapPreview(TowerData sel)
    {
        current = sel;
        if (preview) Destroy(preview);
        if (!current) return;

        preview = Instantiate(current.prefab);
        foreach (var mb in preview.GetComponentsInChildren<MonoBehaviour>()) mb.enabled = false;
        foreach (var col in preview.GetComponentsInChildren<Collider>()) col.enabled = false;

        previewRenderers = preview.GetComponentsInChildren<Renderer>();
        SetPreviewOK(false);
    }

    bool TryGetPlacementPoint(out Vector3 point)
    {
        point = default;
        Ray r = cam.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(r, out var hit, 500f, baseMask)) return false;
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
        if (Physics.CheckSphere(pos, radius, blockedMask, QueryTriggerInteraction.Collide)) return false;
        return true;
    }

    void SetPreviewOK(bool ok)
    {
        if (previewRenderers == null) return;
        var mat = ok ? okMat : badMat;
        foreach (var r in previewRenderers) r.sharedMaterial = mat;
    }

    void Place(Vector3 pos)
    {
        var go = Instantiate(current.prefab, pos, preview.transform.rotation);
        go.layer = LayerMask.NameToLayer("Tower");
        foreach (var mb in go.GetComponentsInChildren<MonoBehaviour>()) mb.enabled = true;
    }

    void CancelBuild()
    {
        BuildManager.I.Clear();
        if (preview) Destroy(preview);
        preview = null; current = null; previewRenderers = null;
    }
}
