using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class test : MonoBehaviour
{
    [SerializeField] RoomFirstMapGenerator roomFirstMapGenerator;
    [SerializeField] float speed = 25f;

    private PathFinding pFinding;

    [SerializeField]
    Transform player;



    private int currentPathIndex;
    private List<Vector3> pathVectorList;

    static private List<Vector3> gridPos;


    private void Start()
    {
        pFinding = new PathFinding(roomFirstMapGenerator.getMapWidth, roomFirstMapGenerator.getMapHeight);
        pFinding.createWalkabelGrid(roomFirstMapGenerator.getFloor);
        pFinding.createGrid(roomFirstMapGenerator.getFloor);


    }
    private void Update()
    {
       


        if (Input.GetMouseButton(0))
        {
            StopAllCoroutines();
            // Get the mouse position in world space
            Vector3 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPosition.z = player.position.z; // Ensure the z-coordinate remains unchanged

            // Get grid coordinates for the mouse position

            if (pFinding == null)
            {
                Debug.LogError("pFinding is null");
                Time.timeScale = 0f;

            }
            pFinding.GetGrid().GetXY(targetPosition, out int targetX, out int targetY);
            pFinding.GetGrid().GetXY(player.position, out int x, out int y);

            List<Node> path = pFinding.FindPath(x, y, targetX, targetY);
            if (path == null)
            {
                Debug.LogError("path node is null");
                Time.timeScale = 0f;
            }
            for (int i = 0; i < path.Count - 1; i++)
            {
                Debug.DrawLine(nodeToV3(path[i]), nodeToV3(path[i + 1]), Color.red, 10f);
                Debug.DrawRay(nodeToV3(path[i]), Vector3.up * 0.2f, Color.green, 10f);

            }
            gridPos = NodetoVector3(path);
            StartCoroutine(FollowPath());
            gridPos = null;




        }

        if (Input.GetKey(KeyCode.G))
        {
            //pFinding = new PathFinding(roomFirstMapGenerator.getMapWidth, roomFirstMapGenerator.getMapHeight);

            if (roomFirstMapGenerator.getFloor == null)
            {
                Debug.Log(" floor is null 789 ");
            }
            else if (pFinding == null)
            {
                Debug.Log(" pFindfin is null 789 ");

            }
            else
            {
                pFinding.createWalkabelGrid(roomFirstMapGenerator.getFloor);
                pFinding.createGrid(roomFirstMapGenerator.getFloor);

            }

        }





    }

   

    IEnumerator FollowPath()
    {
            foreach (Vector3 pos in gridPos)
            {
                while (Vector3.Distance(transform.position, pos) > 0.5f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, (Vector3)pos, speed * Time.deltaTime);
                    yield return null;
                }
                // Ensure exact positioning on the grid
                transform.position = (Vector3)pos;
                yield return null; // Optional: wait for a frame before moving to the next position
            }
        

    }

    List<Vector3> NodetoVector3(List<Node> l)
    {
        List<Vector3> newList = new List<Vector3>();
        foreach (Node node in l)
        {
            newList.Add(nodeToV3(node));
        }
        return newList;
    }



    Vector3 nodeToV3(Node node)
    {
        return new Vector3(node.X, node.Y) * 2f + Vector3.one * 1f;
    }



    /////////////////////////////
    // Get Mouse Position in World with Z = 0f
    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }
    public static Vector3 GetMouseWorldPositionWithZ()
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
    }
    public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
    }
    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }
    /////////////////////////////////
}
