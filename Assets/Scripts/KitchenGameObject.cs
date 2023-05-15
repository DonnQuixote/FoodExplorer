using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class KitchenGameObject : MonoBehaviour
{
    public static KitchenGameObject Instance { get; private set; }

    public event EventHandler OnStateChanged;
    public event EventHandler OnPaused;
    public event EventHandler OnUnPaused;
    [SerializeField] LivingEntity player;
    private LivingEntity tempPlayer;
     private enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver,
    }

    private State state;
    private float waitingToStartTimer = 1.0f;
    private float countdownToStartTimer = 3.0f;
    private float gamePlayingTimer;
    private float gamePlayingTimerMax = 600.0f;

    private bool IsPauseGame = false;
    private bool isNotStartGame = true;
    private void Awake()
    {
        Instance = this;
        state = State.WaitingToStart;
    }

    private void Start()
    {
        GameIntroduction.Instance.OnInGame += KitchenGameObject_OnInGame;
        tempPlayer = player;
        tempPlayer.OnPlayerDestroy += KitchenGameObject_OnPlayerDestroy;
        InputMessage.Instance.OnPause += InputMessage_OnPause;
    }

    private void KitchenGameObject_OnInGame(object sender, EventArgs e)
    {
        isNotStartGame = false;
    }

    private void KitchenGameObject_OnPlayerDestroy(object sender, EventArgs e)
    {
        OnStateChanged?.Invoke(this, EventArgs.Empty);
        gamePlayingTimer = 0f;
        state = State.GameOver;
    }

    private void InputMessage_OnPause(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    private void Update()
    {
        switch (state)
        {
            case State.WaitingToStart:
                if (isNotStartGame) return;

                gamePlayingTimer = gamePlayingTimerMax;

                waitingToStartTimer -= Time.deltaTime;
                if(waitingToStartTimer <= 0f)
                {
                    state = State.CountdownToStart;
                }
                break;

            case State.CountdownToStart:
                OnStateChanged?.Invoke(this, EventArgs.Empty);
                countdownToStartTimer -= Time.deltaTime;
                if (countdownToStartTimer <= 0f)
                {
                    state = State.GamePlaying;
                }
                break;

            case State.GamePlaying:
                OnStateChanged?.Invoke(this, EventArgs.Empty);
                gamePlayingTimer -= Time.deltaTime;
                if (gamePlayingTimer <= 0f)
                {
                    state = State.GameOver;
                }
                break;

            case State.GameOver:
                OnStateChanged?.Invoke(this, EventArgs.Empty);

                break;
        }
        //Debug.Log(state);
    }

    

    public bool IsGamePlaying()
    {
        return state == State.GamePlaying;
    }

    public bool IsGameOver()
    {
        return state == State.GameOver;
    }

    public bool IsCountdownToStartActive()
    {
        return state == State.CountdownToStart;
    }

    public float GetCountdownToStartTimer()
    {
        return countdownToStartTimer;
    }

    public float GetGamePlayingTimerNormalized()
    {
        return (1-gamePlayingTimer / gamePlayingTimerMax);
    }


    public void TogglePauseGame()
    {
        IsPauseGame = !IsPauseGame;
        if (IsPauseGame)
        {
            Time.timeScale = 0.0f;
            OnPaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1.0f;
            OnUnPaused?.Invoke(this, EventArgs.Empty);

        }
    }


}
