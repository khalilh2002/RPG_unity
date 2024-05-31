using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Main : MonoBehaviour
{
    [SerializeField] Transform boss;
    [SerializeField] Transform player;
    [SerializeField] RoomFirstMapGenerator obj;
    [SerializeField] GameObject coinPrefab;

    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject greenslimePrefab;
    [SerializeField] GameObject redslimePrefab;
    [SerializeField] BossScript bossobj;


    public TMP_Text coinText;
    public TMP_Text enemyText;
    public TMP_Text healthBar;

    private int coins = 0;
    private int enemiesKilled = 0;
    private int maxHealth = 5;
    public int currentHealth;
    public GameObject windSwordPrefab;
    public GameObject thunderSwordPrefab;
    public GameObject flameSwordPrefab;
    private List<GameObject> swords = new List<GameObject>();


    Vector3 maxRoom = default;
    double maxDistance = default;
    private Rigidbody2D rb;
    public GameObject originalCoin; // Store the original coin prefab
    public GameObject originalEnemy; // Store the original enemy prefab

    public GameObject wind;
    public GameObject fire;
    public GameObject thunder;


    public GameObject originalRedSlime;
    public GameObject originalGreenSlime;
    public AudioClip coinsound;

    public AudioClip hitsound;
    private AudioSource audioSource;
    CoinCollector coinCollector; // Reference to the CoinCollector script
    public SpriteRenderer playerSpriteRenderer; // Reference to the player's SpriteRenderer

    // Dictionary to track if coins are collected in each room
    Dictionary<BoundsInt, bool> roomCoinsCollected = new Dictionary<BoundsInt, bool>();

    void Start()
    {
        obj.runRoomFirstMapGeneratorClass();

        // Store the original coin and enemy prefab
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        originalCoin = coinPrefab;
        originalEnemy = enemyPrefab;
        wind = windSwordPrefab;
        fire = flameSwordPrefab;
        thunder = thunderSwordPrefab;
        originalRedSlime = GameObject.FindGameObjectWithTag("RedSlime");
        originalGreenSlime = GameObject.FindGameObjectWithTag("GreenSlime");
        currentHealth = maxHealth;
        UpdateUI();


        // Calculate the maximum distance from the player room to any other room
        Dictionary<BoundsInt, double> distances = RoomFirstMapGenerator.djikstra_result;
        maxDistance = GetMaxDistance(distances);

        // Spawn coins and enemies in each room based on the distance from the player room
        foreach (BoundsInt roomBounds in RoomFirstMapGenerator.listRoomOrigin)
        {
            double distance = distances[roomBounds];
            int numberOfCoins = CalculateNumberOfCoins(distance, maxDistance);
            int numberOfEnemies = CalculateNumberOfEnemies(distance, maxDistance);
            int numberOfReds = CalculateNumberOfReds(distance, maxDistance);
            int numberOfGreens = CalculateNumberOfGreens(distance, maxDistance);

            // Spawn coins and enemies
            SpawnCoinsAndEnemiesRandomlyInRoom(roomBounds, numberOfCoins, numberOfEnemies, numberOfGreens, numberOfReds);
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
        PlaceSwordsRandomly();
        // Disable automatic collection at start
        if (coinCollector != null)
        {
            coinCollector.DeactivateAutomaticCollection(); // Add this line
        }
    }

    void UpdateUI()
    {
        coinText.text = coins.ToString();
        enemyText.text = enemiesKilled.ToString();
        healthBar.text = currentHealth.ToString();
    }

    public void ApplyKnockback(Vector2 direction, float force)
    {
        rb.velocity = Vector2.zero; // Reset the player's velocity before applying the knockback
        rb.AddForce(direction * force, ForceMode2D.Impulse);
    }
    public void CollectCoin()
    {
        if (coinsound != null)
        {
            audioSource.PlayOneShot(coinsound);
        }
        coins++;
        UpdateUI();
    }

    public void EnemyKilled()
    {
        if (hitsound != null)
        {
            audioSource.PlayOneShot(hitsound);
        }
        enemiesKilled++;
        UpdateUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        UpdateUI();
    }



    void Update()
    {
        if (Input.GetKey(KeyCode.Y) || (currentHealth <= 0))
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
            coins = 0;
            enemiesKilled = 0;
            bossobj.maxHealth = 10;
            currentHealth = maxHealth;
            playerSpriteRenderer.color = Color.white;


            UpdateUI();

            AdjustCoinsAndEnemiesBasedOnDistance(djikstra_player);
            PlaceSwordsRandomly();

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
            int numberOfGreens = CalculateNumberOfGreens(distance, maxDistance);
            int numberOfReds = CalculateNumberOfReds(distance, maxDistance);

            // Spawn coins and enemies from the original ones
            SpawnCoinsAndEnemiesRandomlyInRoom(roomBounds, numberOfCoins, numberOfEnemies, numberOfGreens, numberOfReds);
        }
    }

    void RemoveAllCoinsAndEnemiesExceptOriginal()
    {
        // Find all existing coins and enemies
        GameObject[] existingCoins = GameObject.FindGameObjectsWithTag("Coin");
        GameObject[] existingEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] existingReds = GameObject.FindGameObjectsWithTag("RedSlime");
        GameObject[] existingGreens = GameObject.FindGameObjectsWithTag("GreenSlime");


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
        foreach (GameObject red in existingReds)
        {
            if (red != originalRedSlime)
                Destroy(red);
        }
        foreach (GameObject green in existingGreens)
        {
            if (green != originalGreenSlime)
                Destroy(green);
        }
    }

    void SpawnCoinsAndEnemiesRandomlyInRoom(BoundsInt roomBounds, int numberOfCoins, int numberOfEnemies, int numberOfGreens, int numberOfReds)
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

        for (int i = 0; i < numberOfReds; i++)
        {
            Vector2Int randomPosition = new Vector2Int(
                Random.Range(minX, maxX),
                Random.Range(minY, maxY)
            );

            Instantiate(originalRedSlime, new Vector3(randomPosition.x, randomPosition.y, 0), Quaternion.identity).tag = "RedSlime";
        }
        for (int i = 0; i < numberOfGreens; i++)
        {
            Vector2Int randomPosition = new Vector2Int(
                Random.Range(minX, maxX),
                Random.Range(minY, maxY)
            );

            Instantiate(originalGreenSlime, new Vector3(randomPosition.x, randomPosition.y, 0), Quaternion.identity).tag = "GreenSlime";
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
        int maxEnemies = 3;
        return Mathf.RoundToInt(Mathf.Lerp(minEnemies, maxEnemies, t));
    }

    int CalculateNumberOfGreens(double distance, double maxDistance)
    {
        float t = (float)(distance / maxDistance);
        int minGreens = 0;
        int maxGreens = 2;
        return Mathf.RoundToInt(Mathf.Lerp(minGreens, maxGreens, t));
    }

    int CalculateNumberOfReds(double distance, double maxDistance)
    {
        float t = (float)(distance / maxDistance);
        int minReds = 0;
        int maxReds = 1;
        return Mathf.RoundToInt(Mathf.Lerp(minReds, maxReds, t));
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
        BoundsInt maxBoundsInt = default;

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
    void PlaceSwordsRandomly()
    {
        // Clear the swords list to avoid adding duplicate swords
        swords.Clear();

        // Add the swords to the list
        swords.Add(windSwordPrefab);
        swords.Add(thunderSwordPrefab);
        swords.Add(flameSwordPrefab);

        // Remove existing swords from the scene
        RemoveExistingSwords();

        List<BoundsInt> availableRooms = new List<BoundsInt>(RoomFirstMapGenerator.listRoomOrigin);

        // Loop through each sword
        foreach (var sword in swords)
        {
            if (availableRooms.Count == 0) break;

            // Get a random room from the available rooms
            int randomIndex = Random.Range(0, availableRooms.Count);
            BoundsInt randomRoom = availableRooms[randomIndex];

            // Check if the random room is the boss room, if so, find another room
            while (randomRoom.center == maxRoom)
            {
                randomIndex = Random.Range(0, availableRooms.Count);
                randomRoom = availableRooms[randomIndex];
            }

            // Remove the selected room from the available list
            availableRooms.RemoveAt(randomIndex);

            // Get a random position within the room
            Vector3 randomPosition = new Vector3(
                Random.Range(randomRoom.xMin + (RoomFirstMapGenerator.offsetvar + 1), randomRoom.xMax - (RoomFirstMapGenerator.offsetvar + 1)),
                Random.Range(randomRoom.yMin + (RoomFirstMapGenerator.offsetvar + 1), randomRoom.yMax - (RoomFirstMapGenerator.offsetvar + 1)),
                0);

            // Instantiate the sword at the random position
            Instantiate(sword, randomPosition, Quaternion.identity);
        }
    }
    void RemoveExistingSwords()
    {
        GameObject[] existingWeapons = GameObject.FindGameObjectsWithTag("Weapon");

        foreach (GameObject sword in existingWeapons)
        {
            if (sword != wind && sword != fire && sword != thunder)
            {
                Destroy(sword);
            }
        }


    }
}
