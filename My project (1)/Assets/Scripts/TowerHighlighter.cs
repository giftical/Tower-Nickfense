using UnityEngine;

public class TowerHighlighter : MonoBehaviour
{
    [SerializeField] Color highlightColor = Color.blue;

    Renderer[] renderers;
    MaterialPropertyBlock mpb;

    static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
    static readonly int ColorId = Shader.PropertyToID("_Color");

    void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>(true);
        mpb = new MaterialPropertyBlock();
    }

    public void SetHighlighted(bool on)
    {
        foreach (var r in renderers)
        {
            if (r == null) continue;

            if (on)
            {
                r.GetPropertyBlock(mpb);
                mpb.SetColor(BaseColorId, highlightColor);
                mpb.SetColor(ColorId, highlightColor);
                r.SetPropertyBlock(mpb);
            }
            else
            {
                // Clear per-renderer overrides
                mpb.Clear();
                r.SetPropertyBlock(mpb);
            }
        }
    }
}
