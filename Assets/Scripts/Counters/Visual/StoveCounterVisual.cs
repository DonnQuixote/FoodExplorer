using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] private StokeCounters stoveCounter;
    [SerializeField] private GameObject stoveOnGameObject;
    [SerializeField] private GameObject particlesGameObject;

    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
    }

    private void StoveCounter_OnStateChanged(object sender, StokeCounters.OnStateChangedEventArgs e)
    {
        bool showVisual = e.state == StokeCounters.State.Frying || e.state == StokeCounters.State.Fried;

        stoveOnGameObject.SetActive(showVisual);
        particlesGameObject.SetActive(showVisual);

    }
}
