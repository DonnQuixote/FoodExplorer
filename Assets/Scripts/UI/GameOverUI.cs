using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipesDeliveryText;

    private void Start()
    {
        KitchenGameObject.Instance.OnStateChanged += KitchenGameObject_OnStateChanged;
        Hide();
    }
    
    private void KitchenGameObject_OnStateChanged(object sender, System.EventArgs e)
    {
        if (KitchenGameObject.Instance.IsGameOver())
        {
            recipesDeliveryText.text = DeliveryManager.Instance.GetSuccessedDelivery().ToString();
            Show();
        }
        else
        {
            Hide();
        }
    }


    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
