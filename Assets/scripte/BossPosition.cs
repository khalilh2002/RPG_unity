using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] Transform Boss;
    [SerializeField] Transform player;

    [SerializeField] RoomFirstMapGenerator obj;

    // Start is called before the first frame update

    List<BoundsInt> rooms;
    void Start()
    {
       Vector3 playerPosition = player.transform.position;

       rooms = RoomFirstMapGenerator.listRoomOrigin;

       Vector3 maxRoomCenter = Vector3Int.RoundToInt(rooms[0].center);
       float maxNumber=0;

       foreach (var room in rooms)
       {
            if (  maxNumber <  Vector3.Distance(playerPosition , Vector3Int.RoundToInt(room.center) ) )
            {
                maxNumber =   Vector3.Distance(playerPosition , Vector3Int.RoundToInt(room.center) );
                maxRoomCenter = Vector3Int.RoundToInt(room.center);
            }
       }

        Boss = GetComponent<Transform>();
        Boss.transform.position = maxRoomCenter;


        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Y))
        {
            Vector3 playerPosition = player.transform.position;

            rooms = RoomFirstMapGenerator.listRoomOrigin;

            Vector3 maxRoomCenter = rooms[0].center;
            float maxNumber=0;

            foreach (var room in rooms)
            {
                    if (  maxNumber <  Vector3.Distance(playerPosition , room.center ) )
                    {
                        maxNumber =   Vector3.Distance(playerPosition , room.center );
                        maxRoomCenter = room.center;
                    }
                   Debug.Log(maxNumber);
            }

            Boss.transform.position = maxRoomCenter;
        }
        
    }
}
