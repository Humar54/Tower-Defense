using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndZone : MonoBehaviour
{
    [SerializeField] private GameObject _endVFX;
    private void OnTriggerEnter(Collider other)
    {
        Enemy currentEnemy = other.GetComponent<Enemy>();
        if (currentEnemy != null)
        {
            currentEnemy.ReachTheEnd();
            Instantiate(_endVFX, transform.position, Quaternion.identity);
        }
    }
}
