using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private Vector2 _input;
    private CharacterController _characterController;
    private Camera _mainCamera;

    private Vector3 _direction;
    private float _gravity = -9.81f;
    private float _verticalVelocity;

    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float rotationSpeed = 500.0f;

    [SerializeField] private float gravityMultiplier = 3.0f;

    [SerializeField] private float jumpForce = 5.0f;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _mainCamera = Camera.main;
    }



    private void ApplyMovement()
    {
        _characterController.Move(_direction * speed * Time.deltaTime);
    }

    private void ApplyRotation()
    {
        if (_input.sqrMagnitude == 0) return;

        _direction = Quaternion.Euler(0.0f, _mainCamera.transform.eulerAngles.y, 0.0f) * new Vector3(_input.x, 0.0f, _input.y);
        var targetRotation = Quaternion.LookRotation(_direction, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        if (IsGrounded() && _verticalVelocity < 0.0f) { _verticalVelocity = -1.0f; }
        else { _verticalVelocity += _gravity * gravityMultiplier * Time.deltaTime; }

        _direction.y = _verticalVelocity;
    }

    private void Update()
    {
        ApplyRotation();
        ApplyGravity();
        ApplyMovement();
    }

    public void Move(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<Vector2>();
        _direction = new Vector3(_input.x, 0.0f, _input.y);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (!IsGrounded()) return;
        _verticalVelocity += jumpForce;
    }

    private bool IsGrounded() => _characterController.isGrounded;
}
