using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveCameraPointer : MonoBehaviour
{
    private bool _isIn;
    private bool _canMoveCamera;
    public static Action<Vector3> _OnMouseCloseToBorder;

    private void Start()
    {
        TowerBuilderManager._onEnterExit += SetCanMoveCamera;
    }

    public void SetCanMoveCamera(bool canMove)
    {
        _canMoveCamera = canMove;
    }
    void Update()
    {
        if (!_canMoveCamera) { return; }

        float xRatio = (Input.mousePosition.x - Screen.width / 2f) / Screen.width;
        float yRatio = (Input.mousePosition.y - Screen.height / 2f) / Screen.height;


        if (xRatio < -0.45f || xRatio > 0.45f || yRatio < -0.45f || yRatio > 0.45f)
        {
            _OnMouseCloseToBorder?.Invoke((new Vector3(xRatio, 0, yRatio)).normalized);
        }
    }
}
