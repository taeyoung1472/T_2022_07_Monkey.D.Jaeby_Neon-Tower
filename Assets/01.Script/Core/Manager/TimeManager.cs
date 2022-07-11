using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instnace;
    float freezeTime = 0;
    float freezeValue = 0;
    bool isStop;
    bool isUseTimeFreeze;
    private void Awake()
    {
        if (instnace == null)
            instnace = this;
    }
    void Start()
    {
        Time.timeScale = 1;
        isUseTimeFreeze = JsonManager.instance.Data.isTimeFreeze;
    }
    public void Update()
    {
        Freeze();
    }
    void Freeze()
    {
        if (isStop || !isUseTimeFreeze) return;
        float freezeValueGoal;
        if (freezeTime > 0)
        {
            freezeTime -= Time.deltaTime * (1 / (1 - freezeValue));
            freezeValueGoal = freezeValue;
        }
        else
        {
            freezeValueGoal = 0;
        }
        freezeValue = Mathf.Lerp(freezeValue, freezeValueGoal, Time.deltaTime * 10);
        Time.timeScale = 1 - freezeValue;
    }
    public void TimeFreeze(float value, float time)
    {
        if (freezeValue < value)
        {
            freezeValue = value;
        }
        if (freezeTime < time)
        {
            freezeTime = time;
        }
    }
    public void ActiveTimeScale(bool isActive)
    {
        isStop = !isActive;
        if (isStop)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1 - freezeValue;
        }
    }
}
