using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBall : MonoBehaviour
{
    public int damage = 1;
    public Transform target;
    public float speed;
    public float explodeAfterSeconds = 5f;
    public GameObject explosionEffect;

    public float flashDuration = 0.1f;
    public Color flashColor = Color.red;

    void Start()
    {
        Invoke("Explode", explodeAfterSeconds);
    }

    void Update()
    {
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Main playerCollided = other.GetComponent<Main>();
        if (playerCollided != null)
        {
            // Apply damage to the player
            playerCollided.TakeDamage(damage);

            // Flash the player sprite
            StartCoroutine(FlashPlayerSprite(playerCollided));

            Explode();
        }
    }

    void Explode()
    {
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
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
