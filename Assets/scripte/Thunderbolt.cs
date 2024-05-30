using UnityEngine;

public class Thunderbolt : MonoBehaviour
{
    public float lifetime = 10f; // Time before the thunderbolt disappears
    public Animator animator;
    public int damage = 1; // Damage dealt by the thunderbolt

    private Main player;

    void Start()
    {
        // Get the Main script from the player object
        player = FindObjectOfType<Main>();
        if (player == null)
        {
            Debug.LogError("Main component not found in the scene. Ensure there is a Main component in the scene.");
        }

        animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.Play("ThunderAnim"); // Replace with your animation name
        }

        // Destroy the thunderbolt after its lifetime
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("GreenSlime") || other.CompareTag("RedSlime"))
        {
            player.EnemyKilled();
            // Destroy the specific enemy instance that was hit
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Boss"))
        {
            BossScript boss = other.GetComponent<BossScript>();
            if (boss != null)
            {
                boss.TakeDamage(damage);
            }
        }
    }
}
