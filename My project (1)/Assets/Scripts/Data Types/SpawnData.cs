using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SpawnGroup
{
    public WaypointFollower prefab; // enemy prefab
    [Min(1)] public int count;
    [Min(0f)] public float startDelay;
    [Min(0f)] public float interval;
}

[System.Serializable]
public class Wave
{
    public string name;
    [Min(0f)] public float preDelay;
    public List<SpawnGroup> groups = new();
}
