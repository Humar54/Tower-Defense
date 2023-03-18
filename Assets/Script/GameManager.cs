using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;
using TMPro;
using UnityEngine.SceneManagement;

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

    private List<Enemy> _activeEnemyList = new List<Enemy>();
    private List<Enemy> _enemyListToSpawn = new List<Enemy>();
    private List<Enemy> _enemyListToKilled = new List<Enemy>();

    [SerializeField] private IDictionary<Tower.TowerType, int> _towerLvlDict = new Dictionary<Tower.TowerType, int>();
    private int _currentLife;
    private int _waveEnemyIndex;
    private int _level = -1;
    private float _spawnDelayMin;
    private float _spawnDelayMax;

    private void Awake()
    {
        _currentLife = _startingLife;
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        Enemy._onReachTheEnd += LoseALife;
        Enemy._onDeath += RemoveAnEnnemy;
        _onGameOver += GameOver;
        
        _towerLvlDict.Add(Tower.TowerType.ArrowTower, 0);
        _towerLvlDict.Add(Tower.TowerType.Wall, 0);
        _towerLvlDict.Add(Tower.TowerType.DeathTower, 0);
        _towerLvlDict.Add(Tower.TowerType.FireTower, 0);
        _towerLvlDict.Add(Tower.TowerType.IceTower, 0);
    }

    private void OnDestroy()
    {
        Enemy._onReachTheEnd -= LoseALife;
        Enemy._onDeath -= RemoveAnEnnemy;
        _onGameOver -= GameOver;
    }

    private void Start()
    {
        _lifeBarUi.UpdateLifeBar(_currentLife, _startingLife);
        StartCoroutine(NewWave());
    }

    private void GameOver()
    {
        _waveText.text = $"GameOver";
        _waveText.transform.parent.gameObject.SetActive(true);
        StartCoroutine(Reload());

    }

    private IEnumerator Reload()
    {
        yield return new WaitForSeconds(3f);
        StopAllCoroutines();
        LeanTween.cancelAll();
        SceneManager.LoadScene("Demo1");

    }

    private void RemoveAnEnnemy(int value, Enemy enemy)
    {
        _activeEnemyList.Remove(enemy);
        
        if (_enemyListToKilled.Count >= 1)
        {
            _enemyListToKilled.Remove(enemy);
            if (_enemyListToKilled.Count <= 0)
            {
                StartCoroutine(NewWave());
            }
        }
    }

    public List<Enemy> GetActiveEnemies()
    {
        return _activeEnemyList;
    }

    [Button]
    public IEnumerator SpawnEnemy()
    {
        float randomDelay = UnityEngine.Random.Range(_spawnDelayMin, _spawnDelayMax);
        yield return new WaitForSeconds(randomDelay);

        if (_enemyListToSpawn.Count >= 1)
        {
            int randomSpawnerIndex = UnityEngine.Random.Range(0, _spawnerList.Count);
            
            Enemy newEnemy = Instantiate(_enemyListToSpawn[0], _spawnerList[randomSpawnerIndex].position, Quaternion.identity);

            _enemyListToKilled.Add(newEnemy);
            _enemyListToSpawn.RemoveAt(0);

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

            foreach (Enemy enemy in _enemyList[_level]._list)
            {
                _enemyListToSpawn.Add(enemy);
            }
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
            Enemy._onReachTheEnd -= LoseALife;
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
