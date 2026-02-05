using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class DiscMeshFill : MonoBehaviour
{
    [SerializeField, Min(3)] int segments = 96;

    Mesh mesh;

    void Awake()
    {
        Build();
    }

    void OnValidate()
    {
        if (!Application.isPlaying) Build();
    }

    void Build()
    {
        if (mesh == null)
        {
            mesh = new Mesh { name = "DiscFillMesh" };
            GetComponent<MeshFilter>().sharedMesh = mesh;
        }
        else
        {
            mesh.Clear();
        }

        // Center + ring vertices
        Vector3[] verts = new Vector3[segments + 1];
        int[] tris = new int[segments * 3];
        Vector2[] uv = new Vector2[segments + 1];

        verts[0] = Vector3.zero;
        uv[0] = new Vector2(0.5f, 0.5f);

        for (int i = 0; i < segments; i++)
        {
            float a = (i / (float)segments) * Mathf.PI * 2f;
            float x = Mathf.Cos(a);
            float z = Mathf.Sin(a);

            verts[i + 1] = new Vector3(x, 0f, z);
            uv[i + 1] = new Vector2(x * 0.5f + 0.5f, z * 0.5f + 0.5f);
        }

        for (int i = 0; i < segments; i++)
        {
            int next = (i + 1) % segments;

            int tri = i * 3;
            tris[tri + 0] = 0;
            tris[tri + 1] = i + 1;
            tris[tri + 2] = next + 1;
        }

        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.uv = uv;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }
}
