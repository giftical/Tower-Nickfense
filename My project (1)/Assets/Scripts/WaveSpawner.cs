using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [Header("Scene refs")]
    public Transform pathRoot;

    [Header("Waves")]
    public List<Wave> waves = new();
    public bool loop = false;

    IEnumerator Start()
    {
        do
        {
            foreach (var wave in waves)
                yield return StartCoroutine(RunWave(wave));
        }
        while (loop);
    }

    IEnumerator RunWave(Wave wave)
    {
        if (wave.preDelay > 0f) yield return new WaitForSeconds(wave.preDelay);

        var running = new List<Coroutine>();
        foreach (var g in wave.groups)
            running.Add(StartCoroutine(RunGroup(g)));

        foreach (var c in running) yield return c;
    }

    IEnumerator RunGroup(SpawnGroup g)
    {
        if (g.prefab == null || g.count <= 0) yield break;
        if (g.startDelay > 0f) yield return new WaitForSeconds(g.startDelay);

        for (int i = 0; i < g.count; i++)
        {
            var e = Instantiate(g.prefab);
            e.Init(pathRoot);
            if (i < g.count - 1 && g.interval > 0f)
                yield return new WaitForSeconds(g.interval);
        }
    }
}
