using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;
    public static Action _onGameOver;

    [SerializeField] private Transform _target;
    [SerializeField] private List<Transform> _spawnerList;
    [SerializeField] private List<Enemy> _enemyList;
    [SerializeField] private LifeBarUIDisplay _lifeBarUi;
    [SerializeField] private int _startingLife = 25;

    private int _currentLife = 20;

    private void Awake()
    {
        if(_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        Enemy._onReachTheEnd += LoseALife;
    }

    private void Start()
    {
        _lifeBarUi.UpdateLifeBar(_currentLife, _startingLife);
    }

    [Button]
    public void SpawnRandomEnemy()
    {
        int randomEnemyIndex = UnityEngine.Random.Range(0, _enemyList.Count);
        int randomSpawnerIndex = UnityEngine.Random.Range(0, _spawnerList.Count);
        Instantiate(_enemyList[randomEnemyIndex], _spawnerList[randomSpawnerIndex].position, Quaternion.identity);
    }

    public Vector3 GetTargetPosition()
    {
        return _target.position;
    }

    private void LoseALife()
    {
        _currentLife--;
        if (_currentLife <= 0)
        {
            _currentLife = 0;
            _onGameOver?.Invoke();
        }
            

        _lifeBarUi.UpdateLifeBar(_currentLife,_startingLife);
    }


}
