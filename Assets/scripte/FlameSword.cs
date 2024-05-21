using UnityEngine;

public class FlameSword : Weapon
{
    public GameObject fireballPrefab; // Assign this in the inspector
    private Transform fireballSpawnPoint; // Assign the spawn point for the fireball

    void Start()
    {
        // Find the player object by tag and get its transform
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            fireballSpawnPoint = player.transform;
        }
        else
        {
            Debug.LogError("Player not found. Make sure the player GameObject is tagged as 'Player'.");
        }
    }

    public override void UseWeapon(Vector3 lastInputDirection)
    {
        Debug.Log("Using Flame Sword! Direction: " + lastInputDirection);

        // Calculate the rotation based on the direction
        float angle = Mathf.Atan2(lastInputDirection.y, lastInputDirection.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle+90));

        // Create and launch the fireball
        GameObject fireball = Instantiate(fireballPrefab, fireballSpawnPoint.position, rotation);

        // Set the direction of the fireball
        fireball.GetComponent<Fireball>().SetDirection(lastInputDirection);

        // Ensure the fireball is correctly aligned with its direction
        fireball.transform.rotation = rotation;
    }
}
