using UnityEngine;

public class ThunderSword : Weapon
{
    public GameObject thunderboltPrefab; // Assign this in the inspector
    private Transform playerTransform; // Reference to the player

 void Start()
    {
        // Find the player object by tag and get its transform
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player not found. Make sure the player GameObject is tagged as 'Player'.");
        }

        // Example of using playerTransform
        // You can replace this with whatever logic you need
        if (playerTransform != null)
        {
            Debug.Log("Player's position: " + playerTransform.position);
        }
    }
    public override void UseWeapon(Vector3 lastInputDirection)
    {
        Debug.Log("Using Thunder Sword!");

        // Calculate the positions for the thunderbolts based on the player's direction
        Vector3 position1 = Vector3.zero;
        Vector3 position2 = Vector3.zero;
        Vector3 position3 = Vector3.zero;

        if (lastInputDirection == Vector3.up)
        {
            position1 = playerTransform.position + new Vector3(-3, 4, 0);
            position2 = playerTransform.position + new Vector3(3, 4, 0);
            position3 = playerTransform.position + new Vector3(0, 5, 0);
        }
        else if (lastInputDirection == Vector3.down)
        {
            position1 = playerTransform.position + new Vector3(-3, -6, 0);
            position2 = playerTransform.position + new Vector3(3, -6, 0);
            position3 = playerTransform.position + new Vector3(0, -7, 0);
        }
        else if (lastInputDirection == Vector3.left)
        {
            position1 = playerTransform.position + new Vector3(-4, 3, 0);
            position2 = playerTransform.position + new Vector3(-4, -4, 0);
            position3 = playerTransform.position + new Vector3(-5, 0, 0);
        }
        else if (lastInputDirection == Vector3.right)
        {
            position1 = playerTransform.position + new Vector3(4, 3, 0);
            position2 = playerTransform.position + new Vector3(4, -4, 0);
            position3 = playerTransform.position + new Vector3(5, 0, 0);
        }

        // Instantiate the thunderbolts
        Instantiate(thunderboltPrefab, position1, Quaternion.identity);
        Instantiate(thunderboltPrefab, position2, Quaternion.identity);
        Instantiate(thunderboltPrefab, position3, Quaternion.identity);
    }
}
