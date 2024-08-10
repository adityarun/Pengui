using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickController : MonoBehaviour
{
    public List<GameObject> pools;
    [SerializeField] private float duration = 1f;
    [SerializeField] private LoLTimer _brickTimer;
    [SerializeField] private Vector3 _startLocation;
    [SerializeField] private Queue<GameObject> poolQueue;

    void Start()
    {
        poolQueue = new Queue<GameObject>();
        _brickTimer.TimerComplete += onTimerComplete;

        foreach (GameObject pool in pools)
        {
            poolQueue.Enqueue(pool);
        }

        _brickTimer.StartTimer(duration);
    }

    private void onTimerComplete()
    {
        SpawnFromPool(new Vector3(0f, 12.8f, -3.66f), Quaternion.identity);
    }

    private void StartPool()
    {
        SpawnFromPool(_startLocation, Quaternion.identity);
    }

    public void SpawnFromPool(Vector3 position, Quaternion rotation)
    {
        GameObject objectToSpawn = poolQueue.Dequeue();

        objectToSpawn.GetComponent<BrickMovement>().IsReset = true;
        objectToSpawn.SetActive(false);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.SetActive(true);

        poolQueue.Enqueue(objectToSpawn);
        _brickTimer.StartTimer(duration);
    }
}
