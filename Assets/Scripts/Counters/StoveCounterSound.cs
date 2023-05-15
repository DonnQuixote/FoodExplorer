using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] private StokeCounters stokeCounter;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        stokeCounter.OnStateChanged += StokeCounter_OnStateChanged;
    }

    private void StokeCounter_OnStateChanged(object sender, StokeCounters.OnStateChangedEventArgs e)
    {
        bool playSound = e.state == StokeCounters.State.Frying ||
                          e.state == StokeCounters.State.Fried;
        if (playSound)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Pause();
        }
    }
}
