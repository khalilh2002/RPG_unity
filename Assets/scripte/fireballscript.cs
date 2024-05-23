using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 10f;
    public GameObject explosionPrefab;

    private Vector3 direction;

private Main player;
    // Set the direction of the fireball

    void Start(){
      // Find the Main script from the player object in the scene
        player = FindObjectOfType<Main>();
        if (player == null)
        {
            Debug.LogError("Main component not found in the scene. Ensure there is a Main component in the scene.");
        }
    }
    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
        Debug.Log("Fireball direction set to: " + direction);
    }

    void Update()
    {
        // Move the fireball
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Enemy"))
        {
            
            // Destroy the specific enemy instance that was hit
            Destroy(other.gameObject);

            player.EnemyKilled();
            // Create an explosion effect
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            // Destroy the fireball
            Destroy(gameObject);
        }
         if (other.CompareTag("GreenSlime"))
        {
            
            // Destroy the specific enemy instance that was hit
            Destroy(other.gameObject);

            player.EnemyKilled();
            // Create an explosion effect
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            // Destroy the fireball
            Destroy(gameObject);
        }
         if (other.CompareTag("RedSlime"))
        {
            
            // Destroy the specific enemy instance that was hit
            Destroy(other.gameObject);

            player.EnemyKilled();
            // Create an explosion effect
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            // Destroy the fireball
            Destroy(gameObject);
        }
        
    }
}
