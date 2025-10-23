using UnityEngine;

[CreateAssetMenu(menuName = "Towers/TowerData")]
public class TowerData : ScriptableObject
{
    public GameObject prefab;
    public float footprintRadius = 0.5f;
    public int cost = 10;
}
