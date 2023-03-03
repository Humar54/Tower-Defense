
using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    [SerializeField] private int _cashValue =10;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Animator _animator;

    [SerializeField] private float _runSpeed = 7.0f;
    [SerializeField] private float _walkSpeed = 3.5f;
    [SerializeField] private bool _isRunning;
    

    public static Action<int> _onDeath;
    public static Action _onReachTheEnd;

    void Start()
    {
        _agent.SetDestination(GameManager._instance.GetTargetPosition());
        if(_isRunning)
        {
            _agent.speed = _runSpeed;
        }
    }

    private void Update()
    {
        _animator.SetFloat("_Speed", _agent.velocity.magnitude/ _runSpeed);
    }

    public void ReachTheEnd()
    {
        _onReachTheEnd?.Invoke();
        Destroy(gameObject);
    }
}
