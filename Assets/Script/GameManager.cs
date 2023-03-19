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

    //[SerializeField] private IDictionary<Tower.TowerType, int> _towerLvlDict = new Dictionary<Tower.TowerType, int>();
    private int _currentLife;
    private int _waveEnemyIndex;
    [SerializeField] private int _level = -1;
    private float _spawnDelayMin;
    private float _spawnDelayMax;


    private int _archerTowerLvl=0;
    private int _deathTowerLvl=0;
    private int _fireTowerLvl=0;
    private int _iceTowerLvl=0;
    

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
        Enemy._onReachTheEnd += LoseLife;
        Enemy._onDeath += RemoveAnEnnemy;
        _onGameOver += GameOver;
    }

    private void OnDestroy()
    {
        Enemy._onReachTheEnd -= LoseLife;
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
        for (int i = 0; i < _activeEnemyList.Count; i++)
        {
            if (_activeEnemyList[i]==null)
            {
                _activeEnemyList.RemoveAt(i);
            }
        }
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
            
            _waveText.text = $"Wave {_level+1}";
            _waveText.transform.parent.gameObject.SetActive(true);
            yield return new WaitForSeconds(2f);
            RessourceManager._instance.ReceiveMoney(_enemyList[_level]._waveBonus);
            yield return new WaitForSeconds(4f);
            _spawnDelayMin = _enemyList[_level]._minSpawndelay;
            _spawnDelayMax = _enemyList[_level]._maxSpawnDelay;
            _waveText.transform.parent.gameObject.SetActive(false);
            StartCoroutine(SpawnEnemy());
        }
        else
        {
            _waveText.text = $"You Win!";
            _waveText.transform.parent.gameObject.SetActive(true);
        }
    }

    public Vector3 GetTargetPosition()
    {
        return _target.position;
    }

    private void LoseLife(Enemy enemy)
    {
        _currentLife-=enemy.GetLifeCost();
        if (_currentLife <= 0)
        {
            _currentLife = 0;
            Enemy._onReachTheEnd -= LoseLife;
            _onGameOver?.Invoke();
        }

        if(_level >= _enemyList.Count-1 || _currentLife==0)
        {

        }
        else
        {
            RemoveAnEnnemy(0, enemy);
        }

        _lifeBarUi.UpdateLifeBar(_currentLife, _startingLife);
    }

    public int GetTowerLvl(Tower.TowerType towerType)
    {
        switch (towerType)
        {
            case Tower.TowerType.IceTower:
                return _iceTowerLvl;
            case Tower.TowerType.FireTower:
                return _fireTowerLvl;
            case Tower.TowerType.ArrowTower:
                return _archerTowerLvl;
            case Tower.TowerType.DeathTower:
                return _deathTowerLvl;
            default:
                return 0;
        }
    }

    public void IncreaseTowerLvl(Tower.TowerType towerType)
    {
        switch (towerType)
        {
            case Tower.TowerType.IceTower:
                 _iceTowerLvl++;
                break;
            case Tower.TowerType.FireTower:
                 _fireTowerLvl++;
                break;
            case Tower.TowerType.ArrowTower:
                 _archerTowerLvl++;
                break;
            case Tower.TowerType.DeathTower:
                _deathTowerLvl++;
                break;
            default:
                break;
              
        }
        _onUpdateTower?.Invoke(towerType);
    }
}
