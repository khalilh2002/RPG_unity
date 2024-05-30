using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class test : MonoBehaviour
{
    private bool debugVar = false;



    [SerializeField] RoomFirstMapGenerator roomFirstMapGenerator;
    [SerializeField] float speed = 25f;

    private PathFinding pFinding;

    [SerializeField]
    Transform player;

    static private List<Vector3> gridPos;


    private float minDistanceFromEnemy = 16f; // Minimum distance from the enemy
    private bool pathfindingIsActive = false;
    private Vector3 endTraget;




    //public float avoidRadius = (float)0.01;



    private void startpFinding()
    {
        //pFinding = new PathFinding(roomFirstMapGenerator.getMapWidth, roomFirstMapGenerator.getMapHeight);
        if (pFinding == null)
        {
            pFinding = new PathFinding(roomFirstMapGenerator.getMapWidth, roomFirstMapGenerator.getMapHeight);
        }
        var floor = roomFirstMapGenerator.getFloor();

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        HashSet<Vector2Int> enemiesArea = new HashSet<Vector2Int>();


        foreach (GameObject enemy in enemies)
        {

            for (int dx = -2; dx <= 2; dx++)
            {
                for (int dy = -2; dy <= 2; dy++)
                {
                    Vector2Int tilePos = new Vector2Int((int)enemy.transform.position.x + dx, (int)enemy.transform.position.y + dy);
                    floor.Remove(tilePos);
                }
            }



        }

        pFinding.createWalkabelGrid(floor);
        if (debugVar)
        {
            pFinding.createGrid(floor);

        }
    }


    private void stoppFinding()
    {
        gridPos = null;
        pathfindingIsActive = false;
        StopAllCoroutines();

    }

    private void Start()
    {
        //startpFinding();

    }
    void UpdatePathfinding()
    {
        try
        {
            StopAllCoroutines();
            startpFinding();
            //Debug.LogWarning("stop you are too close");

            if (endTraget == null)
            {
                Debug.LogError("target is null");
            }

            pFinding.GetGrid().GetXY(endTraget, out int targetX, out int targetY);
            pFinding.GetGrid().GetXY(player.position, out int x, out int y);
            List<Node> path = pFinding.FindPath(x, y, targetX, targetY);
            if (path == null)
            {
                if (debugVar)
                {
                    Debug.LogWarning("path node is null");

                }
                //startpFinding();
                path = pFinding.FindPath(x, y, targetX, targetY);
            }

            if (debugVar)
            {
                for (int i = 0; i < path.Count - 1; i++)
                {
                    Debug.DrawLine(nodeToV3(path[i]), nodeToV3(path[i + 1]), Color.red, 10f);
                    Debug.DrawRay(nodeToV3(path[i]), Vector3.up * 0.2f, Color.blue, 10f);
                }
            }


            gridPos = NodetoVector3(path);
            pathfindingIsActive = true;
            StartCoroutine(FollowPath());

            gridPos = null;
        }
        catch (NullReferenceException e)
        {
            if (debugVar)
            {
                Debug.LogWarning(e.Message);
            }

        }
    }
    IEnumerator WaitAndExecute(float seconds)
    {
        yield return new WaitForSeconds(seconds); // Wait for the specified number of seconds
        UpdatePathfinding(); // Call your pathfinding update logic
    }


    private void FixedUpdate()
    {
        if (updateEnmeyPos() && pathfindingIsActive)
        {
            StartCoroutine(WaitAndExecute(0.1f)); // Wait for 1 second before updating pathfinding
        }
    }

    private void Update()
    {



        if (Input.GetMouseButton(0))
        {

            try
            {

                if (roomFirstMapGenerator.getFloor() == null)
                {
                    if (debugVar)
                    {
                        Debug.Log(" floor is null 789 ");

                    }
                }
                else if (pFinding == null)
                {
                    if (debugVar)
                    {
                        Debug.Log(" pFindfin is null 789 ");

                    }

                }
                else
                {
                    pFinding.createWalkabelGrid(roomFirstMapGenerator.getFloor());
                    if (debugVar)
                    {
                        pFinding.createGrid(roomFirstMapGenerator.getFloor());

                    }

                }
                StopAllCoroutines();
                // Get the mouse position in world space
                Vector3 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                targetPosition.z = player.position.z; // Ensure the z-coordinate remains unchanged
                endTraget = targetPosition;
                // Get grid coordinates for the mouse position

                if (pFinding == null)
                {
                    if (debugVar)
                    {
                        Debug.LogError("pFinding is null");

                    }
                    startpFinding();

                }

                pFinding.GetGrid().GetXY(targetPosition, out int targetX, out int targetY);
                pFinding.GetGrid().GetXY(player.position, out int x, out int y);

                List<Node> path = pFinding.FindPath(x, y, targetX, targetY);

                if (path == null)
                {
                    if (debugVar)
                    {
                        Debug.LogWarning("path node is null");


                    }
                    startpFinding();
                    path = pFinding.FindPath(x, y, targetX, targetY);
                }
                for (int i = 0; i < path.Count - 1; i++)
                {
                    Debug.DrawLine(nodeToV3(path[i]), nodeToV3(path[i + 1]), Color.red, 10f);
                    Debug.DrawRay(nodeToV3(path[i]), Vector3.up * 0.2f, Color.green, 10f);

                }
                gridPos = NodetoVector3(path);

                pathfindingIsActive = true;
                endTraget = targetPosition;
                StartCoroutine(FollowPath());

                gridPos = null;


            }
            catch (NullReferenceException e)
            {
                if (debugVar)
                {
                    Debug.LogWarning(e.Message);

                }
            }


        }
        else if (Input.anyKey)
        {

            stoppFinding();


        }





    }




    IEnumerator FollowPath()
    {
        foreach (Vector3 pos in gridPos)
        {
            while (Vector3.Distance(transform.position, pos) > 1f)
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
        return new Vector3(node.X, node.Y) * pFinding.GetGrid().GetCellSize() + Vector3.one * (pFinding.GetGrid().GetCellSize() / 2);
    }

    bool updateEnmeyPos()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            Vector3 directionToEnemy = transform.position - enemy.transform.position;
            if (directionToEnemy.magnitude < minDistanceFromEnemy)
            {

                return true;
            }
        }
        return false;
    }

}
