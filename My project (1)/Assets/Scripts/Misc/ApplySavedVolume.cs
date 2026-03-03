using UnityEngine;

public class ApplySavedVolume : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    private const string PrefMusicVol = "music_volume";

    private void Awake()
    {
        if (!musicSource) musicSource = GetComponent<AudioSource>();

        float saved = PlayerPrefs.GetFloat(PrefMusicVol, 1f);
        musicSource.volume = Mathf.Clamp01(saved);
    }
}