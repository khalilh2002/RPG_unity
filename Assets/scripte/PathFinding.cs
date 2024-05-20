using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Unity.Mathematics;
using UnityEngine;
using static System.Collections.Specialized.BitVector32;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PathFinding
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 10;

    private Grid<Node> gridPathFinding;
    private Vector3 origin = Vector3.zero;


    private List<Node> openList;
    private List<Node> closeList;


    public PathFinding(int width, int height)
    {
        gridPathFinding = new Grid<Node>(width, height, 2f, origin, (Grid<Node> g, int x, int y) => new Node(g, x, y));

    }




    public List<Node> FindPath(int startX, int startY, int endX, int endY)
    {
        Node startNode = gridPathFinding.GetGridObject(startX, startY);
        Node endNode = gridPathFinding.GetGridObject(endX, endY);
        if (startNode == null || endNode == null)
        {
            // Invalid Path
            return null;
        }

        openList = new List<Node> { startNode };
        closeList = new List<Node>();

        for (int x = 0; x < gridPathFinding.GetWidth(); x++)
        {
            for (int y = 0; y < gridPathFinding.GetHeight(); y++)
            {
                Node pathNode = gridPathFinding.GetGridObject(x, y);

                if (pathNode == null)
                {
                    Debug.Log(" you path Node null 789 x = " + x + " y = " + y);
                }
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateFCost();
                pathNode.cameFromNode = null;
            }
        }
        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            Node currentNode = GetLowestFCost(openList);
            if (currentNode == endNode)
            {
                openList.Clear();
                closeList.Clear();
                return CalculatePath(endNode);
            }
            openList.Remove(currentNode);
            closeList.Add(currentNode);
            foreach (Node node in GetNeighbourList(currentNode))
            {
                if (closeList.Contains(node))
                {
                    continue;
                }
                if (!node.isWalkable)
                {
                    closeList.Add(node);
                    continue;
                }
                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, node);
                if (tentativeGCost < node.gCost)
                {
                    node.cameFromNode = currentNode;
                    node.gCost = tentativeGCost;
                    node.hCost = CalculateDistanceCost(node, endNode);
                    node.CalculateFCost();
                    if (!openList.Contains(node))
                    {
                        openList.Add(node);
                    }

                }
            }
        }
        openList.Clear();
        closeList.Clear();
        // out of nodes in open list coldnt find the path 
        return null;
    }

    private List<Node> GetNeighbourList(Node currentNode)
    {
        List<Node> neighbourList = new List<Node>();
        if (currentNode.X - 1 >= 0)
        {
            //left
            neighbourList.Add(GetNode(currentNode.X - 1, currentNode.Y));
            //if (currentNode.Y - 1 >= 0)
            //{
            //    //down left
            //    neighbourList.Add(GetNode(currentNode.X - 1, currentNode.Y-1));

            //}
            //if (currentNode.Y + 1 < gridPathFinding.GetHeight())
            //{
            //    //down up
            //    neighbourList.Add(GetNode(currentNode.X - 1, currentNode.Y +1));

            //}

        }
        if (currentNode.X + 1 < gridPathFinding.GetWidth())
        {
            //right
            neighbourList.Add(GetNode(currentNode.X + 1, currentNode.Y));

        }

        if (currentNode.Y + 1 < gridPathFinding.GetHeight())
        {
            //down up
            neighbourList.Add(GetNode(currentNode.X, currentNode.Y + 1));

        }
        if (currentNode.Y - 1 >= 0)
        {
            //down up
            neighbourList.Add(GetNode(currentNode.X, currentNode.Y - 1));

        }
        return neighbourList;

    }





    private List<Node> CalculatePath(Node endNode)
    {
        List<Node> path = new List<Node>();
        path.Add(endNode);
        Node currentNode = endNode;
        while (currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }
        path.Reverse();
        return path;
    }
    public Node GetNode(int x, int y)
    {
        return gridPathFinding.GetGridObject(x, y);
    }
    public Grid<Node> GetGrid()
    {
        return gridPathFinding;
    }

    private Node GetLowestFCost(List<Node> pathNodeList)
    {
        Node lowestFCost = pathNodeList[0];
        for (int i = 0; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].fCost < lowestFCost.fCost)
            {
                lowestFCost = pathNodeList[i];
            }
        }
        return lowestFCost;
    }


    private int CalculateDistanceCost(Node a, Node b)
    {
        int xDistance = Mathf.Abs(a.X - b.X);
        int yDistance = Mathf.Abs(a.Y - b.Y);

        int remaining = Mathf.Abs(xDistance - yDistance);

        int res = MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) * MOVE_STRAIGHT_COST * remaining;
        return res;
    }


    public void createGrid(HashSet<Vector2Int> floor)
    {
        gridPathFinding.createGrid(floor);
    }


    //public void createWalkabelGrid(List<GameObject> listObj , HashSet<Vector2Int> floor)
    //{
    //    foreach (GameObject obj in listObj)
    //    {
    //        Vector2Int objV = new Vector2Int((int)obj.transform.position.x, (int)obj.transform.position.y);
    //        for (int x = 0; x < gridPathFinding.GetWidth(); x++)
    //        {
    //            for (int y = 0; y < gridPathFinding.GetHeight(); y++)
    //            {
    //                Vector2Int xitem = new Vector2Int((int)gridPathFinding.getWorldPosition(x, y).x, (int));

    //                if (!floor.Contains(xitem))
    //                {
    //                    continue;
    //                }
    //                if (Vector2Int.Distance(xitem,objV)<0.1f)
    //                {
    //                    GetNode(x,y).setIsWalkable(false);
    //                }

    //            }
    //        }

    //    }
    //}

    public void createWalkabelGrid(HashSet<Vector2Int> floor)
    {
        for (int x = 0; x < gridPathFinding.GetWidth(); x++)
        {
            for (int y = 0; y < gridPathFinding.GetHeight(); y++)
            {
                Vector2Int xitem = new Vector2Int((int)gridPathFinding.getWorldPosition(x, y).x, (int)gridPathFinding.getWorldPosition(x, y).y);
                if (floor == null)
                {
                    Debug.Log("value is null 1254");

                }
                if (WallGenerator.wallsPositions.Contains(xitem))
                {
                    GetNode(x, y).setIsWalkable(false);

                }
                else if (floor.Contains(xitem))
                {

                    GetNode(x, y).setIsWalkable(true);
                }
                else
                {

                    GetNode(x, y).setIsWalkable(false);

                }

            }
        }
        
    }




}
