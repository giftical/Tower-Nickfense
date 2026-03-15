using System.Collections.Generic;
using UnityEngine;

public class BomberProjectile : ProjectileBase
{
    [Header("Movement")]
    [SerializeField, Min(0.1f)] float speed = 10f;
    [SerializeField, Min(0.1f)] float maxLifetime = 4f;

    [Header("Mortar Launch")]
    [SerializeField, Min(0f)] float launchUpwardBias = 1.25f;
    [SerializeField, Min(0f)] float launchForwardBias = 0.35f;
    [SerializeField, Min(0f)] float launchDuration = 0.35f;

    [Header("Homing")]
    [SerializeField, Min(1f)] float turnSpeedDegrees = 360f;

    [Header("Explosion")]
    [SerializeField, Min(0.1f)] float explosionRadius = 2.5f;
    [SerializeField] LayerMask enemyMask;

    [Header("Visual alignment")]
    [SerializeField] Transform visual;
    [SerializeField] Vector3 visualEulerOffset = new Vector3(-90f, 0f, 0f);

    IDamageable target;
    float damage;
    float life;
    float phaseTimer;

    Vector3 moveDir;
    bool initialized;

    public override void Init(IDamageable target, float damage)
    {
        this.target = target;
        this.damage = damage;

        Vector3 forward = transform.forward;
        if (forward.sqrMagnitude < 0.0001f)
            forward = Vector3.forward;

        Vector3 launchDir = (forward * launchForwardBias + Vector3.up * launchUpwardBias).normalized;

        if (target != null && !target.IsDead)
        {
            Vector3 toTarget = target.GetTransform().position - transform.position;
            Vector3 flatToTarget = Vector3.ProjectOnPlane(toTarget, Vector3.up).normalized;

            if (flatToTarget.sqrMagnitude > 0.0001f)
                launchDir = (flatToTarget * launchForwardBias + Vector3.up * launchUpwardBias).normalized;
        }

        moveDir = launchDir;
        initialized = true;

        if (moveDir.sqrMagnitude > 0.0001f)
        {
            transform.rotation = Quaternion.LookRotation(moveDir, Vector3.up);

            if (visual != null)
                visual.localRotation = Quaternion.Euler(visualEulerOffset);
        }
    }

    void Awake()
    {
        if (visual == null && transform.childCount > 0)
            visual = transform.GetChild(0);

        if (visual != null)
            visual.localRotation = Quaternion.Euler(visualEulerOffset);
    }

    void Update()
    {
        life += Time.deltaTime;
        phaseTimer += Time.deltaTime;

        if (life >= maxLifetime)
        {
            Destroy(gameObject);
            return;
        }

        if (!initialized)
            return;

        if (target == null || target.IsDead)
        {
            Destroy(gameObject);
            return;
        }

        if (phaseTimer > launchDuration)
            HomeTowardsTarget();

        float step = speed * Time.deltaTime;
        Vector3 toTarget = target.GetTransform().position - transform.position;

        if (toTarget.sqrMagnitude <= step * step)
        {
            transform.position = target.GetTransform().position;
            Explode();
            return;
        }

        transform.position += moveDir * step;
        UpdateVisualRotation();
    }

    void HomeTowardsTarget()
    {
        if (target == null || target.IsDead)
            return;

        Vector3 toTarget = target.GetTransform().position - transform.position;
        if (toTarget.sqrMagnitude < 0.0001f)
            return;

        Vector3 desiredDir = toTarget.normalized;
        float maxRadiansDelta = turnSpeedDegrees * Mathf.Deg2Rad * Time.deltaTime;

        moveDir = Vector3.RotateTowards(moveDir, desiredDir, maxRadiansDelta, 0f).normalized;
    }

    void UpdateVisualRotation()
    {
        if (moveDir.sqrMagnitude < 0.0001f)
            return;

        transform.rotation = Quaternion.LookRotation(moveDir, Vector3.up);

        if (visual != null)
            visual.localRotation = Quaternion.Euler(visualEulerOffset);
    }

    void Explode()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius, enemyMask);
        HashSet<IDamageable> damaged = new HashSet<IDamageable>();

        foreach (var hit in hits)
        {
            var dmg = hit.GetComponentInParent<IDamageable>() ?? hit.GetComponent<IDamageable>();
            if (dmg == null || dmg.IsDead) continue;
            if (!damaged.Add(dmg)) continue;

            dmg.TakeDamage(damage);
        }

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}