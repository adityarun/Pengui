using System;
using UnityEngine;


public class LoLTimer : MonoBehaviour
{
    private double timerDuration;        //The duration for which the Timer should run
    private double timeLeft;             //The amount of time left before the timer reaches zero
    private bool isTimerOn;             //Boolean value to check if Timer is running or not

    #region Private Variables
    private float _timeCounter;
    #endregion


    private int _currentCounter = 0;

    #region Actions
    //Modify Action return types as per needs
    public Action TimerComplete;
    public Action<int> TimerTick;
#endregion

    private void Awake()
    {
        isTimerOn = false;
    }

    public void StartTimer(double duration)        // Call this function from anywhere to Start the Timer.
    {
        timerDuration = duration;
        timeLeft = timerDuration;
        _timeCounter = 0f;
        isTimerOn = true;
        _currentCounter = Mathf.CeilToInt((float)timeLeft);
        TimerTick?.Invoke(Mathf.CeilToInt((float)timeLeft));
    }

    private void Update()
    {
        if (isTimerOn)
        {
            _timeCounter += Time.deltaTime;
            timeLeft = timerDuration - _timeCounter;
            if (Mathf.CeilToInt((float)timeLeft) != _currentCounter)
            {
                TimerTick?.Invoke(Mathf.CeilToInt((float)timeLeft));        //Action which returns Time left.
                _currentCounter = Mathf.CeilToInt((float)timeLeft);
            }

            if (_timeCounter > timerDuration)
            {
                StopTimer();
            }
        }
    }

    public void StopTimer()     //Can be called from anywhere to stop Timer whenever required.
    {
        if (isTimerOn)
        {
            isTimerOn = false;
            timeLeft = timerDuration - _timeCounter;
            TimerComplete?.Invoke();        //Action called when Timer ends.
        }
    }
    public void ResetTimer()     //Can be called from anywhere to reset Timer whenever required.
    {
        isTimerOn = false;
        timeLeft = timerDuration - _timeCounter;
    }
}