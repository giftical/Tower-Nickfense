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

    [Header("Upgrade (simple multipliers)")]
    public float damageMultiplierPerLevel = 1.5f;
    public float attackSpeedMultiplierPerLevel = 1.25f;
    public float rangeMultiplierPerLevel = 1.1f;
}
