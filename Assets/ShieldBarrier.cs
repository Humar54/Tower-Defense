using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBarrier : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if(enemy != null)
        {
            enemy.Stop();
        }
    }

    private void OnDisable()
    {
        List<Enemy> enemyList = GameManager._instance.GetActiveEnemies();
        foreach (Enemy enemy in enemyList)
        {
            if(enemy!=null)
            {
                enemy.Move();
                enemy.SetSpeed();
            }
        }
    }
}
