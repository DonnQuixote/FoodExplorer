using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] BaseCounter baseCounter;
    [SerializeField] GameObject[] selectedGameObject;

    private void Start()
    {
        Player.Instance.OnSelectedCounterChanged += IPlayer_OnSelectedCounterChanged;
    }

    private void IPlayer_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if(e.selectedCounter2 == baseCounter)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Hide()
    {
        foreach(GameObject go in selectedGameObject)
        {
            go.SetActive(false);
        }
        
    }

    private void Show()
    {
        foreach (GameObject go in selectedGameObject)
        {
            go.SetActive(true);
        }
    }
}
