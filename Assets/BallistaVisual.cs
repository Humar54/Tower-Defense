using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallistaVisual : MonoBehaviour
{
    public Transform _ballista;
    public Transform _spawnPoint;
    public Transform _spotLight;
    public Vector3 _spotLightPos;

    private void Start()
    {
        if(_spotLight!=null)
        {
            _spotLight.transform.position= transform.position+ _spotLightPos;
        }
    }

}
