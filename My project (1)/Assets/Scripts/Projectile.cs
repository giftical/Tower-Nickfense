using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 12f;
    [SerializeField] float maxLifetime = 4f;

    [Header("Visual alignment")]
    [SerializeField] Transform visual;
    [SerializeField] Vector3 visualEulerOffset = new Vector3(-90f, 0f, 0f);

    IDamageable target;
    float damage;
    float life;

    public void Init(IDamageable target, float damage)
    {
        this.target = target;
        this.damage = damage;

        // FIX: set correct rotation immediately on spawn (prevents 1-frame "wrong" orientation)
        if (this.target != null && !this.target.IsDead)
        {
            Vector3 dir = this.target.GetTransform().position - transform.position;
            if (dir.sqrMagnitude > 0.000001f)
            {
                transform.rotation = Quaternion.LookRotation(dir, Vector3.up);

                // keep the model aligned right away too
                if (visual != null)
                    visual.localRotation = Quaternion.Euler(visualEulerOffset);
            }
        }
    }

    void Awake()
    {
        if (visual == null)
            visual = transform.childCount > 0 ? transform.GetChild(0) : null;

        // FIX: ensure visual offset is applied even before first Update
        if (visual != null)
            visual.localRotation = Quaternion.Euler(visualEulerOffset);
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

        if (dir.sqrMagnitude > 0.000001f)
        {
            // Face movement direction
            transform.rotation = Quaternion.LookRotation(dir, Vector3.up);

            // Keep the mesh aligned to forward
            if (visual != null)
                visual.localRotation = Quaternion.Euler(visualEulerOffset);
        }
    }

    void Hit()
    {
        target.TakeDamage(damage);
        Destroy(gameObject);
    }
}
