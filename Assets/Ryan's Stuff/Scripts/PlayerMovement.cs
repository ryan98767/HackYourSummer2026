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
    private Vector2 moveInput;

    [SerializeField] PlayerInput playerInput;

    /*
     * NOTE FROM ADON: Changed the field to be serialized so it can be switched at 
     * default
     */
    // to check which movement system is being used and easily able to switch
    [SerializeField] bool sideScroll = true;

    private void FixedUpdate()
    {
        
        if (sideScroll)
        {
            rb.linearVelocity = new Vector2(horizontal * moveSpeed, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = moveInput.normalized * moveSpeed;
        }
    }

    #region PLAYER_CONTROLS
    public void Move(InputAction.CallbackContext context)
    {
        //side scroll movements
        //horizontal is used in both
        moveInput = context.ReadValue<Vector2>();
        horizontal = moveInput.x;
        //top down movements
        //vertical is only used in top down
        
        vertical = moveInput.y;
        
    }

    public void Jump(InputAction.CallbackContext context)
    {
        //jumping is only used in side scroll
        if (sideScroll && context.performed && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jump);
        }
        else if (!sideScroll)
        {
            Debug.Log("You don't need to jump here!");
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapBox(groundCheck.position,new Vector2(1f, 0.1f), 0, groundLayer);
    }
    #endregion
}
