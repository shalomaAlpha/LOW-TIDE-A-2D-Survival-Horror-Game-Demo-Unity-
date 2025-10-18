using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeRecordor : MonoBehaviour
{
    TimeSystem timesystem;
    public static TimeRecordor Instance;
    public int FireplaceTimeRecord;

    int start;
    int end;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        timesystem = GetComponent<TimeSystem>();    
    }

    public void RecordTimeValueStart()
    {
        start = timesystem.timeValue;
    }

    public void RecordTimeValueEnd()
    {
        end = timesystem.timeValue;
    }

    public int CaculateTimeChange()
    {
        int returnValue = end - start;
        end = 0;
        start = 0;

        return returnValue;
    }
}
