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

    void Awake()
    {
        currentHealth = maxHealth;
    }
}
