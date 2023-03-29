using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public Rigidbody2D rb;
    public float moveSpeed = 5f;
    public InputAction playerControls;

    Vector2 moveDirection = Vector2.zero;

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
    
    // Update is called once per frame
    void Update()
    {
        //float moveX = Input.GetAxis("Horizontal");
        //float moveY = Input.GetAxis("Vertical");

        moveDirection = playerControls.ReadValue<Vector2>();

    }

    void fixedUpdate()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }
}
