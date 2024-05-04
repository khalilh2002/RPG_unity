using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarAlgorithm : RoomFirstMapGenerator
{
}
// Start is called before the first frame update
public class Node
{
    bool walkable;
    UnityEngine.Vector3 position;

    public Node(bool walkable, UnityEngine.Vector3 position)
    {
        this.walkable = walkable;
        this.position = position;
    }
}

public class Grid : RoomFirstMapGenerator
{
    public LayerMask unwalkableMask;
    //public UnityEngine.Vector2 gridWorldSize = new Vector2( RoomFirstMapGenerator, mapHeight);
    public float nodeRadious;
    Node[,] grid;

    int gridSizeX, gridSizeY;
    float nodeDiameter;


    void Start()
    {
        nodeDiameter = nodeRadious * 2;
        //gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        //gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        CreateGrid();

    }

    private void CreateGrid()
    {
        //grid = new Node[gridSizeX, gridSizeY];
        //Vector3 worldBottomleft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward * gridWorldSize.y/2;

        //for (int x = 0; x < gridSizeX; x++)
        //{
        //    for (int y = 0; y < gridSizeY; y++)
        //    {
        //        Vector3 worldPoint = worldBottomleft + Vector3.right * (x * nodeDiameter + nodeRadious) + Vector3.forward * (y * nodeDiameter + nodeRadious);

        //    }
        //}
    }
}


