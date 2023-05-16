using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        playButton.onClick.AddListener(OnPlayClick);
        quitButton.onClick.AddListener(OnQuitClick);

        Time.timeScale = 1.0f;
    }

    private void OnPlayClick()
    {
        //SceneManager.LoadScene(0);
        Loader.Load(Loader.Scene.GameScene);
        //Debug.Log("Click the PLAY button");
    }

    private void OnQuitClick()
    {
        //There is no effect in editor mode, only on other platforms
        Application.Quit();
        //Debug.Log("Click the QUIT button");
    }
}
