using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    [SerializeField] float _timeLimit = 60;
    [SerializeField] LoLTimer _timer;

    private Vector3 _intialPosition;
    private bool _isMoving = false;

    public bool IsMoving
    {
        get { return _isMoving; }
        set { _isMoving = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        _isMoving = true;
        _intialPosition = transform.position;
        StartTimer();

        _timer.TimerComplete += onTimerComplete;
    }

    public void StartTimer()
    {
        _timer.StartTimer(_timeLimit);
    }

    public void StopTimer()
    {
        _timer.StopTimer();
    }

    private void onTimerComplete()
    {
        transform.position = _intialPosition;
        _timer.StartTimer(_timeLimit);
    }

    // Update is called once per frame
    void Update()
    {
        if(_isMoving)
        {
            _intialPosition += new Vector3(0f, -1.5f, -1.5f) * Time.deltaTime;
            transform.position += new Vector3(2f, -1.5f, -1.5f) * Time.deltaTime;
        }
    }
}
