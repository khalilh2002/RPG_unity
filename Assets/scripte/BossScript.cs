using System.Collections;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    public int maxHealth = 10;
    private int currentHealth;
    public float projectileSpeed = 5f;
    public float enhancedProjectileSpeed = 10f;
    public GameObject projectilePrefab;
    public Color enragedColor = Color.red;
    public float projectileCooldown = 2f;
    private float lastProjectileTime;
    public float attackDistance = 10f; // Minimum distance for the boss to start attacking

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Transform player;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform; // Ensure your player GameObject has the tag "Player"
    }

    void Update()
    {
        if (player != null && Vector2.Distance(transform.position, player.position) <= attackDistance)
        {
            if (Time.time >= lastProjectileTime + projectileCooldown)
            {
                LaunchProjectiles();
                lastProjectileTime = Time.time;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hitbox"))
        {
            Main playerHit = other.GetComponentInParent<Main>();
            if (playerHit != null)
            {
                
               TakeDamage(1);
            }
            else
            {
                Debug.LogError("Main not found!");
            }
        }
    }

    void LaunchProjectiles()
    {
        float speed = currentHealth <= maxHealth / 2 ? enhancedProjectileSpeed : projectileSpeed;

        Vector2[] directions = new Vector2[]
        {
            Vector2.up,
            Vector2.down,
            Vector2.left,
            Vector2.right
        };

        foreach (Vector2 direction in directions)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            rb.velocity = direction * speed;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        
        if (currentHealth <= maxHealth / 2)
        {
            spriteRenderer.color = enragedColor;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        animator.SetTrigger("Die");
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        Destroy(gameObject, 1f);
    }
}
