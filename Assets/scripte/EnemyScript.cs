using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public int damage = 1;

    private Main player; // Change "Main" to "PlayerController"

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

        // Check if the collider is the player
        if (other.CompareTag("Player"))
        {
            // Get the PlayerController script from the player object
            if (player == null)
            {
                player = other.GetComponent<Main>();

                // Check if we successfully got the PlayerController script
                if (player != null)
                {
                    
                    player.TakeDamage(damage);
                    
                }
                else
                {
                    Debug.LogError("PlayerController not found!");
                }
            }
        }
    }
}
