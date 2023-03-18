using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTower : Tower
{
    protected override Enemy GetTarget()
    {
        Enemy target = null;
        float maxHealth = -10000f;
        foreach (Enemy enemy in _enemyList)
        {
            if (enemy != null)
            {
                int health = enemy.GetCurrentHealth();
                float dist = (enemy.transform.position - transform.position).magnitude;
                if (health >= maxHealth && dist <= _attackRange)
                {
                    target = enemy;
                    maxHealth = health;
                }
            }
        }
        return target;
    }
}
