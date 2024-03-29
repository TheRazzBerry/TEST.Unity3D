using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private MouseSensitivity mouseSensitivity;
    [SerializeField] private CameraAngle cameraAngle;

    private Vector2 _input;

    private float _cameraDistance;
    private CameraRotation _cameraRotation;


    private void Awake()
    {
        _cameraDistance = Vector3.Distance(transform.position, target.position);
    }

    public void Look(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        _cameraRotation.yaw += _input.x * mouseSensitivity.horizontal * Time.deltaTime;
        _cameraRotation.pitch += _input.y * mouseSensitivity.vertical * Time.deltaTime;
        _cameraRotation.pitch = Mathf.Clamp(_cameraRotation.pitch, cameraAngle.min, cameraAngle.max);

    }

    private void LateUpdate()
    {
        transform.eulerAngles = new Vector3(_cameraRotation.pitch, _cameraRotation.yaw, 0.0f);
        transform.position = target.position - transform.forward * _cameraDistance;
    }
}

[Serializable]
public struct MouseSensitivity
{
    public float horizontal;
    public float vertical;
}

public struct CameraRotation
{
    public float pitch;
    public float yaw;
}

[Serializable]
public struct CameraAngle
{
    public float min;
    public float max;
}