using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerReCheckUI : MonoBehaviour
{
    public static PlayerReCheckUI Instance { get; private set; }
    [SerializeField]Button yesButton;
    [SerializeField]Button noButton;

    public event EventHandler<OnPlayerClickReCheckButtonArgs> OnPlayerClickReCheckButton;

    public class OnPlayerClickReCheckButtonArgs : EventArgs
    {
        public int agreeInstantiatePosition;
    }

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        Hide();
        yesButton.onClick.AddListener(() => {
            OnPlayerClickReCheckButton?.Invoke(this, new OnPlayerClickReCheckButtonArgs
            {
                agreeInstantiatePosition = 1
            });
            Hide();
        });
        noButton.onClick.AddListener(() => {
            OnPlayerClickReCheckButton?.Invoke(this, new OnPlayerClickReCheckButtonArgs
            {
                agreeInstantiatePosition = 2
            });
            Hide();
        });
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
