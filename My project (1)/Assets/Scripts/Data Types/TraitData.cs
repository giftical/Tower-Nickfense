using UnityEngine;

[CreateAssetMenu(menuName = "Towers/TraitData")]
public class TraitData : ScriptableObject
{
    public string displayName = "Mercenary";

    [Header("Activation (unique tower types)")]
    [Min(1)] public int requiredUniqueTypes = 2;

    [Header("Buffs (bonuses as percentages)")]
    [Min(0f)] public float attackSpeedBonus = 0.10f;
    [Min(0f)] public float damageBonus = 0.10f;
    [Min(0f)] public float rangeBonus = 0.10f;

    public float AttackSpeedMultiplier => 1f + attackSpeedBonus;
    public float DamageMultiplier => 1f + damageBonus;
    public float RangeMultiplier => 1f + rangeBonus;
}
