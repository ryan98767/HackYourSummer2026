using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;

    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jump = 5f;

    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheck;

    private float horizontal;
    private float vertical;

    //to check which movement system is being used and easily able to switch
    private bool sideScroll = true;

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontal * moveSpeed, rb.linearVelocity.y);
    }

    #region PLAYER_CONTROLS
    public void Move(InputAction.CallbackContext context)
    {
        //side scroll movements
        //horizontal is used in both
        horizontal = context.ReadValue<Vector2>().x;
        //top down movements
        //vertical is only used in top down
        if (!sideScroll)
        {
            vertical = context.ReadValue<Vector2>().y;
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        //jumping is only used in side scroll
        if (context.performed && IsGrounded() && sideScroll)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jump);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapBox(groundCheck.position,new Vector2(1f, 0.1f), 0, groundLayer);
    }
    #endregion
}
