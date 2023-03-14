using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] ClearCount clearCount;
    [SerializeField] GameObject selectedGameObject;

    private void Start()
    {
        Player.Instance.OnSelectedCounterChanged += IPlayer_OnSelectedCounterChanged;
    }

    private void IPlayer_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if(e.selectedCounter2 == clearCount)
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
        selectedGameObject.SetActive(false);
    }

    private void Show()
    {
        selectedGameObject.SetActive(true);
    }
}
