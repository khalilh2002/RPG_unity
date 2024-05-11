using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Runtime.CompilerServices;
using System.Drawing;


public class Grid<GridOject>
{
    public const int sortingOrderDefault = 5000;
    private int width, height;
    float cellSize;
    private GridOject[,] gridArray;
    Vector3 origin;

    public Grid(int width, int height, float cellSize , Vector3 origin, Func<Grid<GridOject>, int, int, GridOject> createGridObject)
    {
        this.origin = origin;
        this.width = (int)(width / cellSize + 1);
        this.height = (int)(height / cellSize + 1);
        this.cellSize = cellSize;
        this.gridArray = new GridOject[this.width,this.height]  ;

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = createGridObject(this, x, y);
            }
        }

    }
   


   public void createGrid(HashSet<Vector2Int> floor)
    {
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                //floor
                Vector2Int xitem = new Vector2Int((int)getWorldPosition(x, y).x, (int)getWorldPosition(x, y).y);

                //wall

                if (floor == null)
                {
                    Debug.Log("value is null 1254");
                }
                else if (floor.Contains(xitem))
                {
                    //CreateWorldText(gridArray[x, y].ToString(), null, getWorldPosition(x, y), 20, Color.green, TextAnchor.MiddleCenter).tag = "gridCell";
                    Debug.DrawLine(getWorldPosition(x, y), getWorldPosition(x + 1, y), UnityEngine.Color.white, 100f);
                    Debug.DrawLine(getWorldPosition(x, y), getWorldPosition(x, y + 1), UnityEngine.Color.white, 100f);
                }
                else
                {
                    //CreateWorldText(gridArray[x, y].ToString(), null, getWorldPosition(x, y), 20, Color.red, TextAnchor.MiddleCenter).tag = "gridCell";
                    Debug.DrawLine(getWorldPosition(x, y), getWorldPosition(x + 1, y), UnityEngine.Color.blue, 100f);
                    Debug.DrawLine(getWorldPosition(x, y), getWorldPosition(x, y + 1), UnityEngine.Color.blue, 100f);
                }

            }
        }
    }
    
    public Vector3 getWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + origin;
    }



    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

    public float GetCellSize()
    {
        return cellSize;
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - origin).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - origin).y / cellSize);
    }

    public GridOject GetGridObject(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            return default(GridOject);
        }
    }

    public GridOject GetGridObject(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetGridObject(x, y);
    }


}




























    
  






//// Start is called before the first frame update
//public class Node
//{
//    bool walkable;
//    UnityEngine.Vector3 position;

//    public Node(bool walkable, UnityEngine.Vector3 position)
//    {
//        this.walkable = walkable;
//        this.position = position;
//    }
//}

//public class Grid : RoomFirstMapGenerator
//{
//    public LayerMask unwalkableMask;
//    //public UnityEngine.Vector2 gridWorldSize = new Vector2( RoomFirstMapGenerator, mapHeight);
//    public float nodeRadious;
//    Node[,] grid;

//    int gridSizeX, gridSizeY;
//    float nodeDiameter;


//    void Start()
//    {
//        nodeDiameter = nodeRadious * 2;
//        //gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
//        //gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

//        CreateGrid();

//    }

//    private void CreateGrid()
//    {
//        //grid = new Node[gridSizeX, gridSizeY];
//        //Vector3 worldBottomleft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward * gridWorldSize.y/2;

//        //for (int x = 0; x < gridSizeX; x++)
//        //{
//        //    for (int y = 0; y < gridSizeY; y++)
//        //    {
//        //        Vector3 worldPoint = worldBottomleft + Vector3.right * (x * nodeDiameter + nodeRadious) + Vector3.forward * (y * nodeDiameter + nodeRadious);

//        //    }
//        //}
//    }
//}


