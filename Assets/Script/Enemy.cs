
using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    [SerializeField] private int _cashValue =10;
    [SerializeField] private int _startingHealth = 100;

    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Animator _animator;

    [SerializeField] private float _runSpeed = 7.0f;
    [SerializeField] private float _walkSpeed = 3.5f;
    [SerializeField] private bool _isRunning;
    

    public static Action<int,Enemy> _onDeath;
    public static Action<Enemy> _onReachTheEnd;
    private int _currentHealth;
    void Start()
    {
        _agent.SetDestination(GameManager._instance.GetTargetPosition());
        if(_isRunning)
        {
            _agent.speed = _runSpeed;
        }
        _currentHealth = _startingHealth;
    }

    private void Update()
    {
        _animator.SetFloat("_Speed", _agent.velocity.magnitude/ _runSpeed);
    }

    public void TakeDamage(int damage)
    {
        _currentHealth-= damage;
        Debug.Log("HasReceivedDamage");

        if(_currentHealth<=0)
        {
            _currentHealth = 0;
            Death();
        }
    }

    public void Death()
    {
        _onDeath?.Invoke(_cashValue,this);
        Destroy(gameObject);
    }

    public void ReachTheEnd()
    {
        _onReachTheEnd?.Invoke(this);
        Destroy(gameObject);
    }
}
