using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private float _cameraSpeed;
    [SerializeField] private float _zoomSpeed = 10f;
    [SerializeField] private float _accelerationDelay = 2f;

    private float _currentCameraSpeed;
    private float _timer;


    private void Start()
    {
        MoveCameraPointer._OnMouseCloseToBorder += SetCameraSpeed;

    }
    void Update()
    {

        Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")) + Input.GetAxisRaw("Mouse ScrollWheel") * _zoomSpeed * transform.forward;

        SetCameraSpeed(direction);

    }

    private void SetCameraSpeed(Vector3 direction)
    {
        if (direction.magnitude >= 1f)
        {
            _timer += Time.unscaledDeltaTime;
        }
        else
        {
            _timer = _accelerationDelay;
        }
        _currentCameraSpeed = _timer / _accelerationDelay * _cameraSpeed;
        transform.position += direction * Time.unscaledDeltaTime * _currentCameraSpeed;
    }

    private void OnDestroy()
    {
        MoveCameraPointer._OnMouseCloseToBorder -= SetCameraSpeed;
    }
}
