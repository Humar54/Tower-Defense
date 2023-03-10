
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    [SerializeField] private int _cashValue = 10;
    [SerializeField] private int _startingHealth = 100;

    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Animator _animator;

    [SerializeField] private float _runSpeed = 7.0f;
    [SerializeField] private float _walkSpeed = 3.5f;
    [SerializeField] private int _iceArmor, _fireArmor, normalArmor;

    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private Condition _condition;

    public static Action<int, Enemy> _onDeath;
    public static Action<Enemy> _onReachTheEnd;
    private int _currentHealth;
    private int _iceModArmor, _fireModArmor, _normalModArmor;
    protected bool _isDead;

    public bool _isSlow;
    public bool _isFast;
    private IEnumerator _slowCoroutine;
    private IEnumerator _fastCoroutine;
    private IEnumerator _armorCoroutine;


    void Start()
    {
        _agent.SetDestination(GameManager._instance.GetTargetPosition());
        SetSpeed();

        _currentHealth = _startingHealth;
    }

    private void Update()
    {
        _animator.SetFloat("_Speed", _agent.velocity.magnitude / _runSpeed);
    }

    public void TakeDamage(Projectile.DamageType type, int damage)
    {


        int damageDealt = 0;
        switch (type)
        {
            case Projectile.DamageType.Fire:

                damageDealt = damage - (_fireArmor + _fireModArmor);
                break;
            case Projectile.DamageType.Ice:
                damageDealt = damage - (_iceArmor + _iceModArmor);
                break;
            case Projectile.DamageType.Normal:
                damageDealt = damage - (normalArmor + _normalModArmor);
                break;
            default:
                break;
        }
        if(damageDealt<=0)
        {
            damageDealt = 1;
        }
        Debug.Log(damageDealt);
        _currentHealth -= damageDealt;
        _healthBar.UpdateHealthBar(_startingHealth, _currentHealth);

        if (_currentHealth <= 0 )
        {
            _currentHealth = 0;
            Death();
        }
    }

    private void  SetSpeed()
    {
        if(_isFast&&_isSlow)
        {
            _agent.speed = _walkSpeed;
        }
        else if(_isFast &&!_isSlow)
        {
            _agent.speed = _runSpeed;
        }
        else if(!_isFast && _isSlow)
        {
            _agent.speed = _walkSpeed / 2f;
        }
        else if(!_isFast && !_isSlow)
        {
            _agent.speed = _walkSpeed;
        }

        if(_isDead)
        {
            _agent.speed = 0f;
        }
    }
    public void SetModifier(int iceArmor, int fireArmor, int normalArmor)
    {
        _iceModArmor = iceArmor;
        _fireModArmor = fireArmor;
        _normalModArmor = normalArmor;
    }

    public void Slow(float delay)
    {
        if(_slowCoroutine!=null)
        {
            StopCoroutine(_slowCoroutine);
        }
        
        _isSlow = true;
        _condition.ActivateSlow();
        SetSpeed();
        _slowCoroutine = ResetSlow(delay);
        StartCoroutine(_slowCoroutine);
    }

    public void ArmorBuff(float delay)
    {
        if(_armorCoroutine!=null)
        {
            StopCoroutine(_armorCoroutine);
        }
        SetModifier(10, 10, 10);
        _condition.ActivateArmor();
        _armorCoroutine = ResetArmor(delay);
        StartCoroutine(_armorCoroutine);
    }

    public void Fast(float delay)
    {
        if(_fastCoroutine!=null)
        {
            StopCoroutine(_fastCoroutine);
        }
        
        _isFast = true;
        _condition.ActivateSpeed();
        SetSpeed();
        _fastCoroutine = ResetFast(delay);
        StartCoroutine(_fastCoroutine);
    }

    private IEnumerator ResetFast(float delay)
    {
        yield return new WaitForSeconds(delay);
        _isFast = false;
        SetSpeed();
        _condition.DisactivateSpeed();
    }

    private IEnumerator ResetArmor(float delay)
    {
        yield return new WaitForSeconds(delay);
        SetModifier(0, 0, 0);
        _condition.DisactivateArmor();
    }

    private IEnumerator ResetSlow(float delay)
    {
        yield return new WaitForSeconds(delay);
        _isSlow = false;
        SetSpeed();
        _condition.DisactivateSlow();
    }

    public virtual void Death()
    {
        if(_isDead==false)
        {
            StopAllCoroutines();
            _onDeath?.Invoke(_cashValue, this);
            _animator.SetBool("_Death", true);
            _isDead = true;
            SetSpeed();
            Destroy(gameObject, 4f);
        }
    }

    public void ReachTheEnd()
    {
        _onReachTheEnd?.Invoke(this);
        Destroy(gameObject);
    }
}
