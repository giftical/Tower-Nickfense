using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager I { get; private set; }
    TowerData selected;

    void Awake() => I = this;

    public void Select(TowerData data) => selected = data;
    public TowerData GetSelection() => selected;
    public void Clear() => selected = null;
}
