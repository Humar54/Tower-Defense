using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;
    public static Action _onGameOver;

    [SerializeField] private Transform _target;
    [SerializeField] private List<Transform> _spawnerList;
    [SerializeField] private List<EnemyList> _enemyList;

    [SerializeField] private LifeBarUIDisplay _lifeBarUi;
    [SerializeField] private TextMeshProUGUI _waveText;
    [SerializeField] private int _startingLife = 25;
    [SerializeField] private float _minSpawnDelay, _maxSpawnDelay;

    private List<Enemy> _activeEnemyList = new List<Enemy>();
    private int _currentLife = 20;
    private int _waveEnemyIndex;
    private int _level = 0;
    private float _spawnDelay;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        Enemy._onReachTheEnd += LoseALife;
        Enemy._onDeath += RemoveAEnnemy;
    }

    private void Start()
    {
        _lifeBarUi.UpdateLifeBar(_currentLife, _startingLife);
        StartCoroutine(SwitchToNextLevel());
    }

    private void RemoveAEnnemy(int value, Enemy enemy)
    {
        _activeEnemyList.Remove(enemy);
        if (_waveEnemyIndex >= (_enemyList[_level]._list.Count-1) )
        {
            StartCoroutine(SwitchToNextLevel());
        }
    }

    public List<Enemy> GetActiveEnemies()
    {
        return _activeEnemyList;
    }

    [Button]
    public IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(_spawnDelay);

        int randomSpawnerIndex = UnityEngine.Random.Range(0, _spawnerList.Count);
        Enemy newEnemy = Instantiate(_enemyList[_level]._list[_waveEnemyIndex], _spawnerList[randomSpawnerIndex].position, Quaternion.identity);
        _activeEnemyList.Add(newEnemy);
        
        if(_waveEnemyIndex < _enemyList[_level]._list.Count)
        {
            _waveEnemyIndex++;
            StartCoroutine(SpawnEnemy());
        }
         
    }

    private IEnumerator SwitchToNextLevel()
    {
        _waveText.text = $"Wave {_level}";
        _waveText.transform.parent.gameObject.SetActive(true);
        RessourceManager._instance.ReceiveMoney(100);

            _level++;
        

        yield return new WaitForSeconds(2f);
        _spawnDelay = Mathf.Lerp(_maxSpawnDelay, _minSpawnDelay, (float)_level / 10f);
        _waveEnemyIndex = 0;
        _waveText.transform.parent.gameObject.SetActive(false);

        StartCoroutine(SpawnEnemy());
    }




    public Vector3 GetTargetPosition()
    {
        return _target.position;
    }

    private void LoseALife(Enemy enemy)
    {
        _currentLife--;
        if (_currentLife <= 0)
        {
            _currentLife = 0;
            _onGameOver?.Invoke();
        }
        RemoveAEnnemy(0, enemy);
        _lifeBarUi.UpdateLifeBar(_currentLife, _startingLife);
    }


}
