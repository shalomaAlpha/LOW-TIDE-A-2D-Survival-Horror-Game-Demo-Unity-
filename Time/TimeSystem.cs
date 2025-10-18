using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeSystem : MonoBehaviour
{
    public static TimeSystem Instance { get; private set; }
    public int hour=7;
    public int minute=0;
    public int day = 1;  

    public int timeValue; //each 720 second in real world is one day in game


    public TMP_Text ClockText;
    private Coroutine TickTimeCoroutine;
    bool pauseOrNot;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        UpdateClockText();
        StartTimer();
    }
    public void PauseTime()
    {
        if(TickTimeCoroutine!=null)
        StopCoroutine(TickTimeCoroutine);

        pauseOrNot = true;
        TickTimeCoroutine = null;
    }

    public void ContinueTime()
    {
        if (TickTimeCoroutine != null) return;

        TickTimeCoroutine = StartCoroutine(TickTime());
    }
    
    public void StartTimer()
    {
        if (pauseOrNot || TickTimeCoroutine != null) return;

        TickTimeCoroutine = StartCoroutine(TickTime());
    }
    IEnumerator TickTime()
    {
        while(timeValue<=720)
        {
            yield return new WaitForSeconds(1f); //each second in real life is 2 minute in the game
            timeValue += 1;
            minute += 2;
            if (minute % 10 == 0)
            {
                UpdateClockText(); 
            }

            if (minute >= 60)
            {
                hour += 1;
                if(hour==24)
                {
                    hour = 0;
                }
                minute = 0;
                UpdateClockText();
            }
        }
        timeValue = 0;
        StartNextDay();
    }

    public void UpdateClockText()
    {
        if(minute==0)
        {
            ClockText.text = hour + ":00";
        }
        else
        {
            ClockText.text = hour + ":" + minute;
        }
        
    }

    public void StartNextDay()
    {
        day++;
    }
}
