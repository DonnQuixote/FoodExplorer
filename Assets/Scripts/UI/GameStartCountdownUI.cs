using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameStartCountdownUI : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI countdownText;

    private void Start()
    {
        KitchenGameObject.Instance.OnStateChanged += KitchenGameObject_OnStateChanged;
        Hide();
    }
    private void Update()
    {
        //countdownText.text = KitchenGameObject.Instance.GetCountdownToStartTimer().ToString("F2");
        //countdownText.text = KitchenGameObject.Instance.GetCountdownToStartTimer().ToString("#.##");
        countdownText.text = Mathf.Ceil(KitchenGameObject.Instance.GetCountdownToStartTimer()).ToString();

    }
    private void KitchenGameObject_OnStateChanged(object sender, System.EventArgs e)
    {
        if (KitchenGameObject.Instance.IsCountdownToStartActive())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }


    private void Show()
    {
        countdownText.transform.gameObject.SetActive(true);
    }

    private void Hide()
    {
        countdownText.transform.gameObject.SetActive(false);
    }
}
