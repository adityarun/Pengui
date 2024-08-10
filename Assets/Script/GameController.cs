using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DevCommon;
using UnityEngine.UI;

public class GameController : Singleton<GameController>
{
    public GameObject PenguinPlayer;

    [SerializeField] private GameObject _birdEnemy;
    [SerializeField] private Transform[] _birdPositionArray;

    [SerializeField] private GameObject _keyPrefab;
    [SerializeField] private Transform[] _keyPositionArray;
    [SerializeField] private int _keyCount = 3;

    [SerializeField] private LoLTimer _timer;
    [SerializeField] private int _timeLimit = 60;

    [SerializeField] private TMPro.TMP_Text _scoreText;
    [SerializeField] private Button _reLoadButton;

    private Queue<GameObject> _enemyPool;
    private int _score = 0;
    private List<int> _numbers;

    public Action KeyDetected;
    public Action GameOver;
    public Action PlayerLanded;

    private void Start()
    {
        _enemyPool = new Queue<GameObject>();
        _numbers = new List<int>();
        _reLoadButton.gameObject.SetActive(false);

        foreach (Transform transform in _birdPositionArray)
        {
            GameObject bird = Instantiate(_birdEnemy, transform) as GameObject;
            Quaternion rotation = Quaternion.LookRotation(Vector3.right);
            bird.transform.rotation = rotation;
            _enemyPool.Enqueue(bird);
        }

        for(int i = 0; i<_keyPositionArray.Length; i++)
        {
            _numbers.Add(i);
        }

        Shuffle(_numbers);

        for(int i = 0; i < _keyCount; i++)
        {
            int pos = _numbers[i];
            GameObject key = Instantiate(_keyPrefab, _keyPositionArray[pos]) as GameObject;
            key.transform.position = _keyPositionArray[pos].position;
        }

        _timer.StartTimer(_timeLimit);
        _timer.TimerComplete += onTimerComplete;
        KeyDetected += setScore;
        GameOver += setRelaodButton;
    }

    private void Shuffle(List<int> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);
            int temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    private void setScore()
    {
        _score++;
        _scoreText.text = _score.ToString();
    }

    private void setRelaodButton()
    {
        _reLoadButton.gameObject.SetActive(true);
    }

    private void onTimerComplete()
    {
        SpawnBirdsFromPool();
    }

    private void SpawnBirdsFromPool()
    {
        foreach(Transform transform in _birdPositionArray)
        {
            GameObject objectToSpawn = _enemyPool.Dequeue();

            Bird bird = objectToSpawn.GetComponent<Bird>();
            if(bird != null)
            {
                bird.IsMoving = false;
                bird.StopTimer();
            }

            objectToSpawn.transform.position = transform.position;
            Quaternion rotation = Quaternion.LookRotation(Vector3.left);
            objectToSpawn.transform.rotation = rotation;

            if (bird != null)
            {
                bird.IsMoving = true;
                bird.StartTimer();
            }

            _enemyPool.Enqueue(objectToSpawn);
            _timer.StartTimer(_timeLimit);
        }
    }

    public void OnClickReload()
    {
        Application.LoadLevel("Gamescene01");
    }
}
