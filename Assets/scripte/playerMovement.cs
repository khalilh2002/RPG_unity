using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    [SerializeField] public float speed = 25f;

    public static float pspeed;

    public bool IsRunning = false;
    public bool lastInputWasUp = false; // Default to up
    public bool lastInputWasDown = false;
    bool isAttacking = false;

    private SwordManager swordManager;
    private Vector3 lastNonZeroInputDirection = Vector3.right; // Default to down

    void Start()
    {
        pspeed = speed;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        swordManager = GetComponent<SwordManager>();

        if (swordManager == null)
        {
            Debug.LogError("SwordManager not found on the player GameObject.");
        }

        
    }

    void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        // Movement
        if (!isAttacking)  // Prevent movement during attack
        {
            transform.Translate(new Vector3(horizontalInput, verticalInput, 0f) * speed * Time.deltaTime);
        }

        // Check if the player is running
        IsRunning = (horizontalInput != 0 || verticalInput != 0);
        animator.SetBool("IsRunning", IsRunning);

        Vector3 lastInputDirection = Vector3.zero;

        // Flip sprite if moving horizontally
        if (horizontalInput < 0)
        {
            spriteRenderer.flipX = true;
            lastInputWasDown = false;
            lastInputWasUp = false;
            swordManager.SetOffset(swordManager.leftOffset);
            lastInputDirection = Vector3.left;
        }
        else if (horizontalInput > 0)
        {
            spriteRenderer.flipX = false;
            lastInputWasDown = false;
            lastInputWasUp = false;
            swordManager.SetOffset(swordManager.rightOffset);
            lastInputDirection = Vector3.right;
        }

        // Set Idle animations
        animator.SetBool("IsIdle", !IsRunning || (horizontalInput == 0 && verticalInput == 0 && !lastInputWasUp));
        animator.SetBool("IsUp", lastInputWasUp && !IsRunning && horizontalInput == 0);
        animator.SetBool("IsDown", lastInputWasDown && !IsRunning && horizontalInput == 0);

        // Set RunningUp and RunningDown animations
        animator.SetBool("IsRunningUp", verticalInput > 0 && IsRunning);
        animator.SetBool("IsRunningDown", verticalInput < 0 && IsRunning);

        // Update lastInputDirection
        if (verticalInput > 0)
        {
            swordManager.SetOffset(swordManager.upOffset);
            lastInputWasUp = true;
            lastInputWasDown = false;
            lastInputDirection = Vector3.up;
        }
        else if (verticalInput < 0)
        {
            lastInputWasDown = true;
            lastInputWasUp = false;
            swordManager.SetOffset(swordManager.downOffset);
            lastInputDirection = Vector3.down;
        }

// If there's a non-zero input, update the last non-zero input direction
        if (lastInputDirection != Vector3.zero)
        {
            lastNonZeroInputDirection = lastInputDirection.normalized;
        }
        // Update hitbox position
        if (swordManager != null)
        {
            swordManager.UpdateHitboxPosition(transform.position);
        }

        // Use current weapon
        if (swordManager != null)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                isAttacking = true;
                swordManager.UseCurrentWeapon(lastNonZeroInputDirection);
            }
            else if (Input.GetKeyUp(KeyCode.X))
            {
                isAttacking = false;
            }
        }

        // Reset attack animation state when not attacking
        if (!isAttacking && animator.GetBool("isAttacking"))
        {
            animator.SetBool("isAttacking", false);
            swordManager.hitbox.SetActive(false);
        }
    }
}
