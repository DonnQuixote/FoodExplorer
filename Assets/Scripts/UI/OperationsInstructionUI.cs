using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OperationsInstructionUI : MonoBehaviour
{
    [SerializeField] Button backButton;
    [SerializeField] Transform gamePausedUI;

    private void Start()
    {
        backButton.onClick.AddListener(() => {
            gamePausedUI.gameObject.SetActive(true);
            Hide();
        });
        Hide();
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        backButton.Select();
    }
}
