using UnityEngine;

public class TowerSynergyAgent : MonoBehaviour
{
    public Tower Tower { get; private set; }

    void Awake()
    {
        // Tower is often on a child (your PlacementSystem uses GetComponentInChildren<Tower>())
        Tower = GetComponent<Tower>();
        if (Tower == null)
            Tower = GetComponentInChildren<Tower>(true);

        if (Tower == null)
            Debug.LogError("[TowerSynergyAgent] No Tower found on this prefab (root or children).", this);
    }

    void OnEnable()
    {
        SynergyManager.Instance?.Register(this);
    }

    void OnDisable()
    {
        SynergyManager.Instance?.Unregister(this);
    }
}
