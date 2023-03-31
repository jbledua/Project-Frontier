using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestPlayerScript : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed = 5f;
    public PlayerInputActions playerControls;

    Vector2 moveDirection = Vector2.zero;
    private InputAction move;
    private InputAction fire;

    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
    }

    // Update is called once per frame
    private void Update()
    {
        //float moveX = Input.GetAxis("Hoizontal");
        //float moveY = Input.GetAxis("Vertical");
        //moveDirection = new Vector2(moveX, moveY);

        moveDirection = move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }
}
