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

    //to check which movement system is being used and easily able to switch
    private bool sideScroll = true;

    //death check
    private bool hasDied = false;

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

        //Debug.Log("Player speed: " + moveSpeed.ToString());
    }

    public bool SideScroll
    {
        get { return sideScroll; }
        set { sideScroll = value; }
    }

    public bool HasDied
    {
        get { return hasDied; }
        set { hasDied = value; }
    }

    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
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
        if (context.performed && IsGrounded() && sideScroll)
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
        return Physics2D.OverlapBox(groundCheck.position, new Vector2(1f, 0.1f), 0, groundLayer);
    }
    #endregion

    //temporary function to switch between control schemes
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Switch"))
        {
            sideScroll = !sideScroll;
            rb.gravityScale = sideScroll ? 1f : 0f;
            rb.linearVelocity = Vector2.zero;
            Debug.Log("Switched control scheme");
            if (playerInput.currentActionMap.name == "SideScroll")
            {
                playerInput.SwitchCurrentActionMap("TopDown");
            }
            else if (playerInput.currentActionMap.name == "TopDown")
            {
                playerInput.SwitchCurrentActionMap("SideScroll");
            }
        }
    }

    public void Die()
    {
        if (HasDied) { return; }
        //if the player dies and then re-triggers death, such as animation freezing on the screen
        //causing them to recollide with an entity, won't re call the function

        hasDied = true;
        Debug.Log("Player has died");
        //placeholder code for things like death animations
        //playerAnimation.SetTrigger("Die", true);
    }
}
