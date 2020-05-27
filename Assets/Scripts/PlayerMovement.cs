using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Enums;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement current;
    private Vector2 _moveaxis;
    public Vector2 MoveAxis => _moveaxis;
    private Controls _Controls;
    private Rigidbody _player_rb;
    public Rigidbody Player_Rb => _player_rb;
    private PlayerState currentState;
    [Range(6f, 10f)]
    public float moveSpeed = 7f;
    public event Action Moveu;


    private void OnEnable()
    {
        _Controls = new Controls();
        _Controls.Player.DirectionalMovement.performed += HandleMove;
        _Controls.Player.DirectionalMovement.Enable();
    }

    private void HandleMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        try
        {
            _moveaxis = context.ReadValue<Vector2>();

            if (MoveAxis == Vector2.zero)
            {
                currentState = PlayerState.STILL;
            }
            else
            {
                currentState = PlayerState.MOVING;
            }
        }
        catch
        {
            throw new System.NotImplementedException();
        }
    }

    private void OnDisable()
    {
        _Controls.Player.DirectionalMovement.performed -= HandleMove;
        _Controls.Player.DirectionalMovement.Disable();
    }

    private void Awake()
    {
        if (current == null)
            current = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        _player_rb = GetComponent<Rigidbody>();
        Player_Rb.freezeRotation = true;
    }

    private void Update()
    {
        switch (currentState)
        {
            case PlayerState.STILL:
                break;
            case PlayerState.MOVING:
                MovePlayer();
                break;
            default:
                Debug.LogError("NO STATE");
                break;
        }
    }

    private void MovePlayer()
    {
        // Debug.Log(MoveAxis.x * moveSpeed + " " + 0 + " " + MoveAxis.y * moveSpeed);
        _player_rb.AddForce(MoveAxis.x * moveSpeed, 0, MoveAxis.y * moveSpeed);

        if (Moveu != null)
        {
            Moveu();
        }
    }
}
