using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyStats))]
public class WaypointFollower : MonoBehaviour
{
    [SerializeField] Transform pathRoot;
    [SerializeField] float turnSpeed = 10f;
    [SerializeField] float reachThreshold = 0.05f;

    readonly List<Transform> waypoints = new();
    int index;
    EnemyStats stats;

    public void Init(Transform path)
    {
        pathRoot = path;
        BuildWaypointList();
        transform.position = waypoints[0].position;
        index = 1;
    }

    void Awake()
    {
        stats = GetComponent<EnemyStats>();
        if (pathRoot != null) { BuildWaypointList(); transform.position = waypoints[0].position; index = 1; }
    }

    void Update()
    {
        if (pathRoot == null || index >= waypoints.Count) return;

        Vector3 target = waypoints[index].position;
        Vector3 dir = target - transform.position;

        float step = stats.moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target, step);

        if (dir.sqrMagnitude > 0.0001f)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), turnSpeed * Time.deltaTime);

        if ((transform.position - target).sqrMagnitude <= reachThreshold * reachThreshold) index++;
        if (index >= waypoints.Count) Destroy(gameObject); // reached goal; replace with goal damage if needed
    }

    void BuildWaypointList()
    {
        waypoints.Clear();
        foreach (Transform c in pathRoot) waypoints.Add(c);
        waypoints.Sort((a, b) => a.GetSiblingIndex().CompareTo(b.GetSiblingIndex()));
    }

    void OnDrawGizmosSelected()
    {
        if (!pathRoot) return;
        Gizmos.color = Color.yellow;
        Vector3? prev = null;
        foreach (Transform t in pathRoot)
        {
            Gizmos.DrawSphere(t.position, 0.15f);
            if (prev.HasValue) Gizmos.DrawLine(prev.Value, t.position);
            prev = t.position;
        }
    }
}
