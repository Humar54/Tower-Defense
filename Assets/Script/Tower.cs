using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private int _price;
    [SerializeField] private int _damage=10;
    [SerializeField] private float _attackDelay;
    [SerializeField] private float _attackRange;

    private float _attackTimer;

    [SerializeField] protected Projectile _projectile;

    private List<Enemy> _enemyList;
    protected virtual void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
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

    public int GetPrice()
    {
        return _price;
    }

    protected virtual void Attack(Enemy enemy)
    {
        if (enemy == null) return;

        Projectile newProjectile = Instantiate(_projectile, (transform.position + 2*Vector3.up), Quaternion.identity);
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
}
