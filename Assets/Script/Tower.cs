using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public enum TowerType
    {
        IceTower,
        FireTower,
        ArrowTower,
        DeathTower,
        Wall,
    }
    /*
    public enum AttackType
    {
        Splash,
        SingleTarget,
        Split,
    }

    public enum TargetType
    {
        Closest,
        HighestLife,
    }
    */
    [SerializeField] protected Projectile _projectile;
    [SerializeField] private List<TowerStats> _towerStats;
    [SerializeField] private TowerType _type;
    [SerializeField] private GameObject _towerVisual;

    private int _price;
    private int _damage = 10;
    private float _attackDelay;
    private float _attackRange;
    private float _AOERange;
    private float _attackTimer;
    private bool _hasBeenBuilt = false;

    private List<Enemy> _enemyList;
    private int _towerLvl;

    protected virtual void Start()
    {
        //UpdateTower();
        
    }

    public void UpgradeTower(TowerType type)
    {
        Debug.Log("3");
        if (_type==type)
        {
            UpdateTower();
        }
    }
    public void UpdateTower()
    {
        if(_towerVisual!=null)
        {
            Destroy(_towerVisual);
        }

        _towerLvl = GameManager._instance.GetTowerLvl(_type);
        TowerStats stats = _towerStats[_towerLvl];
        _towerVisual = Instantiate(stats._visual, transform.position, Quaternion.identity);
        _towerVisual.transform.SetParent(transform);
        _price = stats._price;
        _damage = stats._damage;
        _attackDelay = stats._attackDelay;
        _attackRange = stats._attackRange;
        _projectile = stats._projectile;
    }

    public TowerStats GetTowerStats()
    {
        return _towerStats[GameManager._instance.GetTowerLvl(_type)];
    }

    protected virtual void Update()
    {
        if (_hasBeenBuilt)
        {
            _attackTimer += Time.deltaTime;
            _enemyList = GameManager._instance.GetActiveEnemies();

            if (_attackTimer >= _attackDelay)
            {
                if (EnemyIsInRange())
                {
                    Attack(GetTarget());
                }
            }
        }
    }

    public void Build()
    {
        _hasBeenBuilt = true;
        GameManager._onUpdateTower += UpgradeTower;
   
        UpdateTower();
    }

    public int GetPrice()
    {
        return _price;
    }

    protected virtual void Attack(Enemy enemy)
    {
        if (enemy == null) return;

        Projectile newProjectile = Instantiate(_projectile, (transform.position + 2 * Vector3.up), Quaternion.identity);
        newProjectile.Init(_damage, enemy.transform);
        _attackTimer = 0f;
    }

    private bool EnemyIsInRange()
    {
        foreach (Enemy enemy in _enemyList)
        {
            if ((enemy.transform.position - transform.position).magnitude <= _attackRange)
            {
                return true;
            }
        }
        return false;
    }

    protected virtual Enemy GetTarget()
    {
        Enemy target = null;
        float minDist = 10000f;
        foreach (Enemy enemy in _enemyList)
        {
            float dist = (enemy.transform.position - transform.position).magnitude;
            if (dist <= minDist && dist <= _attackRange)
            {
                target = enemy;
                minDist = dist;
            }
        }
        return target;
    }

    public TowerType GetTowerType()
    {
        return _type;
    }
}
