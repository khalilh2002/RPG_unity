using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    private int x , y;
    private Grid<Node> grid;

    public int gCost;
    public int fCost;
    public int hCost;

    public bool isWalkable;

    public Node cameFromNode;

    public int X { get => x; set => x = value; }
    public int Y { get => y; set => y = value; }

    public Node(Grid<Node> grid ,int x, int y)
    {
        this.grid = grid;

        this.x = x;
        this.y = y;

        isWalkable = false;
  
    }

    public void setIsWalkable(bool answer)
    {
        isWalkable = answer;
    }
    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public override string ToString()
    {
        return x + " " + y;
    }
}
