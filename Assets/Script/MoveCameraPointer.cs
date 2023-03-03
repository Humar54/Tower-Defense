using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveCameraPointer : MonoBehaviour
{
    private bool _isIn;
    public static Action<Vector3> _OnMouseCloseToBorder;



    void Update()
    {
        float xRatio = (Input.mousePosition.x - Screen.width / 2f) / Screen.width;
        float yRatio = (Input.mousePosition.y - Screen.height / 2f) / Screen.height;

        //Debug.Log($"xRatio {xRatio} yRatio{yRatio}");

        if (xRatio < -0.45f || xRatio > 0.45f || yRatio < -0.45f || yRatio > 0.45f)
        {
            _OnMouseCloseToBorder?.Invoke((new Vector3(xRatio, 0, yRatio)).normalized);
        }
    }
}
