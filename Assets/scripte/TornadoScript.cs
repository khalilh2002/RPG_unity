using UnityEngine;

public class Tornado : MonoBehaviour
{

    public float lifetime = 10f; // Time before the tornado disappears
    private Animator animator;
    private Main player;



    void Start()
    {
        // Get the Main script from the player object
        // Find the Main script from the player object in the scene
        player = FindObjectOfType<Main>();
        if (player == null)
        {
            Debug.LogError("Main component not found in the scene. Ensure there is a Main component in the scene.");
        }
        animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.Play("TornadoAnim"); // Replace with your animation name
        }
        // Destroy the tornado after its lifetime
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Enemy"))
        {
            player.EnemyKilled();
            // Destroy the specific enemy instance that was hit
            Destroy(other.gameObject);
        }
    }
}
