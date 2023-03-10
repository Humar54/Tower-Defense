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
    public static Action<Tower.TowerType> _onUpdateTower;

    [SerializeField] private Transform _target;
    [SerializeField] private List<Transform> _spawnerList;
    [SerializeField] private List<EnemyList> _enemyList;

    [SerializeField] private LifeBarUIDisplay _lifeBarUi;
    [SerializeField] private TextMeshProUGUI _waveText;
    [SerializeField] private int _startingLife = 25;
    [SerializeField] private float _minSpawnDelay, _maxSpawnDelay;

    private List<Enemy> _activeEnemyList = new List<Enemy>();
    [SerializeField] private IDictionary<Tower.TowerType, int> _towerLvlDict = new Dictionary<Tower.TowerType, int>();
    private int _currentLife = 20;
    private int _waveEnemyIndex;
    private int _nbEnemyToKill;
    private int _nbEnemyToSpawn;
    private int _level = -1;
    private float _spawnDelayMin;
    private float _spawnDelayMax;

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
        Enemy._onDeath += RemoveAnEnnemy;

        _towerLvlDict.Add(Tower.TowerType.ArrowTower, 0);
        _towerLvlDict.Add(Tower.TowerType.Wall, 0);
        _towerLvlDict.Add(Tower.TowerType.DeathTower, 0);
        _towerLvlDict.Add(Tower.TowerType.FireTower, 0);
        _towerLvlDict.Add(Tower.TowerType.IceTower, 0);
    }

    private void Start()
    {
        _lifeBarUi.UpdateLifeBar(_currentLife, _startingLife);
        StartCoroutine(NewWave());
    }

    private void RemoveAnEnnemy(int value, Enemy enemy)
    {
        _activeEnemyList.Remove(enemy);
        _nbEnemyToKill--;
        if (_nbEnemyToKill <= 0)
        {
            StartCoroutine(NewWave());
        }
    }

    public List<Enemy> GetActiveEnemies()
    {
        return _activeEnemyList;
    }

    [Button]
    public IEnumerator SpawnEnemy()
    {
        float randomDelay = UnityEngine.Random.Range(_minSpawnDelay, _maxSpawnDelay);
        yield return new WaitForSeconds(randomDelay);

        if (_nbEnemyToSpawn >= 1)
        {
            int randomSpawnerIndex = UnityEngine.Random.Range(0, _spawnerList.Count);
            _nbEnemyToSpawn--;
            Enemy newEnemy = Instantiate(_enemyList[_level]._list[_waveEnemyIndex], _spawnerList[randomSpawnerIndex].position, Quaternion.identity);
            _activeEnemyList.Add(newEnemy);
            _waveEnemyIndex++;
            StartCoroutine(SpawnEnemy());
        }
    }

    private IEnumerator NewWave()
    {
        _level++;
        if (_level < _enemyList.Count)
        {
            _waveEnemyIndex = 0;
            _nbEnemyToKill = _enemyList[_level]._list.Count;
            _nbEnemyToSpawn = _enemyList[_level]._list.Count;
            _waveText.text = $"Wave {_level}";
            _waveText.transform.parent.gameObject.SetActive(true);
            RessourceManager._instance.ReceiveMoney(100);
            yield return new WaitForSeconds(2f);
            _spawnDelayMin = _enemyList[_level]._minSpawndelay;
            _spawnDelayMax = _enemyList[_level]._maxSpawnDelay;
            _waveText.transform.parent.gameObject.SetActive(false);
            StartCoroutine(SpawnEnemy());
        }

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
        RemoveAnEnnemy(0, enemy);
        _lifeBarUi.UpdateLifeBar(_currentLife, _startingLife);
    }

    public int GetTowerLvl(Tower.TowerType towerType)
    {
        return _towerLvlDict[towerType];
    }

    public void IncreaseTowerLvl(Tower.TowerType towerType)
    {
        _towerLvlDict[towerType]++;
        _onUpdateTower?.Invoke(towerType);
    }
}
