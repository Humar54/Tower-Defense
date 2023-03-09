using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    private Transform _target;
    private int _damage;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _projectileSpeed;
    [SerializeField] private GameObject _spawnVFX;
    [SerializeField] private GameObject _collisionVFX;


    public void Init(int damage, Transform target)
    {
        _damage = damage;
        _target = target;
        GameObject spawnVFX = Instantiate(_spawnVFX, transform.position, Quaternion.identity);
        Destroy(spawnVFX, 2f);
    }

    private void Update()
    {
        if (_target == null) { return; }
        _rb.velocity = (_target.position - transform.position).normalized * _projectileSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == _target)
        {
            other.GetComponent<Enemy>().TakeDamage(_damage);
            GameObject collisionVFX = Instantiate(_collisionVFX, transform.position, Quaternion.identity);
            Destroy(collisionVFX, 2f);
            Destroy(gameObject);
        }
    }
}
