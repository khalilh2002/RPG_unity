using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    Animator animator;
    SpriteRenderer spriteRenderer;
    [SerializeField] float speed = 25f;

    bool IsRunning = false;
    bool lastInputWasUp = false; // Default to up
    bool lastInputWasDown = false;
    bool isAttacking = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        // Movement
        transform.Translate(new Vector3(horizontalInput, verticalInput, 0f) * speed * Time.deltaTime);

        // Check if the player is running
        IsRunning = (horizontalInput != 0 || verticalInput != 0);
        animator.SetBool("IsRunning", IsRunning);

        // Flip sprite if moving horizontally
        if (horizontalInput < 0)
        {
            spriteRenderer.flipX = true;
            lastInputWasDown = false;
            lastInputWasUp = false;
        }
        else if (horizontalInput > 0)
        {
            spriteRenderer.flipX = false;
            lastInputWasDown = false;
            lastInputWasUp = false;
        }

        // Set Idle animations
        animator.SetBool("IsIdle", !IsRunning || (horizontalInput == 0 && verticalInput == 0 && !lastInputWasUp));
        animator.SetBool("IsUp", lastInputWasUp && !IsRunning && horizontalInput == 0);
        animator.SetBool("IsDown", lastInputWasDown && !IsRunning && horizontalInput == 0);

        // Set RunningUp and RunningDown animations
        animator.SetBool("IsRunningUp", verticalInput > 0 && IsRunning);
        animator.SetBool("IsRunningDown", verticalInput < 0 && IsRunning);

        // Attack
        if (Input.GetKeyDown(KeyCode.X))
        {
            isAttacking = true;
            animator.SetBool("isAttacking", true);
        }
        else if (Input.GetKeyUp(KeyCode.X))
        {
            isAttacking = false;
            animator.SetBool("isAttacking", false);
        }

        // Update lastInputWasUp
        if (verticalInput > 0)
        {
            lastInputWasUp = true;
            lastInputWasDown = false;
        }
        else if (verticalInput < 0)
        {
            lastInputWasDown = true;
            lastInputWasUp = false;
        }
    }
}
