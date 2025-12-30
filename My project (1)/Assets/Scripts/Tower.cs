using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Stats (current runtime values)")]
    [SerializeField, Min(0.05f)] float attackSpeed = 1f;
    [SerializeField, Min(0f)] float damage = 2f;
    [SerializeField, Min(0.1f)] float range = 5f;

    [Header("Targeting")]
    [SerializeField] LayerMask enemyMask;
    [SerializeField] Transform firePoint;
    [SerializeField] Projectile projectilePf;

    [Header("Meta")]
    [SerializeField] TowerData data;
    [SerializeField] int level = 1;

    public TowerData Data => data;
    public int Level => level;

    float baseAttackSpeed, baseDamage, baseRange;
    bool baseCaptured;

    float traitAttackSpeedMult = 1f;
    float traitDamageMult = 1f;
    float traitRangeMult = 1f;

    float cd;

    void Awake()
    {
        CaptureBaseIfNeeded();
        RebuildStats();
    }

    void CaptureBaseIfNeeded()
    {
        if (baseCaptured) return;
        baseCaptured = true;
        baseAttackSpeed = attackSpeed;
        baseDamage = damage;
        baseRange = range;
    }

    public void InitFromData(TowerData d)
    {
        CaptureBaseIfNeeded();

        data = d;
        level = 1;

        traitAttackSpeedMult = 1f;
        traitDamageMult = 1f;
        traitRangeMult = 1f;

        RebuildStats();
    }

    public bool TryUpgrade()
    {
        if (data == null) return false;

        level++;
        RebuildStats();

        Debug.Log($"Tower {data.displayName} upgraded to level {level}");
        return true;
    }

    public void SetTraitMultipliers(float atk, float dmg, float rng)
    {
        traitAttackSpeedMult = Mathf.Max(0.01f, atk);
        traitDamageMult = Mathf.Max(0.01f, dmg);
        traitRangeMult = Mathf.Max(0.01f, rng);

        RebuildStats();
    }

    void RebuildStats()
    {
        int steps = Mathf.Max(0, level - 1);

        float lvlAtkMult = 1f;
        float lvlDmgMult = 1f;
        float lvlRngMult = 1f;

        if (data != null)
        {
            lvlAtkMult = Mathf.Pow(data.attackSpeedMultiplierPerLevel, steps);
            lvlDmgMult = Mathf.Pow(data.damageMultiplierPerLevel, steps);
            lvlRngMult = Mathf.Pow(data.rangeMultiplierPerLevel, steps);
        }

        attackSpeed = baseAttackSpeed * lvlAtkMult * traitAttackSpeedMult;
        damage = baseDamage * lvlDmgMult * traitDamageMult;
        range = baseRange * lvlRngMult * traitRangeMult;
    }

    void Update()
    {
        cd -= Time.deltaTime;
        if (cd > 0f) return;

        var target = AcquireTarget();
        if (target == null) return;

        Shoot(target);

        cd = 1f / Mathf.Max(0.05f, attackSpeed);
    }

    IDamageable AcquireTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, range, enemyMask);

        IDamageable best = null;
        float bestProg = -1f;
        float fallbackBestSqr = float.PositiveInfinity;

        foreach (var h in hits)
        {
            var dmg = h.GetComponentInParent<IDamageable>() ?? h.GetComponent<IDamageable>();
            if (dmg == null || dmg.IsDead) continue;

            var prog = h.GetComponentInParent<IPathProgress>() ?? h.GetComponent<IPathProgress>();
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
}
