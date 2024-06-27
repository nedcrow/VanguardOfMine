using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextTimer : MonoBehaviour
{
    void Start()
    {
        GameManager.instance.gameTimer.OnTimeChangeEvent += OnChangeTime;        
    }

    /// <summary>
    /// example) OnChangeTime(136772) -> 02:16:78
    /// </summary>
    /// <param name="currentMilliSec"></param>
    void OnChangeTime(float currentMilliSec)
    {
        Text text = GetComponent<Text>();
        if (text == null) return;

        float centiSec = Mathf.Ceil(currentMilliSec * 0.1f);
        float sec = Mathf.Floor(centiSec * 0.01f);
        float min = Mathf.Floor(sec / 60);
        string centiSec_str = (centiSec % 100).ToString("00");
        string sec_str = Mathf.Floor(sec % 60).ToString("00");
        string min_str = min.ToString("00");

        text.text = min_str + " : " + sec_str + " : " + centiSec_str;
    }
}
