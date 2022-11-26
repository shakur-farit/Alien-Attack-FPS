using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#region Enums

public enum TimerFormats
{
    Hours,
    Minutes
}

public enum SecondsFormats
{
    Whole,
    TenthDecimal,
    HundrethDecimal
}

#endregion

public class Timer : MonoBehaviour
{
    #region Fields

    [Header("Component")]
    public Text timerText;

    [Header("Timer Settings")]
    public float currentTime;
    public bool countDown;

    [Header("Limit Settings")]
    public bool hasLimit;
    public float timerLimit;

    [Header("Timer Format Settings")]
    [Tooltip("Has Timer Format or not. If has Seconds Timer Format will be disabled")]
    public bool hasTimerFormat;
    public TimerFormats format;
    private Dictionary<TimerFormats, string> timeFormats = new Dictionary<TimerFormats, string>();

    [Header("Seconds Format Settings")]
    [Tooltip("Has Seconds Timer Format or not.")]
    public bool hasSecondsFormat;
    public SecondsFormats secondsTimeFormat;
    private Dictionary<SecondsFormats, string> secondsFormats = new Dictionary<SecondsFormats, string>();

    private int seconds;
    private int minutes;
    private int hours;

    private string timerMinutesString;
    private string timerHoursString;

    #endregion

    #region Private methods
    private void Start()
    {
        timeFormats.Add(TimerFormats.Minutes, timerMinutesString);
        timeFormats.Add(TimerFormats.Hours, timerHoursString);

        secondsFormats.Add(SecondsFormats.Whole, "0");
        secondsFormats.Add(SecondsFormats.TenthDecimal, "0.0");
        secondsFormats.Add(SecondsFormats.HundrethDecimal, "0.00");
    }

    private void Update()
    {
        hours = Mathf.FloorToInt((currentTime / 3600f) % 24f);
        minutes = Mathf.FloorToInt((currentTime / 60f) % 60f);
        seconds = Mathf.FloorToInt(currentTime % 60f);

        timerMinutesString = string.Format("{0:00}:{1:00}", minutes, seconds);
        timerHoursString = string.Format("{0:0}:{1:00}:{2:00}", hours, minutes, seconds);

        // If current time was set equal 3600 or more TimeFormat change to Hours
        if (currentTime >= 3600)
        {
            format = TimerFormats.Hours;
        }

        // If countDown is true time is couting down
        if (countDown)
        {
            currentTime -= Time.deltaTime;
        }
        else
        {
            currentTime += Time.deltaTime;
        }

        // Stop the timer if has made it to limit
        if (hasLimit && ((countDown && currentTime <= timerLimit) || (!countDown && currentTime >= timerLimit)))
        {
            currentTime = timerLimit;
            SetTimerText();
            timerText.color = Color.red;
            enabled = false;
        }

        // In last 10 seconds (if timer is couting down) turn off TimerFormat (if it on) and turn on Timer Format in TenthDecamal for beauty
        if (countDown && currentTime < 10)
        {
            hasTimerFormat = false;
            hasSecondsFormat = true;
            secondsTimeFormat = SecondsFormats.TenthDecimal;
        }

        SetTimerText();
    }

    /// <summary>
    /// Set the timer's text
    /// </summary>
    private void SetTimerText()
    {
        if (hasTimerFormat)
        {
            hasSecondsFormat = false;
            if (format == TimerFormats.Minutes)
            {
                timerText.text = timerMinutesString;
            }
            else
            {
                timerText.text = timerHoursString;
            }
        }
        else if (hasSecondsFormat)
        {
            hasTimerFormat = false;
            timerText.text = currentTime.ToString(secondsFormats[secondsTimeFormat]);
        }
        else
            timerText.text = currentTime.ToString();
    }

    #endregion
}
