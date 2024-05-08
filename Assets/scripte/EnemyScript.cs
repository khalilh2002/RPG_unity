using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the trigger is with the hitbox
        if (other.CompareTag("Hitbox"))
        {
            // Destroy the enemy object
            Destroy(gameObject);
        }
    }
}