using UnityEngine;

public class Coin : MonoBehaviour
{
    private Main player;

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that collided with the coin is the player
        if (other.CompareTag("Player"))
        {
            // Get the PlayerController script from the player object
            if (player == null)
            {
                // Get the PlayerController script from the player object
                player = other.GetComponent<Main>();

                // Check if we successfully got the PlayerController script
                if (player != null)
                {
                    // Collect the coin and destroy it
                    player.CollectCoin();
                    Destroy(gameObject);
                }
                else
                {
                    Debug.LogError("PlayerController not found!");
                }
            }
        }
    }
}
