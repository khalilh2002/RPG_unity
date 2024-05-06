using UnityEngine;

public class Coin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Get the SpriteRenderer component and set the sprite
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = spriteRenderer.sprite; // Just to access the sprite renderer, you can remove this line

        // Play the coin animation
        Animator coinAnimation = GetComponent<Animator>();
        coinAnimation.Play("Coin");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
       // Check if the object that collided with the coin is the player
        if (other.CompareTag("Player"))
        {
            // Destroy the coin's game object
            Destroy(gameObject);

            // Remove the coin from the list in CoinCollector script
            CoinCollector coinCollector = FindObjectOfType<CoinCollector>();
            if (coinCollector != null)
            {
                coinCollector.RemoveCoin(transform);
            }
        }
    }
}
