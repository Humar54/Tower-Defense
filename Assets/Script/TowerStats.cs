using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "My Assets/TowerStats")]
public class TowerStats : ScriptableObject
{
    public GameObject _visual;
    public Projectile _projectile;
    public int _price;
    public int _damage;
    public float _attackDelay;
    public float _attackRange;
    public int _towerUpgradePrice;

}
