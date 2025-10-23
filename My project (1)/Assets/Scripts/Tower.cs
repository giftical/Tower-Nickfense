using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField, Min(0.05f)] float attackSpeed = 1f;
    [SerializeField, Min(0f)] float damage = 2f;
    [SerializeField, Min(0.1f)] float range = 5f;

    [Header("Targeting")]
    [SerializeField] LayerMask enemyMask;
    [SerializeField] Transform firePoint;
    [SerializeField] Projectile projectilePf;

    float cd;

    void Update()
    {
        cd -= Time.deltaTime;
        if (cd > 0f) return;

        var target = AcquireTarget();
        if (target == null) return;

        Shoot(target);
        cd = 1f / attackSpeed;
    }

    IDamageable AcquireTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, range, enemyMask);
        IDamageable best = null;
        float bestProg = -1f;
        float fallbackBestSqr = float.PositiveInfinity;

        for (int i = 0; i < hits.Length; i++)
        {
            var dmg = hits[i].GetComponentInParent<IDamageable>() ?? hits[i].GetComponent<IDamageable>();
            if (dmg == null || dmg.IsDead) continue;

            var prog = hits[i].GetComponentInParent<IPathProgress>() ?? hits[i].GetComponent<IPathProgress>();
            if (prog != null)
            {
                if (prog.Progress01 > bestProg) { bestProg = prog.Progress01; best = dmg; }
            }
            else if (bestProg < 0f)
            {
                float d2 = (dmg.GetTransform().position - transform.position).sqrMagnitude;
                if (d2 < fallbackBestSqr) { fallbackBestSqr = d2; best = dmg; }
            }
        }
        return best;
    }

    void Shoot(IDamageable target)
    {
        if (!projectilePf || !firePoint) return;
        var p = Instantiate(projectilePf, firePoint.position, firePoint.rotation);
        p.Init(target, damage);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
