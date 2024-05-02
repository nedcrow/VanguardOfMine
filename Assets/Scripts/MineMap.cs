using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineMap : MonoBehaviour
{
    void Awake()
    {
        GameTimer.OnTimeOverEvent += GameOver;
    }


    public void GameOver(float currentTime) {
        Debug.Log("GameOver");
    }
}
