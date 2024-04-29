using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Main : MonoBehaviour
{

    [SerializeField] Transform boss;

    [SerializeField] Transform player;

    [SerializeField] RoomFirstMapGenerator obj;


    void Start()
    {
       // player = GetComponent<Transform>();
        //boss = GetComponent<Transform>();

        obj.runRoomFirstMapGeneratorClass();



        
        var FirstRoom = RoomFirstMapGenerator.FirstRoom;
        player.transform.position = new Vector3(FirstRoom.center.x, FirstRoom.center.y, FirstRoom.center.z);
        
        float max = float.MinValue;
        BoundsInt bossRoom = RoomFirstMapGenerator.listRoomOrigin[0];
        foreach (var room in RoomFirstMapGenerator.listRoomOrigin)
        {
            var distance = Vector3.Distance(FirstRoom.center, room.center);
            if (distance > max)
            {
                max = distance;
                bossRoom = room;
            }
        }

        FirstRoom = bossRoom;
        boss.transform.position = new Vector3(FirstRoom.center.x, FirstRoom.center.y, FirstRoom.center.z);
        
        //Dictionary<BoundsInt, double> djikstra_player = RoomFirstMapGenerator.djikstra_result;
        //Dictionary<BoundsInt, double> valueBoss = max_distance(djikstra_player);
        //boss.transform.position = valueBoss.Keys.First().center;

    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Y))
        {
            
            Debug.Log("you enter Y");
            obj.runRoomFirstMapGeneratorClass();
            var FirstRoom = RoomFirstMapGenerator.FirstRoom;
            player.transform.position = new Vector3(FirstRoom.center.x, FirstRoom.center.y, FirstRoom.center.z);


            float max = float.MinValue;
            BoundsInt bossRoom = RoomFirstMapGenerator.listRoomOrigin[0];
            foreach (var room in RoomFirstMapGenerator.listRoomOrigin)
            {
                var distance = Vector3.Distance(FirstRoom.center, room.center);
                if (distance > max)
                {
                    max = distance;
                    bossRoom = room;
                }
            }

            FirstRoom = bossRoom;
            boss.transform.position = new Vector3(FirstRoom.center.x, FirstRoom.center.y, FirstRoom.center.z);
            




            /*
            obj.runRoomFirstMapGeneratorClass();

            // Get the player's current position
            var playerPosition = player.position;

            // Get the Dijkstra result from the RoomFirstMapGenerator
            Dictionary<BoundsInt, double> djikstra_player = RoomFirstMapGenerator.djikstra_result;

            // Find the position with the maximum distance from the player
            var farthestPosition = GetFarthestPosition(playerPosition, djikstra_player);

            // Set the boss's position to the farthest position
            boss.position = farthestPosition;
            */

        }
    }


    Dictionary<BoundsInt, double> max_distance( Dictionary<BoundsInt, double> distances)
    {
        double maxValue = double.NegativeInfinity;
        BoundsInt maxBoundsint = default;
        foreach (var item in distances)
        {
            if (item.Value > maxValue)
            {
                maxValue = item.Value;
                maxBoundsint = item.Key;
            }

        }

        Dictionary<BoundsInt, double> maxDict = new Dictionary<BoundsInt, double>();
        maxDict[maxBoundsint] = maxValue;

        return maxDict;
    }

    // Method to find the position with the maximum distance from the player using Dijkstra's algorithm
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

}
