// SynergyManager.cs
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class SynergyManager : MonoBehaviour
{
    public static SynergyManager Instance { get; private set; }

    readonly List<TowerSynergyAgent> towers = new();

    // For each trait, the set of unique tower typeIds currently present
    readonly Dictionary<TraitData, HashSet<string>> traitToTypes = new();

#if UNITY_EDITOR
    [Header("Debug (Editor only)")]
    [SerializeField, TextArea(6, 20)] string debugState;
#endif

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void Register(TowerSynergyAgent agent)
    {
        if (agent == null || towers.Contains(agent)) return;
        towers.Add(agent);
        RecalculateAndApply();
    }

    public void Unregister(TowerSynergyAgent agent)
    {
        if (agent == null) return;
        towers.Remove(agent);
        RecalculateAndApply();
    }

    // Called by TowerSynergyAgent when a tower changes its data/traits.
    public void RecalculateAndApplyPublic() => RecalculateAndApply();

    void RecalculateAndApply()
    {
        traitToTypes.Clear();

        // 1) Build unique tower-type sets per trait
        for (int i = 0; i < towers.Count; i++)
        {
            var tower = towers[i]?.Tower;
            var data = tower != null ? tower.Data : null;
            if (data == null || data.traits == null) continue;

            var typeId = data.typeId;
            if (string.IsNullOrWhiteSpace(typeId)) continue;

            for (int k = 0; k < data.traits.Count; k++)
            {
                var trait = data.traits[k];
                if (trait == null) continue;

                if (!traitToTypes.TryGetValue(trait, out var set))
                {
                    set = new HashSet<string>();
                    traitToTypes[trait] = set;
                }

                set.Add(typeId); // unique type counting
            }
        }

        // 2) Apply stacked multipliers per tower (only from active traits)
        for (int i = 0; i < towers.Count; i++)
        {
            var tower = towers[i]?.Tower;
            var data = tower != null ? tower.Data : null;
            if (tower == null || data == null || data.traits == null) continue;

            float atk = 1f;
            float dmg = 1f;
            float rng = 1f;

            for (int k = 0; k < data.traits.Count; k++)
            {
                var trait = data.traits[k];
                if (trait == null) continue;

                if (traitToTypes.TryGetValue(trait, out var set) &&
                    set.Count >= trait.requiredUniqueTypes)
                {
                    atk *= trait.AttackSpeedMultiplier;
                    dmg *= trait.DamageMultiplier;
                    rng *= trait.RangeMultiplier;
                }
            }

            tower.SetTraitMultipliers(atk, dmg, rng);
        }

#if UNITY_EDITOR
        var sb = new StringBuilder();
        foreach (var kv in traitToTypes)
            sb.AppendLine($"{(kv.Key ? kv.Key.displayName : "<null trait>")}: {kv.Value.Count} unique types");
        debugState = sb.ToString();
#endif
    }
}
