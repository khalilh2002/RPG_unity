using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public float invincibilityDuration = 1f; // Duration of invincibility after getting hit
    public Color hitColor = Color.yellow; // Color when hit
    public AudioClip hitSound; // Sound to play when hit

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Transform player;
    private bool isInvincible = false;
    private AudioSource audioSource;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
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
                if (!isInvincible)
                {
                    TakeDamage(1);
                }
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

        if (currentHealth <= 5)
        {
            directions = new Vector2[]
            {
                Vector2.up,
                Vector2.down,
                Vector2.left,
                Vector2.right,
                new Vector2(1, 1).normalized, // Diagonal directions
                new Vector2(-1, 1).normalized,
                new Vector2(1, -1).normalized,
                new Vector2(-1, -1).normalized
            };
        }

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

        if (hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }

        if (currentHealth <= maxHealth / 2)
        {
            spriteRenderer.color = enragedColor;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvincibilityCoroutine());
        }
    }

    IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        spriteRenderer.color = hitColor;
        yield return new WaitForSeconds(invincibilityDuration);
        spriteRenderer.color = currentHealth <= maxHealth / 2 ? enragedColor : Color.white;
        isInvincible = false;
    }

    void Die()
    {
        animator.SetTrigger("Die");
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        float dieAnimationLength = 1f; // Get the length of the current animation
        StartCoroutine(LoadLevelCompleteSceneAfterDelay(dieAnimationLength));
    }

    IEnumerator LoadLevelCompleteSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("LevelComplete"); // Replace with your level complete scene name
    }
}
