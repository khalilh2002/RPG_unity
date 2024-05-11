using System.Collections;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public int damage = 1;
    public float knockbackDistance = 1f; // Adjust this value as needed
    public float knockbackDuration = 0.1f; // Adjust this value as needed
    public float flashDuration = 0.1f;
    public Color flashColor = Color.red;
    public LayerMask obstacleLayer; // Set this layer mask to the layer where your walls are placed

    private Main player;
    private bool hasHitPlayer = false;

   private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the trigger is with the hitbox
        if (other.CompareTag("Hitbox"))
        {
            // Get the PlayerController script from the player object
            if (player == null)
            {
                player = other.GetComponentInParent<Main>();

                // Check if we successfully got the PlayerController script
                if (player != null)
                {
                    // Destroy the enemy object
                    player.EnemyKilled();
                    Destroy(gameObject);
                }
                else
                {
                    Debug.LogError("Main not found!");
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !hasHitPlayer)
        {
            player = collision.gameObject.GetComponent<Main>();
            if (player != null)
            {
                // Apply damage to the player
                player.TakeDamage(damage);

                // Calculate knockback direction
                Vector2 knockbackDirection = (player.transform.position - transform.position).normalized;

                // Move the player away from the enemy
                StartCoroutine(KnockbackPlayer(knockbackDirection));

                // Flash the player sprite
                StartCoroutine(FlashPlayerSprite());

                // Set the flag to prevent repeated knockback
                hasHitPlayer = true;
                hasHitPlayer = false;
            }
        }
    }

    // Coroutine to move the player away from the enemy
    IEnumerator KnockbackPlayer(Vector2 knockbackDirection)
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
    IEnumerator FlashPlayerSprite()
    {
        SpriteRenderer playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
        Color originalColor = playerSpriteRenderer.color;
        playerSpriteRenderer.color = flashColor;

        yield return new WaitForSeconds(flashDuration);

        playerSpriteRenderer.color = originalColor;
    }
}
