using UnityEngine;

public class Tornado : MonoBehaviour
{
    public float lifetime = 10f; // Time before the tornado disappears
    private Animator animator;
    private Main player;
    public int damage = 1; // Damage dealt by the tornado

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
            animator.Play("TornadoAnim"); // Replace with your animation name
        }

        // Destroy the tornado after its lifetime
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
