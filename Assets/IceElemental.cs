using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceElemental : Enemy
{
    [SerializeField] private float _deathRadius;
    [SerializeField] private float _armorBonusDelay = 10f;
    [SerializeField] private GameObject _vfx;
    public override void Death()
    {
        if (_isDead == false)
        {
            Collider[] colliderArray = Physics.OverlapSphere(transform.position, _deathRadius);
            foreach (Collider collider in colliderArray)
            {
                Enemy enemy = collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.ArmorBuff(_armorBonusDelay);
                }
            }
            GameObject vfx = Instantiate(_vfx, transform.position, Quaternion.identity);
            vfx.transform.localScale = Vector3.one * _deathRadius / 5f;
            Destroy(vfx, 2f);
        }

        base.Death();
    }

    public override string GetDescription()
    {
        return _description.Replace("{0}", _deathRadius.ToString());
    }
}
