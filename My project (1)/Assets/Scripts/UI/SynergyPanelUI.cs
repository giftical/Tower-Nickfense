// SynergyPanelManualUI.cs
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SynergyPanelManualUI : MonoBehaviour
{
    [Serializable]
    public class Row
    {
        public TraitData trait;   // which synergy this row represents
        public TMP_Text label;    // where to write it
    }

    [Header("Refs")]
    [SerializeField] private SynergyManager synergyManager;

    [Header("Rows (set size manually)")]
    [SerializeField] private List<Row> rows = new();

    void OnEnable()
    {
        if (synergyManager == null) synergyManager = SynergyManager.Instance;
        if (synergyManager != null) synergyManager.OnSynergyChanged += Refresh;

        Refresh(); // draw immediately
    }

    void OnDisable()
    {
        if (synergyManager != null) synergyManager.OnSynergyChanged -= Refresh;
    }

    public void Refresh()
    {
        if (synergyManager == null) synergyManager = SynergyManager.Instance;
        if (synergyManager == null) return;

        var counts = synergyManager.GetTraitCounts(); // only traits currently present -> we fallback to 0

        for (int i = 0; i < rows.Count; i++)
        {
            var r = rows[i];
            if (r == null || r.label == null || r.trait == null) continue;

            int current = counts.TryGetValue(r.trait, out int c) ? c : 0;
            int required = Mathf.Max(0, r.trait.requiredUniqueTypes);

            // Format: [name] - x/x (always shown, even 0)
            r.label.text = $"{r.trait.displayName} - {current}/{required}";
        }
    }
}