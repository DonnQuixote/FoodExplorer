using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayingClockUI : MonoBehaviour
{
    [SerializeField] private Image timerImage;
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Update()
    {
        timerImage.fillAmount = KitchenGameObject.Instance.GetGamePlayingTimerNormalized();
        scoreText.text = "Score: " + DeliveryManager.Instance.GetSuccessedScore().ToString();
    }
}
