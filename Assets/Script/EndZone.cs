using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Enemy currentEnemy = other.GetComponent<Enemy>();
        if (currentEnemy != null)
        {
            currentEnemy.ReachTheEnd();
        }
    }
}
