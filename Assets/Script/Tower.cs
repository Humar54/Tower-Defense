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
    public int _towerLvl;

    [SerializeField] protected Projectile _projectile;
    [SerializeField] private List<TowerStats> _towerStats;
    [SerializeField] private TowerType _type;
    [SerializeField] private GameObject _towerVisual;

    protected List<Enemy> _enemyList;

    private int _price;
    private int _damage = 10;
    protected float _attackDelay;
    protected float _attackRange;
    private float _AOERange;
    protected float _attackTimer;
    protected bool _hasBeenBuilt = false;
    private Enemy _target;



    protected virtual void Start()
    {
        //UpdateTower();

        _attackTimer = _attackDelay;

    }

    public void UpgradeTower(TowerType type)
    {
        if (_type == type)
        {
            UpdateTower();
        }
    }


    public void UpdateTower()
    {
        Destroy(_towerVisual.transform.GetChild(0).gameObject);
        _towerLvl = GameManager._instance.GetTowerLvl(_type);
        TowerStats stats = _towerStats[_towerLvl];
        GameObject newTower = Instantiate(stats._visual, transform.position, Quaternion.identity);
        newTower.transform.SetParent(_towerVisual.transform);
        _price = stats._price;
        _damage = stats._damage;
        _attackDelay = stats._attackDelay;
        _attackRange = stats._attackRange;
        _projectile = stats._projectile;
    }

    public TowerStats GetTowerStats(int offset)
    {
        if (GameManager._instance.GetTowerLvl(_type) + offset < _towerStats.Count)
        {
            return _towerStats[GameManager._instance.GetTowerLvl(_type) + offset];
        }
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
                    if (_target != null)
                    {
                        if ((_target.transform.position - transform.position).magnitude <= _attackRange && !_target.GetIsDead())
                        {
                            Attack(_target, transform.position + 2 * Vector3.up);
                        }
                        else
                        {
                            _target = GetTarget();
                            Attack(_target, transform.position + 2 * Vector3.up);
                        }
                    }
                    else
                    {
                        _target = GetTarget();
                        Attack(_target, transform.position + 2 * Vector3.up);
                    }
                }
            }
        }
    }

    public void Build()
    {
        _hasBeenBuilt = true;
        GameManager._onUpdateTower += UpgradeTower;
        GetComponent<Collider>().enabled = true;

        UpdateTower();
    }

    public int GetPrice()
    {
        return _price;
    }

    protected virtual void Attack(Enemy enemy, Vector3 position)
    {
        if (enemy == null) return;

        Projectile newProjectile = Instantiate(_projectile, transform.position + 2 * Vector3.up, Quaternion.identity);
        newProjectile.Init(_damage, enemy.transform);
        _attackTimer = 0f;
    }

    protected bool EnemyIsInRange()
    {
        foreach (Enemy enemy in _enemyList)
        {
            if (enemy == null) { continue; }
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
            if (enemy != null)
            {
                float dist = (enemy.transform.position - transform.position).magnitude;
                if (dist <= minDist && dist <= _attackRange)
                {
                    target = enemy;
                    minDist = dist;
                }
            }
        }
        return target;
    }

    public TowerType GetTowerType()
    {
        return _type;
    }
    public virtual string GetToolTip(int offset)
    {
        switch (_type)
        {
            case TowerType.IceTower:
                return $"Hurls ice projectiles to the closest enemy causing  damage in a {GetProjectile(offset)._AOERange}m radius  and slowing enemies for {GetProjectile(offset)._slowDelay}s";
            case TowerType.FireTower:
                return $"Hurls fire ball to the closest enemy causing damage in a {GetProjectile(offset)._AOERange}m radius ";
            case TowerType.ArrowTower:
                return $"Hurls bolt to the closest enemy causing damage to target";
            case TowerType.DeathTower:
                return $"Hurls death ball to the strongest enemy causing massive damage to target";
            case TowerType.Wall:
                return $"Block enemy movement";
            default:
                break;
        }
        return "";
    }

    public Projectile GetProjectile(int offset)
    {
        return GetTowerStats(offset)._projectile;
    }
}
