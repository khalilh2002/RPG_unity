using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{

    [SerializeField] Transform boss;

    [SerializeField] Transform player;

    [SerializeField] RoomFirstMapGenerator obj;


    void Start()
    {
        player = GetComponent<Transform>();
        boss = GetComponent<Transform>();

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


        }
    }
}
