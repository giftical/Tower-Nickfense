using UnityEngine;
using System.Collections;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public static ShopManager I { get; private set; }

    [Header("All towers that can appear in the shop")]
    [SerializeField] TowerData[] pool;

    [Header("Slots in UI (size 4)")]
    [SerializeField] ShopSlotUI[] slots;

    [Header("Auto Refresh")]
    [SerializeField] float refreshInterval = 10f;

    [Header("UI")]
    [SerializeField] TMP_Text refreshTimerText;

    float timer;
    Coroutine refreshRoutine;

    void Awake()
    {
        if (I != null && I != this)
        {
            Destroy(gameObject);
            return;
        }
        I = this;
    }

    void Start()
    {
        RollShop();
        timer = refreshInterval;
        refreshRoutine = StartCoroutine(ShopRefreshLoop());
    }

    void Update()
    {
        if (timer > 0f)
            timer -= Time.deltaTime;

        if (refreshTimerText != null)
            refreshTimerText.text = Mathf.Ceil(timer).ToString();
    }

    IEnumerator ShopRefreshLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(refreshInterval);
            RollShop();
            timer = refreshInterval;
        }
    }

    public void RollShop()
    {
        if (slots == null || slots.Length == 0) return;

        if (pool == null || pool.Length == 0)
        {
            foreach (var s in slots)
                if (s != null) s.Set(null);
            return;
        }

        var temp = (TowerData[])pool.Clone();
        int available = temp.Length;

        for (int i = 0; i < slots.Length; i++)
        {
            TowerData chosen = null;

            if (available > 0)
            {
                int idx = Random.Range(0, available);
                chosen = temp[idx];

                available--;
                temp[idx] = temp[available];
            }

            if (slots[i] != null)
                slots[i].Set(chosen);
        }
    }
}