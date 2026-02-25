using TMPro;
using UnityEngine;

public class WaveCounterUI : MonoBehaviour
{
    [SerializeField] private WaveSpawner spawner;
    [SerializeField] private TextMeshProUGUI label;

    private void Awake()
    {
        if (label == null) label = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        if (spawner != null)
            spawner.WaveStarted += OnWaveStarted;
    }

    private void OnDisable()
    {
        if (spawner != null)
            spawner.WaveStarted -= OnWaveStarted;
    }

    private void Start()
    {
        if (spawner != null && label != null)
            label.text = $"Wave: {spawner.CurrentWaveIndex}";
    }

    private void OnWaveStarted(int current, int total)
    {
        if (label == null) return;
        label.text = $"Wave: {current}/{total}";
    }
}