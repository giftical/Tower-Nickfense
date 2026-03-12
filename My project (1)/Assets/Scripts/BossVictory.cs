using UnityEngine;

public class BossVictory : MonoBehaviour
{
    bool triggered;

    void OnDestroy()
    {
        if (triggered) return;
        triggered = true;

        if (VictoryManager.I != null)
            VictoryManager.I.Win();
    }
}