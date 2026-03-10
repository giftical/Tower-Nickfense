using UnityEngine;

public class Projectile : ProjectileBase
{
    [Header("Movement")]
    [SerializeField] float speed = 12f;
    [SerializeField] float maxLifetime = 4f;

    [Header("Visual alignment")]
    [SerializeField] Transform visual;
    [SerializeField] Vector3 visualEulerOffset = new Vector3(-90f, 0f, 0f);

    [Header("Optional slow effect")]
    [SerializeField] bool applySlow = false;
    [SerializeField, Range(0f, 1f)] float slowPercent = 0.2f;
    [SerializeField, Min(0.01f)] float slowDuration = 2f;

    IDamageable target;
    float damage;
    float life;

    public override void Init(IDamageable target, float damage)
    {
        this.target = target;
        this.damage = damage;

        if (this.target != null && !this.target.IsDead)
        {
            Vector3 dir = this.target.GetTransform().position - transform.position;
            if (dir.sqrMagnitude > 0.000001f)
            {
                transform.rotation = Quaternion.LookRotation(dir, Vector3.up);

                if (visual != null)
                    visual.localRotation = Quaternion.Euler(visualEulerOffset);
            }
        }
    }

    void Awake()
    {
        if (visual == null)
            visual = transform.childCount > 0 ? transform.GetChild(0) : null;

        if (visual != null)
            visual.localRotation = Quaternion.Euler(visualEulerOffset);
    }

    void Update()
    {
        life += Time.deltaTime;
        if (life >= maxLifetime)
        {
            Destroy(gameObject);
            return;
        }

        if (target == null || target.IsDead)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 tp = target.GetTransform().position;
        Vector3 dir = tp - transform.position;
        float step = speed * Time.deltaTime;

        if (dir.sqrMagnitude <= step * step)
        {
            transform.position = tp;
            Hit();
            return;
        }

        transform.position += dir.normalized * step;

        if (dir.sqrMagnitude > 0.000001f)
        {
            transform.rotation = Quaternion.LookRotation(dir, Vector3.up);

            if (visual != null)
                visual.localRotation = Quaternion.Euler(visualEulerOffset);
        }
    }

    void Hit()
    {
        if (target == null || target.IsDead)
        {
            Destroy(gameObject);
            return;
        }

        if (applySlow)
        {
            var status = target.GetTransform().GetComponentInParent<EnemyStatusEffects>();
            if (status == null)
                status = target.GetTransform().GetComponent<EnemyStatusEffects>();

            if (status != null)
            {
                float speedMultiplier = 1f - slowPercent;
                status.ApplyMoveSlow(speedMultiplier, slowDuration);
            }
        }

        target.TakeDamage(damage);
        Destroy(gameObject);
    }
}