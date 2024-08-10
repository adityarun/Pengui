using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == GameController.Instance.PenguinPlayer)
        {
            transform.gameObject.SetActive(false);
            GameController.Instance.KeyDetected?.Invoke();
        }
    }
}
