using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/EnemyList")]
public class EnemyList : ScriptableObject
{
    public List<Enemy> _list;
    public float _minSpawndelay=0;
    public float _maxSpawnDelay=2f;
    public int _waveBonus = 100;
}
