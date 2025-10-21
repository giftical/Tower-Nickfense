using UnityEngine;

[DisallowMultipleComponent]
public class EnemyStats : MonoBehaviour
{
    [Header("Movement")]
    [Min(0f)] public float moveSpeed = 3.5f;

    [Header("Vitals")]
    [Min(1)] public int maxHealth = 10;
    public int currentHealth;

    void Awake() => currentHealth = maxHealth;

    public void TakeDamage(int amount)
    {
        if (currentHealth <= 0) return;
        currentHealth = Mathf.Max(0, currentHealth - amount);
        if (currentHealth == 0) Destroy(gameObject);
    }
}
