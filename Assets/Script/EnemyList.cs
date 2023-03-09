using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/EnemyList")]
public class EnemyList : ScriptableObject
{
    public List<Enemy> _list;
}
