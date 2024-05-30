using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public int damage = 1;
    public float lifespan = 5f; // Time in seconds before the projectile gets destroyed

    void Start()
    {
        // Schedule the projectile to be destroyed after its lifespan
        Destroy(gameObject, lifespan);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Main player = collision.gameObject.GetComponent<Main>();
            if (player != null)
            {
                player.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
