using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public playerMovement playerMovementScript; // Reference to the playerMovement script
    public float playerSpeed = 5f; // Player movement speed

    private List<Transform> coins = new List<Transform>(); // List to store all coins except the original
    private int currentCoinIndex = 0; // Index of the current coin to collect

    // Reference to the original coin
    private GameObject originalCoin;

    // Boolean variable to control automatic collection
    private bool automaticCollectionActive = false;

    // Reference to the Main script
    Main mainScript;

    void Start()
    {
        // Get the reference to the Main script
        mainScript = FindObjectOfType<Main>();
        // Get the reference to the playerMovement script
        playerMovementScript = player.GetComponent<playerMovement>();
    }

    void Update()
    {
        // Toggle automatic collection on/off when "C" is pressed
        if (Input.GetKeyDown(KeyCode.C))
        {
            automaticCollectionActive = !automaticCollectionActive;
            if (automaticCollectionActive)
            {
                StartAutomaticCollection();
            }
            else
            {
                DeactivateAutomaticCollection();
            }
        }

        if (automaticCollectionActive && currentCoinIndex < coins.Count)
        {
            // Set the player's speed to the player movement script's speed
            playerSpeed = playerMovementScript.speed;
            MoveToNextCoin();

            // Update player animations
            UpdatePlayerAnimations();
        }
    }

    void StartAutomaticCollection()
    {
        FindAllCoins(originalCoin);
        currentCoinIndex = 0;
    }

    public void OnCoinsGenerated(GameObject originalCoin)
    {
        // Store the reference to the original coin
        this.originalCoin = originalCoin;

        // Find all coins in the scene except the original coin and add them to the list
        FindAllCoins(originalCoin);

        // Start moving towards the first coin
        currentCoinIndex = 0;
        automaticCollectionActive = true;
    }

    void FindAllCoins(GameObject originalCoin)
    {
        // Clear the previous list of coins
        coins.Clear();

        // Find all coins in the scene except the original coin and add them to the list
        GameObject[] coinObjects = GameObject.FindGameObjectsWithTag("Coin");
        foreach (GameObject coinObject in coinObjects)
        {
            // Check if the coin is not the original one
            if (coinObject != originalCoin)
            {
                coins.Add(coinObject.transform);
            }
        }

        Debug.Log("Detected " + coins.Count + " coins.");
    }

    void MoveToNextCoin()
{
    // Ensure that the current coin index is within the bounds of the list
    if (currentCoinIndex >= coins.Count)
    {
        automaticCollectionActive = false;
        return;
    }

    // Find the nearest coin among the remaining coins
    Transform nearestCoin = FindNearestCoin();

    // Ensure that there is a nearest coin and it hasn't been destroyed
    if (nearestCoin == null)
    {
        Debug.LogWarning("No nearest coin found. Stopping automatic collection.");
        automaticCollectionActive = false;
        return;
    }

    // Calculate direction towards the nearest coin
    Vector3 direction = (nearestCoin.position - player.position).normalized;

    // Round the direction to the nearest main direction
    direction = RoundDirection(direction);

    // Move the player towards the nearest coin
    player.Translate(direction * playerSpeed * Time.deltaTime);

    Debug.Log("Moving towards nearest coin...");

    // Check if player reached the nearest coin
    if (Vector3.Distance(player.position, nearestCoin.position) < 0.5f)
    {
        // Remove the collected coin from the list
        coins.Remove(nearestCoin);

        Debug.Log("Coin collected. Remaining coins: " + coins.Count);

        // Check if all coins in the room are collected
        if (coins.Count == 0)
        {
            // Get the current room bounds
            BoundsInt roomBounds = mainScript.GetCurrentRoomBounds();
            // Inform Main script that all coins in this room are collected
            mainScript.SetCoinsCollectedStatus(roomBounds, true);
        }
    }
}

Vector3 RoundDirection(Vector3 direction)
{
    // Round direction to the nearest main direction
    if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
    {
        // Horizontal movement dominant
        return new Vector3(Mathf.Sign(direction.x), 0f, 0f);
    }
    else
    {
        // Vertical movement dominant
        return new Vector3(0f, Mathf.Sign(direction.y), 0f);
    }
}
    Transform FindNearestCoin()
    {
        Transform nearestCoin = null;
        float shortestDistance = float.MaxValue;

        // Iterate through all remaining coins to find the nearest one
        foreach (Transform coin in coins.ToArray()) // Convert to array to avoid modifying the list while iterating
        {
            // Ensure the coin hasn't been destroyed
            if (coin == null)
                continue;

            float distance = Vector3.Distance(player.position, coin.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestCoin = coin;
            }
        }

        return nearestCoin;
    }

    // Function to deactivate automatic coin collection
    public void DeactivateAutomaticCollection()
    {
        automaticCollectionActive = false;
    }
// Function to remove a coin from the list when it's destroyed
    public void RemoveCoin(Transform coin)
    {
        if (coins.Contains(coin))
        {
            coins.Remove(coin);
            Debug.Log("Coin removed. Remaining coins: " + coins.Count);
        }
    }
 void UpdatePlayerAnimations()
{
    // Get the position of the player and the coin
    Vector3 playerPos = player.position;
    Vector3 coinPos = coins[currentCoinIndex].position;

    // Determine animation parameters based on movement direction
    bool isRunning = Mathf.Abs(playerPos.x - coinPos.x) > 0;

    // Set animator parameters
    playerMovementScript.animator.SetBool("IsRunning", isRunning);

    // Set sprite flip and offset based on horizontal position
    if (playerPos.x < coinPos.x)
    {
        // Player should move right
        playerMovementScript.spriteRenderer.flipX = false;
        playerMovementScript.lastInputWasDown = false;
        playerMovementScript.lastInputWasUp = false;
        playerMovementScript.offset = playerMovementScript.rightOffset;
    }
    else if (playerPos.x > coinPos.x)
    {
        // Player should move left
        playerMovementScript.spriteRenderer.flipX = true;
        playerMovementScript.lastInputWasDown = false;
        playerMovementScript.lastInputWasUp = false;
        playerMovementScript.offset = playerMovementScript.leftOffset;
    }
}



}
     






