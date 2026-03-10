using UnityEngine;

[DisallowMultipleComponent]
public class EnemyStats : MonoBehaviour
{
    [Header("Movement")]
    [Min(0f)] public float moveSpeed = 3.5f;

    [Header("Vitals")]
    [Min(1)] public int maxHealth = 10;
    public int currentHealth;

    [Header("Rewards")]
    [Min(0)] public int goldOnDeath = 1;

    [Header("UI")]
    public string displayName = "Enemy";

    float moveSpeedMultiplier = 1f;

    public float CurrentMoveSpeed => moveSpeed * moveSpeedMultiplier;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void SetMoveSpeedMultiplier(float multiplier)
    {
        moveSpeedMultiplier = Mathf.Max(0.01f, multiplier);
    }

    public void ResetMoveSpeedMultiplier()
    {
        moveSpeedMultiplier = 1f;
    }
}