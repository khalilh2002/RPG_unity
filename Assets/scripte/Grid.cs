using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Runtime.CompilerServices;


public class Grid 
{
    public const int sortingOrderDefault = 5000;
    int width, height;
    float cellSize;
    private int[,] gridArray;
    Vector3 origin;

    public Grid(int width, int height, float cellSize , HashSet<Vector2Int> floor , Vector3 origin )
    {
        this.origin = origin;
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.gridArray = new int[width,height]  ;

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                Vector2Int xitem = new Vector2Int((int)getWorldPosition(x, y).x, (int)getWorldPosition(x, y).y);
                if (floor == null)
                {
                    Debug.Log("value is null 1254");
                }
                else if (floor.Contains(xitem))
                {
                    //CreateWorldText(gridArray[x, y].ToString(), null, getWorldPosition(x, y), 20, Color.green, TextAnchor.MiddleCenter).tag = "gridCell";
                    Debug.DrawLine(getWorldPosition(x, y), getWorldPosition(x + 1, y), Color.green, 100f);
                    Debug.DrawLine(getWorldPosition(x, y), getWorldPosition(x, y + 1), Color.green, 100f);
                }
                else
                {
                    //CreateWorldText(gridArray[x, y].ToString(), null, getWorldPosition(x, y), 20, Color.red, TextAnchor.MiddleCenter).tag = "gridCell";
                    Debug.DrawLine(getWorldPosition(x, y), getWorldPosition(x + 1, y), Color.red, 100f);
                    Debug.DrawLine(getWorldPosition(x, y), getWorldPosition(x, y + 1), Color.red, 100f);
                }
                
            }
         }
    }
    
    private Vector3 getWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + origin;
    }

    public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = sortingOrderDefault)
    {
        if (color == null) color = Color.white;
        return CreateWorldText(parent, text, localPosition, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
    }

    // Create Text in the World
    public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
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


