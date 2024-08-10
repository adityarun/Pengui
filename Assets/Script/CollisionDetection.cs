using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == GameController.Instance.PenguinPlayer)
        {
            Destroy(GameController.Instance.PenguinPlayer);
            GameController.Instance.GameOver?.Invoke();
        }
    }
}
