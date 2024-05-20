using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class test : MonoBehaviour
{
    [SerializeField] RoomFirstMapGenerator roomFirstMapGenerator;
    [SerializeField] float speed = 50f;

    private PathFinding pFinding;

    [SerializeField]
    Transform player;

    static private List<Vector3> gridPos;


    private float minDistanceFromEnemy = 20f; // Minimum distance from the enemy
    private bool pathfindingIsActive = false;
    private Vector3 endTraget;


    public float avoidRadius = (float)0.01;


    //private void startpFinding()
    //{

    //    pFinding = new PathFinding(roomFirstMapGenerator.getMapWidth, roomFirstMapGenerator.getMapHeight);
    //    var floor = roomFirstMapGenerator.getFloor;

    //    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
    //    List<Bounds> enemiesArea = new List<Bounds>();

    //    foreach (GameObject enemy in enemies)
    //    {
    //        Vector3 size = new Vector3(avoidRadius * 2, avoidRadius * 2, 0); // Size of the bounding box

    //        enemiesArea.Add(new Bounds(enemy.transform.position, size));

    //        //floor.Remove(new Vector2Int((int)enemy.transform.position.x, (int)enemy.transform.position.y));
    //        //floor.Remove(new Vector2Int((int)enemy.transform.position.x+1, (int)enemy.transform.position.y));

    //        //floor.Remove(new Vector2Int((int)enemy.transform.position.x-1, (int)enemy.transform.position.y));


    //        //floor.Remove(new Vector2Int((int)enemy.transform.position.x, (int)enemy.transform.position.y+1));
    //        //floor.Remove(new Vector2Int((int)enemy.transform.position.x, (int)enemy.transform.position.y-1));


    //    }
    //    foreach (Bounds area in enemiesArea)
    //    {
    //        foreach (Vector2Int vect in floor)
    //        {
    //            if (area.Contains(new Vector3(vect.x, vect.y, area.center.z)))
    //            {
    //                floor.Remove(vect);
    //                break;
    //            }
    //        }

    //    }

    //    pFinding.createWalkabelGrid(floor);
    //    pFinding.createGrid(floor);
    //}

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

        // Precompute bounds for enemies
        //foreach (GameObject enemy in enemies)
        //{
        //    Vector3 size = new Vector3(avoidRadius, avoidRadius, 0); // Size of the bounding box
        //    Bounds bounds = new Bounds(enemy.transform.position, size);

        //    // Add all tiles within the bounds to enemiesArea
        //    for (int x = Mathf.FloorToInt(bounds.min.x); x <= Mathf.FloorToInt(bounds.max.x); x++)
        //    {
        //        for (int y = Mathf.FloorToInt(bounds.min.y); y <= Mathf.FloorToInt(bounds.max.y); y++)
        //        {
        //            enemiesArea.Add(new Vector2Int(x, y));
        //        }
        //    }
        //}

        foreach (GameObject enemy in enemies)
        {


            //floor.Remove(new Vector2Int((int)enemy.transform.position.x, (int)enemy.transform.position.y));
            //floor.Remove(new Vector2Int((int)enemy.transform.position.x + 1, (int)enemy.transform.position.y));

            //floor.Remove(new Vector2Int((int)enemy.transform.position.x - 1, (int)enemy.transform.position.y));


            //floor.Remove(new Vector2Int((int)enemy.transform.position.x, (int)enemy.transform.position.y + 1));
            //floor.Remove(new Vector2Int((int)enemy.transform.position.x, (int)enemy.transform.position.y - 1));

            //floor.Remove(new Vector2Int((int)enemy.transform.position.x+1, (int)enemy.transform.position.y + 1));
            //floor.Remove(new Vector2Int((int)enemy.transform.position.x+1, (int)enemy.transform.position.y - 1));

            //floor.Remove(new Vector2Int((int)enemy.transform.position.x -1, (int)enemy.transform.position.y + 1));
            //floor.Remove(new Vector2Int((int)enemy.transform.position.x - 1, (int)enemy.transform.position.y + 1));

            for (int dx = -2; dx <= 2; dx++)
            {
                for (int dy = -2; dy <= 2; dy++)
                {
                    Vector2Int tilePos = new Vector2Int( (int)enemy.transform.position.x + dx, (int)enemy.transform.position.y + dy);
                    floor.Remove(tilePos);
                }
            }



        }




        // Remove floor tiles within enemiesArea
        // floor.ExceptWith(enemiesArea);

        // Recreate walkable grid
        pFinding.createWalkabelGrid(floor);
        pFinding.createGrid(floor);
    }


    private void stoppFinding()
    {
        gridPos = null;
        pathfindingIsActive = false;
        StopAllCoroutines();

    }




    private void CheckEnemyPositionCoroutine()
    {

        if (updateEnmeyPos())
        {
            try
            {
                StopAllCoroutines();
                startpFinding();
                Debug.LogWarning("stop you are to clos");

                if (endTraget == null)
                {
                    Debug.LogError("target is null");
                }
                pFinding.GetGrid().GetXY(endTraget, out int targetX, out int targetY);
                pFinding.GetGrid().GetXY(player.position, out int x, out int y);
                List<Node> path = pFinding.FindPath(x, y, targetX, targetY);
                if (path == null)
                {
                    Debug.LogWarning("path node is null");
                    Debug.LogWarning("path node is null");
                    startpFinding();
                    path = pFinding.FindPath(x, y, targetX, targetY);
                }
                for (int i = 0; i < path.Count - 1; i++)
                {
                    Debug.DrawLine(nodeToV3(path[i]), nodeToV3(path[i + 1]), Color.red, 10f);
                    Debug.DrawRay(nodeToV3(path[i]), Vector3.up * 0.2f, Color.blue, 10f);

                }
                gridPos = NodetoVector3(path);

                pathfindingIsActive = true;
                StartCoroutine(FollowPath());

                gridPos = null;
                waitS();
            }
            catch (NullReferenceException e)
            {
                Debug.LogWarning(e.Message);

            }

        }

    }





    private void Start()
    {
        startpFinding();

    }
    void UpdatePathfinding()
    {
        try
        {
            StopAllCoroutines();
            startpFinding();
            Debug.LogWarning("stop you are too close");

            if (endTraget == null)
            {
                Debug.LogError("target is null");
            }

            pFinding.GetGrid().GetXY(endTraget, out int targetX, out int targetY);
            pFinding.GetGrid().GetXY(player.position, out int x, out int y);
            List<Node> path = pFinding.FindPath(x, y, targetX, targetY);
            if (path == null)
            {
                Debug.LogWarning("path node is null");
                startpFinding();
                path = pFinding.FindPath(x, y, targetX, targetY);
            }

            for (int i = 0; i < path.Count - 1; i++)
            {
                Debug.DrawLine(nodeToV3(path[i]), nodeToV3(path[i + 1]), Color.red, 10f);
                Debug.DrawRay(nodeToV3(path[i]), Vector3.up * 0.2f, Color.blue, 10f);
            }

            gridPos = NodetoVector3(path);
            pathfindingIsActive = true;
            StartCoroutine(FollowPath());

            gridPos = null;
        }
        catch (NullReferenceException e)
        {
            Debug.LogWarning(e.Message);
        }
    }
    IEnumerator WaitAndExecute(float seconds)
    {
        yield return new WaitForSeconds(seconds); // Wait for the specified number of seconds
        UpdatePathfinding(); // Call your pathfinding update logic
    }

    private void Update()
    {
        if (updateEnmeyPos() && pathfindingIsActive)
        {
            StartCoroutine(WaitAndExecute(0.3f)); // Wait for 1 second before updating pathfinding
        }



        //if (updateEnmeyPos() && pathfindingIsActive)
        //{
        //    try
        //    {
        //        StopAllCoroutines();
        //        startpFinding();
        //        Debug.LogWarning("stop you are to clos");

        //        if (endTraget == null)
        //        {
        //            Debug.LogError("target is null");
        //        }
        //        pFinding.GetGrid().GetXY(endTraget, out int targetX, out int targetY);
        //        pFinding.GetGrid().GetXY(player.position, out int x, out int y);
        //        List<Node> path = pFinding.FindPath(x, y, targetX, targetY);
        //        if (path == null)
        //        {
        //            Debug.LogWarning("path node is null");
        //            Debug.LogWarning("path node is null");
        //            startpFinding();
        //            path = pFinding.FindPath(x, y, targetX, targetY);
        //        }
        //        for (int i = 0; i < path.Count - 1; i++)
        //        {
        //            Debug.DrawLine(nodeToV3(path[i]), nodeToV3(path[i + 1]), Color.red, 10f);
        //            Debug.DrawRay(nodeToV3(path[i]), Vector3.up * 0.2f, Color.blue, 10f);

        //        }
        //        gridPos = NodetoVector3(path);

        //        pathfindingIsActive = true;
        //        StartCoroutine(FollowPath());
        //        gridPos = null;



        //    }
        //    catch (NullReferenceException e)
        //    {
        //        Debug.LogWarning(e.Message);

        //    }

        //}


        if (Input.GetMouseButton(0))
        {

            try
            {

                if (roomFirstMapGenerator.getFloor() == null)
                {
                    Debug.Log(" floor is null 789 ");
                }
                else if (pFinding == null)
                {
                    Debug.Log(" pFindfin is null 789 ");

                }
                else
                {
                    pFinding.createWalkabelGrid(roomFirstMapGenerator.getFloor());
                    pFinding.createGrid(roomFirstMapGenerator.getFloor());

                }
                StopAllCoroutines();
                // Get the mouse position in world space
                Vector3 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                targetPosition.z = player.position.z; // Ensure the z-coordinate remains unchanged
                endTraget = targetPosition;
                // Get grid coordinates for the mouse position

                if (pFinding == null)
                {
                    Debug.LogError("pFinding is null");
                    startpFinding();

                }

                pFinding.GetGrid().GetXY(targetPosition, out int targetX, out int targetY);
                pFinding.GetGrid().GetXY(player.position, out int x, out int y);

                List<Node> path = pFinding.FindPath(x, y, targetX, targetY);

                if (path == null)
                {
                    Debug.LogWarning("path node is null");
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
                Debug.LogWarning(e.Message);

            }
        }
        else if (Input.anyKey)
        {

            stoppFinding();


        }





    }

    IEnumerable waitS()
    {
        yield return new WaitForSeconds(1);
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
