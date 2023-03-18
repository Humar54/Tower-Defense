using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BowTower : Tower
{
    Transform _currentTarget;
    Transform _previousTarget;

    private List<Vector3> _posList =new List<Vector3>();

    private float ratio;

    protected override void Update()
    {
        if(GameManager._instance.GetTowerLvl(TowerType.ArrowTower)>=2)
        {

            if (_hasBeenBuilt)
            {
                _attackTimer += Time.deltaTime;
                
                _enemyList = GameManager._instance.GetActiveEnemies().OrderBy(x => (x.transform.position - transform.position).magnitude).ToList();

                if (_attackTimer >= _attackDelay)
                {
                    
                    if (EnemyIsInRange())
                    {
                        
                        
                        int limit = 3;
                        if(_enemyList.Count<3)
                        {
                            limit = _enemyList.Count;
                        }

                        for (int i = 0; i < limit; i++)
                        {
                            Vector3 deplacement = _enemyList[i].transform.position - transform.position;
                            float distance = deplacement.magnitude;
                            Vector3 direction = (deplacement).normalized;
                            Vector3 right = GetComponentInChildren<BallistaVisual>()._ballista.forward;
                            float dot = Vector3.Dot(direction, right);
                            if (dot > 0.25f && distance <=_attackRange)
                            {
                                Attack(_enemyList[i], GetComponentInChildren<BallistaVisual>()._spawnPoint.position);
                            }
                        }
 
                    }
                }
            }


            if (_hasBeenBuilt && _enemyList.Count!=0)
            {
                Transform target = _enemyList[0].transform;

                _posList.Add(target.position);
                if (_posList.Count >= 20)
                {
                    _posList.RemoveAt(0);
                }
                Vector3 Average = Vector3.zero;
                for (int i = 0; i < _posList.Count; i++)
                {
                    Average += _posList[i];
                }
                Average /= _posList.Count;
                OrientBalistaTowardTarget(Average);
            }

        }
        else
        {
            base.Update();
            if (_hasBeenBuilt)
            {
                if (GetTarget() == null) { return; }
                Transform target = GetTarget().transform;

                _posList.Add(target.position);
                if (_posList.Count >= 20)
                {
                    _posList.RemoveAt(0);
                }
                Vector3 Average = Vector3.zero;
                for (int i = 0; i < _posList.Count; i++)
                {
                    Average += _posList[i];
                }
                Average /= _posList.Count;


                OrientBalistaTowardTarget(Average);
            }
        }
    }

    private void OrientBalistaTowardTarget(Vector3 targetPos)
    {
        Transform ballista = GetComponentInChildren<BallistaVisual>()._ballista;
        float ballistaY = ballista.transform.position.y;
        ballista.LookAt(new Vector3(targetPos.x, ballistaY, targetPos.z));
    }
}
