using UnityEngine;

public class SwordManager : MonoBehaviour
{
    public GameObject hitbox; // Reference to the hitbox object
    public Animator animator;
    private Weapon currentWeapon;
    private Weapon defaultWeapon;

    // Offset for hitbox position
    public Vector3 offset = Vector3.zero;
    public Vector3 upOffset = new Vector3(0f, 0.1f, 0f);
    public Vector3 downOffset = new Vector3(0f, -2.5f, 0f);
    public Vector3 leftOffset = new Vector3(-1.3f, -1.6f, 0f);
    public Vector3 rightOffset = new Vector3(1.5f, -1.6f, 0f);
    public DialogueManager dialogueManager;
    private string newWeaponMessage;

    private void Start()
    {
        defaultWeapon = GetComponentInChildren<DefaultSword>();
        currentWeapon = defaultWeapon;
        // Find the DialogueManager in the scene if not already assigned
        if (dialogueManager == null)
        {
            dialogueManager = FindObjectOfType<DialogueManager>();
        }

        if (defaultWeapon == null)
        {
            Debug.LogError("Default weapon not found!");
        }
        else
        {
            Debug.Log("Default weapon found: " + defaultWeapon);
        }

        if (currentWeapon == null)
        {
            Debug.LogError("Current weapon not assigned!");
        }
        else
        {
            Debug.Log("Current weapon assigned: " + currentWeapon);
        }

        if (animator == null)
        {
            Debug.LogError("Animator not assigned!");
        }

        if (hitbox == null)
        {
            Debug.LogError("Hitbox not assigned!");
        }

        // Deactivate the hitbox initially
        hitbox.SetActive(false);
        offset = rightOffset;
    }

    public void UseCurrentWeapon(Vector3 lastInputDirection)
    {
        // Add this line to check if currentWeapon is null
        Debug.Log("Current Weapon: " + currentWeapon);

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (currentWeapon != null)
            {
                currentWeapon.UseWeapon(lastInputDirection);
            }
            else
            {
                Debug.LogError("Current weapon is null!");
            }

            if (animator != null)
            {
                animator.SetBool("isAttacking", true);
            }
            else
            {
                Debug.LogError("Animator is not assigned!");
            }

            if (hitbox != null)
            {
                hitbox.SetActive(true);
            }
            else
            {
                Debug.LogError("Hitbox is not assigned!");
            }
        }
        else if (Input.GetKeyUp(KeyCode.X))
        {
            if (animator != null)
            {
                animator.SetBool("isAttacking", false);
            }
            else
            {
                Debug.LogError("Animator is not assigned!");
            }

            if (hitbox != null)
            {
                hitbox.SetActive(false);
            }
            else
            {
                Debug.LogError("Hitbox is not assigned!");
            }
        }
    }

    public void UpdateHitboxPosition(Vector3 playerPosition)
    {
        hitbox.transform.position = playerPosition + offset;
    }

    public void SetOffset(Vector3 newOffset)
    {
        offset = newOffset;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Weapon newWeapon = other.GetComponent<Weapon>();
        if (newWeapon != null)
        {
            Debug.Log("New Sword Obtained");
            SwitchWeapon(newWeapon);

             // Deactivate the new weapon's game object
            newWeapon.gameObject.SetActive(false);
            
        }
    }

    private void SwitchWeapon(Weapon newWeapon)
    {
        currentWeapon = newWeapon;
         // Show the dialogue
        if (dialogueManager != null)
        {
            if(newWeapon.GetType().Name == "FlameSword"){
            newWeaponMessage = "Fire Bullets";
             dialogueManager.ShowDialogue(newWeaponMessage, Color.red );
            }
            if(newWeapon.GetType().Name == "ThunderSword"){
            newWeaponMessage = "Thunder Strikes";
             dialogueManager.ShowDialogue(newWeaponMessage, Color.yellow);
            }
            if(newWeapon.GetType().Name == "WindSword"){
            newWeaponMessage = "Wind Storms";
             dialogueManager.ShowDialogue(newWeaponMessage, Color.green);
            }
           
            
        }
        Debug.Log("Switched to: " + newWeapon.GetType().Name);
    }
}
