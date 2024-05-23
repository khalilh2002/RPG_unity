using System.Collections;
using UnityEngine;

public class GreenSlimeScript : MonoBehaviour
{
    public int damage = 1;
    public float knockbackDistance = 0f; // Adjust this value as needed
    public float knockbackDuration = 0f; // Adjust this value as needed
    public float flashDuration = 0.1f;
    public Color flashColor = Color.red;
    public LayerMask obstacleLayer; // Set this layer mask to the layer where your walls are placed
    public float jumpForce = 2f; // Force applied when the enemy jumps
    public float jumpCooldown = 1.5f; // Time between jumps

    private Main player;
    private bool hasHitPlayer = false;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private int jumpDirection = 1; // 1 for forward, -1 for backward
    private int jumpCount = 0; // Track the number of jumps

    private void Start()
    {
        // Find the player at the start
        player = FindObjectOfType<Main>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Start the jumping coroutine
        StartCoroutine(JumpPattern());
    }

    private void Update()
    {
        // No need for player detection and movement towards the player anymore
    }

    private IEnumerator JumpPattern()
    {
        while (true)
        {
            // Apply jump force in the current direction
            rb.velocity = new Vector2(jumpDirection * jumpForce, rb.velocity.y); // Apply horizontal force

            // Trigger jump animation
            animator.SetTrigger("Jump");

            // Flip sprite based on direction
            spriteRenderer.flipX = jumpDirection == -1;

            // Increment the jump count
            jumpCount++;

            // After two jumps in one direction, change direction
            if (jumpCount >= 2)
            {
                jumpDirection *= -1; // Reverse direction
                jumpCount = 0; // Reset jump count
            }

            // Wait for the next jump
            yield return new WaitForSeconds(jumpCooldown);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the trigger is with the hitbox
        if (other.CompareTag("Hitbox"))
        {
            // Get the Main script from the player object
            Main playerHit = other.GetComponentInParent<Main>();

            // Check if we successfully got the Main script
            if (playerHit != null)
            {
                // Destroy the enemy object
                playerHit.EnemyKilled();
                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("Main not found!");
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !hasHitPlayer)
        {
            Main playerCollided = collision.gameObject.GetComponent<Main>();
            if (playerCollided != null)
            {
                // Apply damage to the player
                playerCollided.TakeDamage(damage);

                // Calculate knockback direction
                Vector2 knockbackDirection = (playerCollided.transform.position - transform.position).normalized;

                // Move the player away from the enemy
                StartCoroutine(KnockbackPlayer(playerCollided, knockbackDirection));

                // Flash the player sprite
                StartCoroutine(FlashPlayerSprite(playerCollided));

                // Set the flag to prevent repeated knockback
                hasHitPlayer = true;
                hasHitPlayer = false;
            }
        }
    }

    // Coroutine to move the player away from the enemy
    IEnumerator KnockbackPlayer(Main player, Vector2 knockbackDirection)
    {
        // Calculate the position where the player should move to
        Vector2 targetPosition = (Vector2)player.transform.position + knockbackDirection * knockbackDistance;

        // Check for obstacles between the player and the target position
        RaycastHit2D hit = Physics2D.Raycast(player.transform.position, knockbackDirection, knockbackDistance, obstacleLayer);
        if (hit.collider != null)
        {
            // If there's an obstacle, adjust the target position to stop just before the obstacle
            targetPosition = hit.point - knockbackDirection * 0.1f; // Adjust 0.1f to the player's radius
        }

        // Move the player smoothly to the target position
        float elapsedTime = 0f;
        Vector2 initialPosition = player.transform.position;
        while (elapsedTime < knockbackDuration)
        {
            float t = elapsedTime / knockbackDuration;
            player.transform.position = Vector2.Lerp(initialPosition, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the player is exactly at the target position
        player.transform.position = targetPosition;
    }

    // Coroutine to flash the player sprite
    IEnumerator FlashPlayerSprite(Main player)
    {
        SpriteRenderer playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
        Color originalColor = playerSpriteRenderer.color;
        playerSpriteRenderer.color = flashColor;

        yield return new WaitForSeconds(flashDuration);

        playerSpriteRenderer.color = originalColor;
    }
}
