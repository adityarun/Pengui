using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickMovement : MonoBehaviour
{
    public float angleDegreesXY = 0f;       // The angle in degrees in the XY plane
    public float angleDegreesXZ = 30f;      // The angle in degrees in the XZ plane (0 means no movement in Z direction)
    public float speed = 5f;                // Movement speed
    public bool IsReset
    {
        get { return isReset; }
        set { isReset = value; }
    }

    private Vector3 direction;
    private bool isReset = false;

    private void OnEnable()
    {
        isReset = false;
        SetUpDirection();
    }

    private void SetUpDirection()
    {
        // Convert the angles from degrees to radians
        float angleRadiansXY = angleDegreesXY * Mathf.Deg2Rad;
        float angleRadiansXZ = angleDegreesXZ * Mathf.Deg2Rad;

        // Calculate the direction vector based on the angles
        float x = Mathf.Sin(angleRadiansXY);
        float y = Mathf.Cos(angleRadiansXY) * Mathf.Cos(angleRadiansXZ);
        float z = Mathf.Cos(angleRadiansXY) * Mathf.Sin(angleRadiansXZ);
        direction = new Vector3(x, y, z).normalized;        // Normalize to ensure consistent speed
    }

    void Update()
    {
        // Move the object in the calculated direction
        if(!isReset)
        {
            transform.position += (-direction) * speed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == GameController.Instance.PenguinPlayer)
        {
            GameController.Instance.PlayerLanded?.Invoke();
        }
    }
}
