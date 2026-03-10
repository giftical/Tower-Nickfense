using UnityEngine;

[RequireComponent(typeof(EnemyStats))]
public class EnemyStatusEffects : MonoBehaviour
{
    EnemyStats stats;

    float activeSlowMultiplier = 1f;
    float slowTimer = 0f;

    void Awake()
    {
        stats = GetComponent<EnemyStats>();
    }

    void Update()
    {
        if (slowTimer > 0f)
        {
            slowTimer -= Time.deltaTime;

            if (slowTimer <= 0f)
            {
                slowTimer = 0f;
                activeSlowMultiplier = 1f;
                stats.ResetMoveSpeedMultiplier();
            }
        }
    }

    public void ApplyMoveSlow(float speedMultiplier, float duration)
    {
        speedMultiplier = Mathf.Clamp(speedMultiplier, 0.01f, 1f);
        duration = Mathf.Max(0.01f, duration);

        bool noSlowActive = slowTimer <= 0f;
        bool sameOrStrongerSlow = speedMultiplier <= activeSlowMultiplier;

        if (noSlowActive || sameOrStrongerSlow)
        {
            activeSlowMultiplier = speedMultiplier;
            stats.SetMoveSpeedMultiplier(activeSlowMultiplier);
        }

        slowTimer = duration;
    }
}