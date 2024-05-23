using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedSlimeScript : MonoBehaviour
{
    public int damage = 1;
    public float knockbackDistance = 0f;
    public float knockbackDuration = 0f;
    public float flashDuration = 0.1f;
    public Color flashColor = Color.red;

    public LayerMask obstacleLayer;
    public GameObject slimeballPrefab;
    public float slimeballSpeed = 5f;
    public float detectionRange = 10f;
    private Main player;
    private bool hasHitPlayer = false;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private bool isPlayerInRange = false;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Main>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        InvokeRepeating("CheckPlayerDistance", 0f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerInRange)
        {
            LaunchFireball();
            isPlayerInRange = false; // Prevent continuous fireball launching
        }
    }

    void CheckPlayerDistance()
    {
        if (player != null && Vector3.Distance(transform.position, player.transform.position) <= detectionRange)
        {
            isPlayerInRange = true;
        }
    }

    void LaunchFireball()
    {
        GameObject slimeball = Instantiate(slimeballPrefab, transform.position, Quaternion.identity);
        SlimeBall slimeballScript = slimeball.GetComponent<SlimeBall>();
        slimeballScript.target = player.transform;
        slimeballScript.speed = slimeballSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hitbox"))
        {
            Main playerHit = other.GetComponentInParent<Main>();
            if (playerHit != null)
            {
                playerHit.EnemyKilled();
                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("Main not found!");
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !hasHitPlayer)
        {
            Main playerCollided = collision.gameObject.GetComponent<Main>();
            if (playerCollided != null)
            {
                playerCollided.TakeDamage(damage);
                Vector2 knockbackDirection = (playerCollided.transform.position - transform.position).normalized;
                StartCoroutine(KnockbackPlayer(playerCollided, knockbackDirection));
                StartCoroutine(FlashPlayerSprite(playerCollided));
                hasHitPlayer = true;
                hasHitPlayer = false;
            }
        }
    }

    IEnumerator KnockbackPlayer(Main player, Vector2 knockbackDirection)
    {
        Vector2 targetPosition = (Vector2)player.transform.position + knockbackDirection * knockbackDistance;
        RaycastHit2D hit = Physics2D.Raycast(player.transform.position, knockbackDirection, knockbackDistance, obstacleLayer);
        if (hit.collider != null)
        {
            targetPosition = hit.point - knockbackDirection * 0.1f;
        }

        float elapsedTime = 0f;
        Vector2 initialPosition = player.transform.position;
        while (elapsedTime < knockbackDuration)
        {
            float t = elapsedTime / knockbackDuration;
            player.transform.position = Vector2.Lerp(initialPosition, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        player.transform.position = targetPosition;
    }

    IEnumerator FlashPlayerSprite(Main player)
    {
        SpriteRenderer playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
        Color originalColor = playerSpriteRenderer.color;
        playerSpriteRenderer.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        playerSpriteRenderer.color = originalColor;
    }
}
