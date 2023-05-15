using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance { get; private set; }
    [SerializeField] private Button soundEffectsButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private TextMeshProUGUI soundEffectText;
    [SerializeField] private TextMeshProUGUI musicText;


    private void Awake()
    {
        Instance = this;
        soundEffectsButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.ChangeVolume();
            UpdarteVisual();
        });

        musicButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeVolume();
            UpdarteVisual();
        });

        closeButton.onClick.AddListener(() => {
            Hide();
            GamePauseUI.Instance.Show();
        });

    }

    private void Start()
    {
        KitchenGameObject.Instance.OnUnPaused += KitchenGameObject_OnUnPaused;
        UpdarteVisual();
        Hide();
    }

    private void KitchenGameObject_OnUnPaused(object sender, System.EventArgs e)
    {
        if (gameObject.activeSelf)
        {
            Hide();
        }
        
    }

    private void UpdarteVisual()
    {
        soundEffectText.text = "Sound Effects: "+Mathf.Round(SoundManager.Instance.GetGlobalVolume()* 10.0f).ToString();
        musicText.text = "Music: "+Mathf.Round(MusicManager.Instance.GetGlobalVolume()* 10.0f).ToString();


    }


    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        soundEffectsButton.Select();
    }
}
