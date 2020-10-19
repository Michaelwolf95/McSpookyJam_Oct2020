using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TempMoveScript : MonoBehaviour
{
    [SerializeField] Rigidbody _playerRb;
    [SerializeField] Transform _mainCamTransform;
    [SerializeField] float _speed = 5;
    Vector3 _forwardDirection, _sideDirection, _moveSpeed;
    float _sideInput, _forwardInput;

    void Awake()
    {
        _playerRb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        UpdateDirection();
        Debug.Log(_moveSpeed);
        if (Input.GetKey(KeyCode.Space))
            Cursor.lockState = CursorLockMode.None;
        UpdateMove();
    }

    void FixedUpdate()
    {
        _playerRb.MovePosition(transform.position + (new Vector3(_moveSpeed.x, _playerRb.velocity.y, _moveSpeed.z) * _speed * Time.deltaTime));
    }

    void UpdateMove()
    {
        _sideInput = Input.GetAxis("Horizontal");
        _forwardInput = Input.GetAxis("Vertical");

        if(_sideInput != 0 || _forwardInput != 0)
        {
            var forwardMove = _forwardInput * _forwardDirection;
            var sideMove = _sideInput * _sideDirection;
            _moveSpeed = forwardMove + sideMove;

            //_playerRb.velocity = new Vector3(_moveSpeed.x, _playerRb.velocity.y, _moveSpeed.z) * _speed;
            //_playerRb.AddForce(new Vector3(_moveSpeed.x, _playerRb.velocity.y, _moveSpeed.z) * _speed);
        }
    }

    void UpdateDirection()
    {
        _forwardDirection = new Vector3(_mainCamTransform.forward.x, 0, _mainCamTransform.forward.z);
        _sideDirection = new Vector3(_mainCamTransform.right.x, 0, _mainCamTransform.right.z);
    }
}
