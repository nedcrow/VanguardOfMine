using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameTimer gameTimer;
    public MineMap mineMap;
    public int mineLevel = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Time.timeScale = 1;

        gameTimer.StartCountDown(72, 0.01f);
    }
}
