using System;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance { get; private set; }

    [Header("Config")]
    [SerializeField] int startingGoldValue = 20;

    int currentGold;
    public int Gold => currentGold;

    public event Action<int> onGoldChanged;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        currentGold = startingGoldValue;
        onGoldChanged?.Invoke(currentGold);
        Debug.Log($"[Economy] Starting gold = {currentGold}");
    }

    public bool TrySpend(int amount)
    {
        if (amount <= 0) return true;

        if (currentGold < amount)
        {
            Debug.Log($"[Economy] Not enough gold. Have {currentGold}, need {amount}");
            return false;
        }

        currentGold -= amount;
        onGoldChanged?.Invoke(currentGold);
        Debug.Log($"[Economy] Spent {amount}, now {currentGold}");
        return true;
    }

    public void AddGold(int amount)
    {
        if (amount <= 0) return;

        currentGold += amount;
        onGoldChanged?.Invoke(currentGold);
        Debug.Log($"[Economy] Gained {amount}, now {currentGold}");
    }
}
