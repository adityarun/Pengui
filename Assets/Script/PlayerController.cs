using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GG.Infrastructure.Utils.Swipe;
using System;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private TouchInputHandler _touchInputHandler;
    [SerializeField] private int _forceValue = 2;
    [SerializeField] private SwipeListener _swipeListener;
    [SerializeField] private float _jumpOffset = 1.5f;
    [SerializeField] private Vector3 _targetOffset = new Vector3(0, 1.9f, 1.19f);

    public float snapDistance = 0.5f;  // The distance within which to snap
    public float snapSpeed = 5f;  // The speed of snapping

    private Rigidbody rb;
    private bool _isJumpingUp = false;
    private Vector3 targetPoint = Vector3.zero;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        //_touchInputHandler.SwipeDetected += OnSwipeDetected;
        _swipeListener.OnSwipe.AddListener(OnSwipeDetected);
        //GameController.Instance.PlayerLanded += OnPlayerLanded;
    }

    private void OnPlayerLanded()
    {
        _isJumpingUp = false;
    }

    private void OnDisable()
    {
        //_touchInputHandler.SwipeDetected -= OnSwipeDetected;
        _swipeListener.OnSwipe.RemoveListener(OnSwipeDetected);
    }

    private void Update()
    {
       //Debug.Log(Vector3.Distance(transform.position, targetPoint));
        if (_isJumpingUp && Vector3.Distance(transform.position, targetPoint) <= snapDistance)
        {
            SnapToTarget();
        }
    }

    private void OnSwipeDetected(string str)
    {
        Vector3 force = Vector3.zero;
        Quaternion targetRotation = new Quaternion(0,0,0,0);

        Debug.Log(str);

        switch(str)
        {
            case "Up":
                {
                    if (!_isJumpingUp)
                    {
                        targetPoint = transform.position + _targetOffset;
                        targetRotation = Quaternion.LookRotation(Vector3.forward);
                        force += (_jumpOffset * Vector3.up + Vector3.forward);
                        _isJumpingUp = true;
                    }
                    else
                        Debug.Log("Iam Jumping");
                    break;
                }
            case "Down":
                {
                    targetRotation = Quaternion.LookRotation(Vector3.forward);
                    force += (Vector3.back + Vector3.up);
                    break;
                }
            case "Left":
                {
                    targetRotation = Quaternion.LookRotation(Vector3.left);
                    force += (Vector3.left + Vector3.up);
                    break;
                }
            case "Right":
                {
                    targetRotation = Quaternion.LookRotation(Vector3.right);
                    force += (Vector3.right + Vector3.up);
                    break;
                }
        }
        //rb.AddForce(force * _forceValue/*,ForceMode.Impulse*/);
        rb.velocity = force * _forceValue;
        _player.transform.rotation = targetRotation;
    }

    void SnapToTarget()
    {
        Debug.Log("Snapping");
        rb.velocity = Vector3.zero;  // Stop any movement
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, snapSpeed * Time.deltaTime);

        // Optionally, set isJumping to false if you want to stop further checks
        if (Vector3.Distance(transform.position, targetPoint) < 0.01f)
        {
            _isJumpingUp = false;
        }
    }

    public void OnActionButtonPressed(string key)
    {
        OnSwipeDetected(key);
    }
}
