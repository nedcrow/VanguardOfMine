using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public float targetTime;
    public float currentTime;
    IEnumerator myCoroutine = null;

    public delegate void TimeOver_Del(float lastTime);
    public event TimeOver_Del OnTimeOverEvent;
    public event TimeOver_Del OnTimeChangeEvent;

    private void OnTimeOver()
    {
        OnTimeOverEvent(currentTime*1000);
    }

    private void OnTimeChange()
    {
        OnTimeChangeEvent(Mathf.Clamp(currentTime,0,targetTime) * 1000);
    }

    /// <summary>
    /// OnTimeOverEvent, OnTimeChangeEvent
    /// </summary>
    /// <param name="targetTime">This type is second</param>
    /// <param name="duration">This type is second</param>
    public void StartCountDown(float targetTime, float duration=1.0f)
    {
        if(myCoroutine != null)
        {
            StopCoroutine(myCoroutine);
        }
        myCoroutine = Co_CountDown(targetTime, duration);
        StartCoroutine(myCoroutine);
    }

    IEnumerator Co_CountDown(float targetTime, float duration)
    {
        this.targetTime = targetTime;
        currentTime = 0;
        while (currentTime < targetTime)
        {
            currentTime += duration;            

            yield return new WaitForSeconds(duration);
            OnTimeChange();
        }
        OnTimeOver();
    }
}
