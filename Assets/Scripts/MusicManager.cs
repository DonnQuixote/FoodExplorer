 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    private float volumeGlobal = 1.0f;
    private const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";
    private AudioSource audioSource;
    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
        volumeGlobal = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME, 1.0f);
        audioSource.volume = volumeGlobal;
    }
    public void ChangeVolume()
    {
        volumeGlobal += .1f;
        if (volumeGlobal > 1f)
        {
            volumeGlobal = 0.0f;
        }
        audioSource.volume = volumeGlobal;
        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, volumeGlobal);
        PlayerPrefs.Save();
    }



    public float GetGlobalVolume()
    {
        return volumeGlobal;
    }
}
