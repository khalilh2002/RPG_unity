using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField] Transform boss;
    [SerializeField] Transform player;
    [SerializeField] RoomFirstMapGenerator obj;
    [SerializeField] GameObject coinPrefab;

    Vector3 maxRoom = default;
    double maxDistance = default;
    GameObject firstCoin;

    void Start()
    {
        obj.runRoomFirstMapGeneratorClass();

        // Calculate the maximum distance from the player room to the boss room
        Dictionary<BoundsInt, double> distances = RoomFirstMapGenerator.djikstra_result;
        double maxDistance = GetMaxDistance(distances);

        // Spawn coins in each room based on the distance from the player room
        foreach (BoundsInt roomBounds in RoomFirstMapGenerator.listRoomOrigin)
        {
            double distance = distances[roomBounds];
            int numberOfCoins = CalculateNumberOfCoins(distance, maxDistance);
            SpawnCoinsRandomlyInRoom(roomBounds, numberOfCoins);
        }

        // Set initial positions
        var FirstRoom = RoomFirstMapGenerator.FirstRoom;
        player.transform.position = new Vector3(FirstRoom.center.x, FirstRoom.center.y, FirstRoom.center.z);

        Dictionary<BoundsInt, double> djikstra_player = RoomFirstMapGenerator.djikstra_result;
        Dictionary<BoundsInt, double> valueBoss = max_distance(djikstra_player, ref maxRoom, ref maxDistance);
        boss.transform.position = maxRoom;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Y))
        {
            obj.runRoomFirstMapGeneratorClass();
            var FirstRoom = RoomFirstMapGenerator.FirstRoom;
            player.transform.position = new Vector3(FirstRoom.center.x, FirstRoom.center.y, FirstRoom.center.z);

            Dictionary<BoundsInt, double> djikstra_player = RoomFirstMapGenerator.djikstra_result;
            Dictionary<BoundsInt, double> valueBoss = max_distance(djikstra_player, ref maxRoom, ref maxDistance);
            boss.transform.position = maxRoom;

            AdjustCoinsBasedOnDistance(djikstra_player);
        }
    }

    void SpawnCoinsRandomlyInRoom(BoundsInt roomBounds, int numberOfCoins)
    {
        int minX = roomBounds.xMin + (RoomFirstMapGenerator.offsetvar+1);
        int maxX = roomBounds.xMax - (RoomFirstMapGenerator.offsetvar+1);
        int minY = roomBounds.yMin + (RoomFirstMapGenerator.offsetvar+1);
        int maxY = roomBounds.yMax - (RoomFirstMapGenerator.offsetvar+1);

        for (int i = 0; i < numberOfCoins; i++)
        {
            Vector2Int randomPosition = new Vector2Int(
                Random.Range(minX, maxX),
                Random.Range(minY, maxY)
            );

            GameObject coin = Instantiate(coinPrefab, new Vector3(randomPosition.x, randomPosition.y, 0), Quaternion.identity);
        }
    }

    int CalculateNumberOfCoins(double distance, double maxDistance)
    {
        // Adjust the number of coins based on the distance
        // The farther the room, the higher the number of coins
        float t = (float)(distance / maxDistance); // Convert to a value between 0 and 1
        int minCoins = 1; // Minimum number of coins for the player room
        int maxCoins = 10; // Maximum number of coins for the boss room
        return Mathf.RoundToInt(Mathf.Lerp(minCoins, maxCoins, t));
    }

    double GetMaxDistance(Dictionary<BoundsInt, double> distances)
    {
        double maxDistance = double.NegativeInfinity;
        foreach (var distance in distances.Values)
        {
            if (distance > maxDistance)
            {
                maxDistance = distance;
            }
        }
        return maxDistance;
    }

    void AdjustCoinsBasedOnDistance(Dictionary<BoundsInt, double> distances)
    {
        // Get the distance from the player room to the boss room
        double maxDistance = GetMaxDistance(distances);

        foreach (BoundsInt roomBounds in RoomFirstMapGenerator.listRoomOrigin)
        {
            double distance = distances[roomBounds];
            int numberOfCoins = CalculateNumberOfCoins(distance, maxDistance);

            // Remove existing coins in the room
            RemoveExistingCoinsInRoom(roomBounds);

            // Spawn new coins in the room
            SpawnCoinsRandomlyInRoom(roomBounds, numberOfCoins);
        }
    }

    void RemoveExistingCoinsInRoom(BoundsInt roomBounds)
    {
        GameObject[] existingCoins = GameObject.FindGameObjectsWithTag("Coin");

        foreach (GameObject coin in existingCoins)
        {
            Vector3 coinPosition = coin.transform.position;

            if (roomBounds.Contains(new Vector3Int((int)coinPosition.x, (int)coinPosition.y, 0)))
            {
                Destroy(coin);
            }
        }
    }

    Vector3 GetFarthestPosition(Vector3 playerPosition, Dictionary<BoundsInt, double> distances)
    {
        double maxDistance = double.NegativeInfinity;
        Vector3 farthestPosition = Vector3.zero;

        foreach (var item in distances)
        {
            // Calculate the distance from the player to the current position
            var distanceToPlayer = item.Value;

            // If the distance is greater than the current maximum distance, update the max distance and farthest position
            if (distanceToPlayer > maxDistance)
            {
                maxDistance = distanceToPlayer;
                farthestPosition = item.Key.center;
            }
        }

        return farthestPosition;
    }

    Dictionary<BoundsInt, double> max_distance(Dictionary<BoundsInt, double> distances, ref Vector3 x, ref double y)
    {
        double maxValue = double.NegativeInfinity;
        BoundsInt maxBoundsint = default;

        x = maxBoundsint.center;
        y = maxValue;

        foreach (var item in distances)
        {
            if (item.Value > maxValue)
            {
                maxValue = item.Value;
                maxBoundsint = item.Key;

                x = maxBoundsint.center;
                y = maxValue;
            }
        }

        Dictionary<BoundsInt, double> maxDict = new Dictionary<BoundsInt, double>();
        maxDict[maxBoundsint] = maxValue;

        return maxDict;
    }
}
