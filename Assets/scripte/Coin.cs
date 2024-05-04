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
}
