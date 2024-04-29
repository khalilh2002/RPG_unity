using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator
{
    public static void createWalls(HashSet<Vector2Int> floorPosition , TilMapVisulaizer tilMapVisulaizer)
    {
        var basicWallPosition = FindWallsInDirections(floorPosition, Direction2D.cardinalDirectionList);
        var cornerWallPositions = FindWallsInDirections(floorPosition, Direction2D.diagonalDirectionList);
        CreateBasicWalls(tilMapVisulaizer, basicWallPosition, floorPosition);
        CreateCornerWalls(tilMapVisulaizer, cornerWallPositions, floorPosition);
    }

    private static void CreateCornerWalls(TilMapVisulaizer tilMapVisulaizer, HashSet<Vector2Int> cornerWallPositions, HashSet<Vector2Int> floorPosition)
    {
        foreach (var position in cornerWallPositions)
        {
            string neighboursBinaryType = "";
            foreach (var direction in Direction2D.eightDirectionsList)
            {
                var neighbourPosition = position + direction;
                if(floorPosition.Contains(neighbourPosition)){
                    neighboursBinaryType += "1";
                }
                else{
                    neighboursBinaryType += "0";
                }
            }
            tilMapVisulaizer.PaintSingleCornerWall(position, neighboursBinaryType);
        }
    }

    private static void CreateBasicWalls(TilMapVisulaizer tilMapVisulaizer, HashSet<Vector2Int> basicWallPosition, HashSet<Vector2Int> floorPosition)
    {
        foreach (var position in basicWallPosition)
        {
            string neighboursBinaryType = "";
            foreach(var direction in Direction2D.cardinalDirectionList){
                var neighbourPosition = position + direction;
                if(floorPosition.Contains(neighbourPosition)){
                    neighboursBinaryType += "1";
                }
                else{
                    neighboursBinaryType += "0";
                }
            }
            tilMapVisulaizer.paintWallTiles(position, neighboursBinaryType);
        }
    }

    private static HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorPosition, List<Vector2Int> directionList)
    {
        HashSet<Vector2Int> wallPosition = new HashSet<Vector2Int>();
        foreach (var position in floorPosition)
        {
            foreach (var direction in directionList)
            {
                var neighbourPosition = position + direction;
                if (floorPosition.Contains(neighbourPosition) == false)
                {
                    wallPosition.Add(neighbourPosition);
                }
            }
        }
        return wallPosition;
    }
}


//hello test