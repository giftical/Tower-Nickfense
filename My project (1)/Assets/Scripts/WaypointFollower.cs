using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyStats))]
public class WaypointFollower : MonoBehaviour, IPathProgress
{
    [SerializeField] Transform pathRoot;
    [SerializeField] float turnSpeed = 1000f;
    [SerializeField] float reachThreshold = 0.05f;

    readonly List<Transform> waypoints = new();
    readonly List<float> segLen = new();
    readonly List<float> prefix = new();
    float totalLen;

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
        if (pathRoot != null)
        {
            BuildWaypointList();
            transform.position = waypoints[0].position;
            index = 1;
        }
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
        if (index >= waypoints.Count) Destroy(gameObject);
    }

    void BuildWaypointList()
    {
        waypoints.Clear();
        foreach (Transform c in pathRoot) waypoints.Add(c);
        waypoints.Sort((a, b) => a.GetSiblingIndex().CompareTo(b.GetSiblingIndex()));

        segLen.Clear();
        prefix.Clear();
        totalLen = 0f;

        if (waypoints.Count >= 2)
        {
            prefix.Add(0f);
            for (int i = 0; i < waypoints.Count - 1; i++)
            {
                float L = Vector3.Distance(waypoints[i].position, waypoints[i + 1].position);
                segLen.Add(L);
                totalLen += L;
                prefix.Add(totalLen);
            }
        }
    }

    public float Progress01
    {
        get
        {
            if (totalLen <= 0f || waypoints.Count < 2) return 0f;

            if (index <= 0) return 0f;
            if (index >= waypoints.Count) return 1f;

            Vector3 a = waypoints[index - 1].position;
            Vector3 b = waypoints[index].position;

            float seg = segLen[index - 1];
            float along = seg > 0f ? Mathf.Clamp(Vector3.Distance(a, transform.position), 0f, seg) : 0f;

            float traveled = prefix[index - 1] + along;
            return Mathf.Clamp01(traveled / totalLen);
        }
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
