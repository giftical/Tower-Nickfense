using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance { get; private set; }

    [SerializeField] TowerData selected;
    bool pendingPaid;

    public TowerData Current => selected;
    public bool HasPendingPurchase => selected != null && pendingPaid;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void StartPurchase(TowerData data)
    {
        if (data == null)
        {
            Debug.LogWarning("[BuildManager] StartPurchase called with null data");
            return;
        }

        // cancel any previous pending purchase (with refund)
        CancelPurchase();

        if (EconomyManager.Instance == null)
        {
            Debug.LogError("[BuildManager] EconomyManager.Instance is null");
            return;
        }

        if (!EconomyManager.Instance.TrySpend(data.cost))
        {
            Debug.Log("[BuildManager] Not enough gold to buy " + data.displayName);
            return;
        }

        selected = data;
        pendingPaid = true;
        Debug.Log("[BuildManager] Purchased " + data.displayName + " (pending placement)");
    }

    public void OnPurchaseConsumed()
    {
        Debug.Log("[BuildManager] Purchase consumed");
        selected = null;
        pendingPaid = false;
    }

    public void CancelPurchase()
    {
        if (pendingPaid && selected != null && EconomyManager.Instance != null)
        {
            EconomyManager.Instance.AddGold(selected.cost);
            Debug.Log("[BuildManager] Refunded " + selected.displayName);
        }

        selected = null;
        pendingPaid = false;
    }
}
