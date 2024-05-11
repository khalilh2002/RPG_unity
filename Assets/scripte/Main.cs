using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField] Transform boss;
    [SerializeField] Transform player;
    [SerializeField] RoomFirstMapGenerator obj;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] GameObject enemyPrefab;

    public TextMesh coinText;
    public TextMesh enemyText;
    public TextMesh healthBar;

    private int coins = 0;
    private int enemiesKilled = 0;
    private int maxHealth = 100;
    private int currentHealth;

    Vector3 maxRoom = default;
    double maxDistance = default;

    public GameObject originalCoin; // Store the original coin prefab
    public GameObject originalEnemy; // Store the original enemy prefab
     CoinCollector coinCollector; // Reference to the CoinCollector script

     // Dictionary to track if coins are collected in each room
    Dictionary<BoundsInt, bool> roomCoinsCollected = new Dictionary<BoundsInt, bool>();

    void Start()
    {
        obj.runRoomFirstMapGeneratorClass();

        // Store the original coin and enemy prefab
        originalCoin = coinPrefab;
        originalEnemy = enemyPrefab;

        // Calculate the maximum distance from the player room to any other room
        Dictionary<BoundsInt, double> distances = RoomFirstMapGenerator.djikstra_result;
        maxDistance = GetMaxDistance(distances);

        // Spawn coins and enemies in each room based on the distance from the player room
        foreach (BoundsInt roomBounds in RoomFirstMapGenerator.listRoomOrigin)
        {
            double distance = distances[roomBounds];
            int numberOfCoins = CalculateNumberOfCoins(distance, maxDistance);
            int numberOfEnemies = CalculateNumberOfEnemies(distance, maxDistance);

            // Spawn coins and enemies
            SpawnCoinsAndEnemiesRandomlyInRoom(roomBounds, numberOfCoins, numberOfEnemies);
            // Mark coins as not collected initially
            roomCoinsCollected[roomBounds] = false;
        }

        // Set initial positions
        var FirstRoom = RoomFirstMapGenerator.FirstRoom;
        player.transform.position = new Vector3(FirstRoom.center.x, FirstRoom.center.y, FirstRoom.center.z);

        // Dictionary<BoundsInt, double> djikstra_player = RoomFirstMapGenerator.djikstra_result;
        // Dictionary<BoundsInt, double> valueBoss = max_distance(djikstra_player, ref maxRoom, ref maxDistance);
        // boss.transform.position = maxRoom;
        Dictionary<BoundsInt, double> djikstra_player = obj.graph_main.Dijkstra(FirstRoom);

        //Dictionary<BoundsInt, double> valueBoss = max_distance(djikstra_player, ref maxRoom, ref maxDistance);

        BoundsInt maxR = max_distance_room(djikstra_player);
        boss.transform.position = maxR.center;

         // Get the CoinCollector script
        coinCollector = GameObject.FindObjectOfType<CoinCollector>();

        // Disable automatic collection at start
    if (coinCollector != null)
    {
        coinCollector.DeactivateAutomaticCollection(); // Add this line
    }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Y))
        {
            obj.runRoomFirstMapGeneratorClass();
            var FirstRoom = RoomFirstMapGenerator.FirstRoom;
            player.transform.position = new Vector3(FirstRoom.center.x, FirstRoom.center.y, FirstRoom.center.z);

            //Dictionary<BoundsInt, double> djikstra_player = RoomFirstMapGenerator.djikstra_result;
            //Dictionary<BoundsInt, double> valueBoss = max_distance(djikstra_player, ref maxRoom, ref maxDistance);
            //boss.transform.position = maxRoom;

            Dictionary<BoundsInt, double> djikstra_player = obj.graph_main.Dijkstra(FirstRoom);

            BoundsInt maxR = max_distance_room(djikstra_player);

            boss.transform.position = maxR.center;

            AdjustCoinsAndEnemiesBasedOnDistance(djikstra_player);

            if (coinCollector != null)
    {
        coinCollector.DeactivateAutomaticCollection(); // Add this line
    }

            // Activate or deactivate CoinCollector based on coins collected in the current room
        BoundsInt currentRoomBounds = GetCurrentRoomBounds();
        if (coinCollector != null && roomCoinsCollected.ContainsKey(currentRoomBounds))
        {
            bool coinsCollected = roomCoinsCollected[currentRoomBounds];
            if (coinsCollected)
            {
                coinCollector.DeactivateAutomaticCollection();
            }
            else
            {
                coinCollector.OnCoinsGenerated(originalCoin);
            }
        }
        }
    }

    void AdjustCoinsAndEnemiesBasedOnDistance(Dictionary<BoundsInt, double> distances)
    {
        // Clear all existing coins and enemies except the original ones
        RemoveAllCoinsAndEnemiesExceptOriginal();

        // Get the distance from the player room to the boss room
        maxDistance = GetMaxDistance(distances);

        // Respawn coins and enemies from the original ones
        foreach (BoundsInt roomBounds in RoomFirstMapGenerator.listRoomOrigin)
        {
            double distance = distances[roomBounds];
            int numberOfCoins = CalculateNumberOfCoins(distance, maxDistance);
            int numberOfEnemies = CalculateNumberOfEnemies(distance, maxDistance);

            // Spawn coins and enemies from the original ones
            SpawnCoinsAndEnemiesRandomlyInRoom(roomBounds, numberOfCoins, numberOfEnemies);
        }
    }

    void RemoveAllCoinsAndEnemiesExceptOriginal()
    {
        // Find all existing coins and enemies
        GameObject[] existingCoins = GameObject.FindGameObjectsWithTag("Coin");
        GameObject[] existingEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Destroy all existing coins and enemies except the original ones
        foreach (GameObject coin in existingCoins)
        {
            if (coin != originalCoin)
                Destroy(coin);
        }

        foreach (GameObject enemy in existingEnemies)
        {
            if (enemy != originalEnemy)
                Destroy(enemy);
        }
    }

    void SpawnCoinsAndEnemiesRandomlyInRoom(BoundsInt roomBounds, int numberOfCoins, int numberOfEnemies)
    {
         // Skip if it's the boss room
        if (roomBounds.center == maxRoom) return;
        int minX = roomBounds.xMin + (RoomFirstMapGenerator.offsetvar + 1);
        int maxX = roomBounds.xMax - (RoomFirstMapGenerator.offsetvar + 1);
        int minY = roomBounds.yMin + (RoomFirstMapGenerator.offsetvar + 1);
        int maxY = roomBounds.yMax - (RoomFirstMapGenerator.offsetvar + 1);

        for (int i = 0; i < numberOfCoins; i++)
        {
            Vector2Int randomPosition = new Vector2Int(
                Random.Range(minX, maxX),
                Random.Range(minY, maxY)
            );

            Instantiate(originalCoin, new Vector3(randomPosition.x, randomPosition.y, 0), Quaternion.identity).tag = "Coin";
        }

        for (int i = 0; i < numberOfEnemies; i++)
        {
            Vector2Int randomPosition = new Vector2Int(
                Random.Range(minX, maxX),
                Random.Range(minY, maxY)
            );

            Instantiate(originalEnemy, new Vector3(randomPosition.x, randomPosition.y, 0), Quaternion.identity).tag = "Enemy";
        }
    }

    int CalculateNumberOfCoins(double distance, double maxDistance)
    {
        float t = (float)(distance / maxDistance);
        int minCoins = 1;
        int maxCoins = 10;
        return Mathf.RoundToInt(Mathf.Lerp(minCoins, maxCoins, t));
    }

    int CalculateNumberOfEnemies(double distance, double maxDistance)
    {
        float t = (float)(distance / maxDistance);
        int minEnemies = 0;
        int maxEnemies = 5;
        return Mathf.RoundToInt(Mathf.Lerp(minEnemies, maxEnemies, t));
    }

    double GetMaxDistance(Dictionary<BoundsInt, double> distances)
    {
        double maxDistance = double.NegativeInfinity;
        BoundsInt maxBoundsInt = default;

        foreach (var item in distances)
        {
            if (item.Value > maxDistance)
            {
                maxDistance = item.Value;
                maxBoundsInt = item.Key;
            }
        }

        maxRoom = maxBoundsInt.center;
        return maxDistance;
    }

    //Dictionary<BoundsInt, double> max_distance(Dictionary<BoundsInt, double> distances, ref Vector3 x, ref double y)
    //{
    //    double maxValue = double.NegativeInfinity;
    //    BoundsInt maxBoundsInt = default;

    //    foreach (var item in distances)
    //    {
    //        if (item.Value > maxValue)
    //        {
    //            maxValue = item.Value;
    //            maxBoundsInt = item.Key;
    //        }
    //    }

    //    x = maxBoundsInt.center;
    //    y = maxValue;

    //    Dictionary<BoundsInt, double> maxDict = new Dictionary<BoundsInt, double>();
    //    maxDict[maxBoundsInt] = maxValue;

    //    return maxDict;
    //}
    BoundsInt max_distance_room(Dictionary<BoundsInt, double> distances)
    {
        double maxValue = double.NegativeInfinity;
        BoundsInt maxBoundsInt = default ;

        foreach (var item in distances)
        {
            if (item.Value > maxValue)
            {
                maxValue = item.Value;
                maxBoundsInt = item.Key;
            }
        }        

        return maxBoundsInt;
    }

    public BoundsInt GetCurrentRoomBounds()
    {
        Vector3 playerPosition = player.transform.position;
        foreach (BoundsInt roomBounds in RoomFirstMapGenerator.listRoomOrigin)
        {
            if (roomBounds.Contains(new Vector3Int((int)playerPosition.x, (int)playerPosition.y, (int)playerPosition.z)))
            {
                return roomBounds;
            }
        }
        return default;
    }

    // Function to set the collected status of coins in a room
    public void SetCoinsCollectedStatus(BoundsInt roomBounds, bool collected)
    {
        if (roomCoinsCollected.ContainsKey(roomBounds))
        {
            roomCoinsCollected[roomBounds] = collected;
        }
    }
}
