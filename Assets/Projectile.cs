using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public enum DamageType
    {
        Fire,
        Ice,
        Normal,
    }

    private Transform _target;
    private int _damage;

    [SerializeField] private Rigidbody _rb;
    [SerializeField] private GameObject _spawnVFX;
    [SerializeField] private GameObject _collisionVFX;

    public DamageType _damageType;

    [SerializeField] private float _projectileSpeed;
    [SerializeField] private float _selfDestroyDelay = 10f;
    [SerializeField] public float _AOERange;
    [SerializeField] public float _slowDelay = 5f;

    private bool _hasBurst = false;
    public void Init(int damage, Transform target)
    {
        _damage = damage;
        _target = target;
        GameObject spawnVFX = Instantiate(_spawnVFX, transform.position, Quaternion.identity);
        Destroy(spawnVFX, 2f);
        Destroy(gameObject, _selfDestroyDelay);
    }

    private void Update()
    {
        if (_target == null && !_hasBurst)
        {
            Destroy(gameObject);
            _hasBurst = true;
            return;
        }
        _rb.velocity = (_target.position - transform.position).normalized * _projectileSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == _target)
        {
            if (_AOERange > 1)
            {
                Collider[] colliderArray = Physics.OverlapSphere(transform.position, _AOERange);
                Burst();
                foreach (Collider collider in colliderArray)
                {
                    Enemy enemy = collider.GetComponent<Enemy>();

                    if (enemy != null)
                    {
                        enemy.TakeDamage(_damageType, _damage);

                        if (_damageType == DamageType.Ice)
                        {
                            enemy.Slow(_slowDelay);
                        }
                    }
                }
            }
            else
            {
                other.GetComponent<Enemy>().TakeDamage(_damageType, _damage);
                Burst();
                if (_damageType == DamageType.Ice)
                {
                    other.GetComponent<Enemy>().Slow(_slowDelay);
                }
                
            }
        }
    }

    private void Burst()
    {
        GameObject collisionVFX = Instantiate(_collisionVFX, transform.position, Quaternion.identity);
        float previousScale = collisionVFX.transform.localScale.x;
        collisionVFX.transform.localScale = new Vector3(previousScale, previousScale, previousScale) * _AOERange;
        Destroy(collisionVFX, 2f);
        Destroy(gameObject);
    }

    
}
