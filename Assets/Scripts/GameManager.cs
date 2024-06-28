using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    #region event
    public delegate void GameOver_Del();
    public event GameOver_Del GameOverStateEvent;
    #endregion

    public GameTimer gameTimer;
    public MineMap mineMap;
    public EMineType mineLevel = EMineType.normal;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Time.timeScale = 1;

        gameTimer.OnTimeOverEvent += (float currentTime) => { GameOver(); };
        gameTimer.StartCountDown(72, 0.01f);
    }

    public void GameOver() {
        //if(GameOverStateEvent.GetInvocationList().Length > 0) GameOverStateEvent();
        GameOverStateEvent?.Invoke();
    }
}
