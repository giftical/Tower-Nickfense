using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 12f;
    [SerializeField] float maxLifetime = 4f;

    IDamageable target;
    float damage;
    float life;

    public void Init(IDamageable target, float damage)
    {
        this.target = target;
        this.damage = damage;
    }

    void Update()
    {
        life += Time.deltaTime;
        if (life >= maxLifetime) { Destroy(gameObject); return; }
        if (target == null || target.IsDead) { Destroy(gameObject); return; }

        Vector3 tp = target.GetTransform().position;
        Vector3 dir = tp - transform.position;
        float step = speed * Time.deltaTime;

        if (dir.sqrMagnitude <= step * step) { Hit(); return; }

        transform.position += dir.normalized * step;
        if (dir != Vector3.zero) transform.rotation = Quaternion.LookRotation(dir);
    }

    void Hit()
    {
        target.TakeDamage(damage);
        Destroy(gameObject);
    }
}
