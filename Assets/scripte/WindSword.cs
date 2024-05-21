using UnityEngine;

public class WindSword : Weapon
{
    public GameObject tornadoPrefab; // Assign this in the Inspector
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
        Debug.Log("Using Wind Sword!");

        // Define positions relative to the player
        Vector3 upPosition = playerTransform.position + new Vector3(0,4,0);
        Vector3 downPosition = playerTransform.position + new Vector3(0,-6,0);
        Vector3 leftPosition = playerTransform.position + new Vector3(-5,-1,0);
        Vector3 rightPosition = playerTransform.position + new Vector3(5,-1,0);

        // Instantiate the tornadoes at these positions
        Instantiate(tornadoPrefab, upPosition, Quaternion.identity);
        Instantiate(tornadoPrefab, downPosition, Quaternion.identity);
        Instantiate(tornadoPrefab, leftPosition, Quaternion.identity);
        Instantiate(tornadoPrefab, rightPosition, Quaternion.identity);
    }
}
