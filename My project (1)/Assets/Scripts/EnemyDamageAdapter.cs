using UnityEngine;

[RequireComponent(typeof(EnemyStats))]
public class EnemyDamageAdapter : MonoBehaviour, IDamageable
{
    EnemyStats stats;

    void Awake() => stats = GetComponent<EnemyStats>();

    public bool IsDead => stats.currentHealth <= 0f;

    public void TakeDamage(float amount)
    {
        if (IsDead) return;
        stats.currentHealth -= (int)amount;
        if (stats.currentHealth <= 0f) Die();
    }

    public Transform GetTransform() => transform;

    void Die()
    {
        // reward, VFX, etc.
        Destroy(gameObject);
    }
}
