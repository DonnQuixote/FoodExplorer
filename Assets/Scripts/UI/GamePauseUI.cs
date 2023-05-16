using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    public static GamePauseUI Instance { get; private set; }
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button operationsButton;
    [SerializeField] private Button backgroundButton;
    [SerializeField] private Transform backgroundUI;
    [SerializeField] private Transform OperationsUI;

    

    private void Awake()
    {
        Instance = this;
        resumeButton.onClick.AddListener(() => {
            KitchenGameObject.Instance.TogglePauseGame();
        });

        mainMenuButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.MainMenuScene);
        });

        optionsButton.onClick.AddListener(() => {
            Hide();
            OptionsUI.Instance.Show();
        });

        backgroundButton.onClick.AddListener(() => {
            Hide();
            backgroundUI.gameObject.SetActive(true);
        });

        operationsButton.onClick.AddListener(() => {
            Hide();
            OperationsUI.gameObject.SetActive(true);
        });

    }
    private void Start()
    {
        KitchenGameObject.Instance.OnPaused += KitchenGameObject_OnPaused;
        KitchenGameObject.Instance.OnUnPaused += KitchenGameObject_OnUnPaused;
        Hide();
    }

    private void KitchenGameObject_OnPaused(object sender, System.EventArgs e)
    {
        Show();
    }

    private void KitchenGameObject_OnUnPaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        resumeButton.Select();
    }
}
