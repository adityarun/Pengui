using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum TouchActionPerformmed
{
    none,
    up,
    down,
    right,
    left
}

public class TouchInputHandler : MonoBehaviour
{
    public float minSwipeDistance = 0.2f; // Minimum swipe distance to be considered a swipe
    private Vector2 startTouchPosition;
    private Vector2 currentTouchPosition;
    private bool startSwiping = false;
    private bool isSwiping = false;
    private TouchActionPerformmed currentTouchAction = TouchActionPerformmed.none;

    private TouchControls touchControls;
    private int touchCount = 0;

    #region Actions

    public Action<string> SwipeDetected;

    #endregion Actions

    private void Awake()
    {
        touchControls = new TouchControls();
    }

    private void OnEnable()
    {
        touchControls.Enable();
        touchControls.Touch.PrimaryTouch.performed += ctx => OnTouchPerformed(ctx);
        touchControls.Touch.TouchZero.performed += ctx => OnTouchEnded(ctx);
        touchControls.Touch.PrimaryTouch.canceled += ctx => OnTouchCanceled(ctx);
    }

    private void OnDisable()
    {
        touchControls.Touch.PrimaryTouch.performed -= ctx => OnTouchPerformed(ctx);
        touchControls.Touch.TouchZero.performed -= ctx => OnTouchEnded(ctx);
        touchControls.Touch.PrimaryTouch.canceled -= ctx => OnTouchCanceled(ctx);
        touchControls.Disable();
    }

    private void OnTouchEnded(InputAction.CallbackContext context)
    {
        touchCount++;
        if(touchCount > 2)
        {
            //Debug.Log("H22");
            currentTouchAction = TouchActionPerformmed.none;
            touchCount = 0;
        }
    }

    private void OnTouchCanceled(InputAction.CallbackContext context)
    {
        startSwiping = false;
    }

    private void OnTouchPerformed(InputAction.CallbackContext context)
    {
        if (!startSwiping)
        {
            startSwiping = true;
            startTouchPosition = touchControls.Touch.PrimaryTouch.ReadValue<Vector2>();
        }
        else if(!isSwiping)
        {
            isSwiping = true;
            currentTouchPosition = touchControls.Touch.PrimaryTouch.ReadValue<Vector2>();
            DetectSwipe();
        }
    }

    private void DetectSwipe()
    {
        Vector2 swipeVector = currentTouchPosition - startTouchPosition;
        float swipeDistance = swipeVector.magnitude;


        //Debug.Log("SwipDistance: " + swipeDistance);

        //if (swipeDistance >= minSwipeDistance * Screen.dpi)
        {
            //Debug.Log("H1111");
            float x = swipeVector.x;
            float y = swipeVector.y;

            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                if (x > 0)
                {
                    OnSwipeRight();
                }
                else
                {
                    OnSwipeLeft();
                }
            }
            else
            {
                if (y > 0)
                {
                    OnSwipeUp();
                }
                else
                {
                    OnSwipeDown();
                }
            }
        }
        startSwiping = false;
        isSwiping = false;
    }

    private void OnSwipeUp()
    {
        if (currentTouchAction == TouchActionPerformmed.up)
            return;

        if(currentTouchAction == TouchActionPerformmed.none)
        {
            SwipeDetected?.Invoke("Up");
            currentTouchAction = TouchActionPerformmed.up;
            //Debug.Log("Swipe Up Detected");
        }
    }

    private void OnSwipeDown()
    {
        if (currentTouchAction == TouchActionPerformmed.down)
            return;
        if (currentTouchAction == TouchActionPerformmed.none)
        {
            SwipeDetected?.Invoke("Down");
            currentTouchAction = TouchActionPerformmed.down;
            //Debug.Log("Swipe Down Detected");
        }
    }

    private void OnSwipeLeft()
    {
        if (currentTouchAction == TouchActionPerformmed.left)
            return;
        if (currentTouchAction == TouchActionPerformmed.none)
        {
            SwipeDetected?.Invoke("Left");
            currentTouchAction = TouchActionPerformmed.left;
            //Debug.Log("Swipe Left Detected");
        }
    }

    private void OnSwipeRight()
    {
        if (currentTouchAction == TouchActionPerformmed.right)
            return;
        if (currentTouchAction == TouchActionPerformmed.none)
        {
            SwipeDetected?.Invoke("Right");
            currentTouchAction = TouchActionPerformmed.right;
            //Debug.Log("Swipe Right Detected");
        }
    }
}
