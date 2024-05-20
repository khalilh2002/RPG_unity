using UnityEngine;

public class AutoMoveAwayFromEnemy : MonoBehaviour
{
    public float minDistanceFromEnemy = 3f; // Minimum distance from the enemy
    public KeyCode toggleKey = KeyCode.B; // Key to toggle auto move away

    private bool moveAwayEnabled = false; // Toggle for moving away from enemies
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private playerMovement playerMovementScript;





    

  


    void Start()
    {
        moveAwayEnabled = false; // Set to false by default
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerMovementScript = GetComponent<playerMovement>();
    }

    void Update()
    {
        // Toggle move away behavior
        if (Input.GetKeyDown(toggleKey))
        {
            moveAwayEnabled = !moveAwayEnabled;
        }

        if (moveAwayEnabled)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (GameObject enemy in enemies)
            {
                Vector3 directionToEnemy = transform.position - enemy.transform.position;
                if (directionToEnemy.magnitude < minDistanceFromEnemy)
                {
                    Vector3 moveDirection = directionToEnemy.normalized;
                    transform.Translate(moveDirection * playerMovementScript.speed * Time.deltaTime);
                }
            }
        }

    }
}
