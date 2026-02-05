using UnityEngine;

public class TowerRangeRing : MonoBehaviour
{
    [Header("Parts")]
    [SerializeField] Transform fill;               // Plane or Quad
    [SerializeField] LineRenderer outline;         // LineRenderer

    [Header("Appearance")]
    [SerializeField, Min(3)] int segments = 96;
    [SerializeField] float yOffset = 0.02f;
    [SerializeField] float outlineHeight = 0.01f;
    [SerializeField] float outlineWidth = 0.08f;

    [Header("Radius Multipliers")]
    [SerializeField] float fillRadiusMult = 1.00f;     // set to ~1.03 to match outline
    [SerializeField] float outlineRadiusMult = 1.03f;  // outline slightly outside

    [Header("Fill Mesh Scaling")]
    [Tooltip("Plane = 10, Quad = 1")]
    [SerializeField] float fillMeshSize = 10f;

    void Awake()
    {
        if (outline != null)
        {
            outline.useWorldSpace = false;
            outline.loop = true;
            outline.startWidth = outlineWidth;
            outline.endWidth = outlineWidth;
        }

        Hide();
    }

    public void ShowAt(Vector3 worldPos, float radius)
    {
        gameObject.SetActive(true);
        transform.position = new Vector3(worldPos.x, worldPos.y + yOffset, worldPos.z);

        // Fill: scale by diameter, corrected for mesh base size
        if (fill != null)
        {
            float rFill = radius * fillRadiusMult;
            float diameterWorld = rFill * 2f;
            float diameterLocal = diameterWorld / Mathf.Max(0.0001f, fillMeshSize);

            Vector3 s = fill.localScale;
            fill.localScale = new Vector3(diameterLocal, s.y, diameterLocal);
        }

        // Outline: draw a slightly larger ring
        BuildOutline(radius * outlineRadiusMult);
    }

    void BuildOutline(float radius)
    {
        if (outline == null) return;

        outline.positionCount = segments;
        outline.startWidth = outlineWidth;
        outline.endWidth = outlineWidth;

        for (int i = 0; i < segments; i++)
        {
            float a = (i / (float)segments) * Mathf.PI * 2f;
            float x = Mathf.Cos(a) * radius;
            float z = Mathf.Sin(a) * radius;
            outline.SetPosition(i, new Vector3(x, outlineHeight, z));
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
