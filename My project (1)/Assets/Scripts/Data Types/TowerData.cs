using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Towers/TowerData")]
public class TowerData : ScriptableObject
{
    [Header("Prefab / footprint")]
    public GameObject prefab;
    public float footprintRadius = 0.5f;

    [Header("Shop")]
    public string displayName;
    public Sprite icon;
    public int cost = 10;

    [Header("Unique tower type id (used for trait stacks)")]
    [Tooltip("Must be unique per tower kind. Example: 'scout', 'sniper'. Two placed scouts count as ONE unique type.")]
    public string typeId;

    [Header("Traits (shared assets)")]
    public List<TraitData> traits = new();

    [Header("Upgrade (simple multipliers)")]
    public float damageMultiplierPerLevel = 1.5f;
    public float attackSpeedMultiplierPerLevel = 1.25f;
    public float rangeMultiplierPerLevel = 1.1f;
}
