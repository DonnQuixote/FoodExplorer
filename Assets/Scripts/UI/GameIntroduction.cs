using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameIntroduction : MonoBehaviour
{

    public static GameIntroduction Instance { get; private set; }
    [SerializeField] Transform backStoryUI;
    [SerializeField] Transform operationsUI;
    [SerializeField] Button backGoOnButton;
    [SerializeField] Button operationsPreviousButton;
    [SerializeField] Button operationsStartButton;


    public event EventHandler OnInGame;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        operationsUI.gameObject.SetActive(false);
        Time.timeScale = 0;
        backGoOnButton.onClick.AddListener(() =>
        {
            operationsUI.gameObject.SetActive(true);
            backStoryUI.gameObject.SetActive(false);
        });
        operationsPreviousButton.onClick.AddListener(() =>
        {
            backStoryUI.gameObject.SetActive(true);
            operationsUI.gameObject.SetActive(false);
        });
        operationsStartButton.onClick.AddListener(() =>
        {
            operationsUI.gameObject.SetActive(true);
            backStoryUI.gameObject.SetActive(true);
            OnInGame?.Invoke(this, EventArgs.Empty);
            Time.timeScale = 1;
            Hide();
        });
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}
