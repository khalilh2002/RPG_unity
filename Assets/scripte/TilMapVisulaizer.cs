using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilMapVisulaizer : MonoBehaviour
{
   [SerializeField]private Tilemap floorTilmap ,wallTilemap;
   [SerializeField]
   private TileBase floorTile , wallTop, wallSideRight, wallSideLeft, wallBottom, wallFull,
   wallInnerCornerDownLeft, wallInnerCornerDownRight,
   wallDiagonalCornerDownLeft,wallDiagonalCornerDownRight, wallDiagonalCornerUpRight, wallDiagonalCornerUpLeft ;

   public void paintFloorTiles(IEnumerable<Vector2Int> floorPosition){
        paintTiles(floorPosition,floorTilmap,floorTile);
   }

    private void paintTiles(IEnumerable<Vector2Int> Positions, Tilemap Tilmap, TileBase Tile)
    {
        
        foreach (var item in Positions)
        {
            paintSingleTile(Tilmap , Tile , item);
        }
    }

    private void paintSingleTile(Tilemap tilmap, TileBase tile, Vector2Int position)
    {
        var tilePosition = tilmap.WorldToCell((Vector3Int)position);
        tilmap.SetTile(tilePosition,tile);
    }

    public void paintWallTiles(Vector2Int position, string BinaryType)
    {
        int typeAsInt = Convert.ToInt32(BinaryType, 2);
        TileBase tile = null;
        if(WallTypesHelper.wallTop.Contains(typeAsInt)){
            tile = wallTop;
        }else if(WallTypesHelper.wallSideRight.Contains(typeAsInt)){
            tile = wallSideRight;
        }else if(WallTypesHelper.wallSideLeft.Contains(typeAsInt)){
            tile = wallSideLeft;
        }else if(WallTypesHelper.wallBottm.Contains(typeAsInt)){
            tile = wallBottom;
        }else if(WallTypesHelper.wallFull.Contains(typeAsInt)){
            tile = wallFull;
        }
        if(tile!=null)
        paintSingleTile(wallTilemap,tile,position);
    }
    public void clear(){
        floorTilmap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
    }

    internal void PaintSingleCornerWall(Vector2Int position, string BinaryType)
    {
        int typeAsInt = Convert.ToInt32(BinaryType, 2);
        TileBase tile = null;
        if(WallTypesHelper.wallTop.Contains(typeAsInt)){

            tile = wallTop;

        }else if(WallTypesHelper.wallInnerCornerDownLeft.Contains(typeAsInt)){

            tile = wallInnerCornerDownLeft;

        }else if(WallTypesHelper.wallInnerCornerDownRight.Contains(typeAsInt)){

            tile = wallInnerCornerDownRight;

        }else if(WallTypesHelper.wallDiagonalCornerDownLeft.Contains(typeAsInt)){

            tile = wallDiagonalCornerDownLeft;

        }else if(WallTypesHelper.wallDiagonalCornerDownRight.Contains(typeAsInt)){

            tile = wallDiagonalCornerDownRight;

        }else if(WallTypesHelper.wallDiagonalCornerUpRight.Contains(typeAsInt)){

            tile = wallDiagonalCornerUpRight;

        }else if(WallTypesHelper.wallDiagonalCornerUpLeft.Contains(typeAsInt)){

            tile = wallDiagonalCornerUpLeft;

        }else if(WallTypesHelper.wallFullEightDirections.Contains(typeAsInt)){

            tile = wallFull;

        }else if(WallTypesHelper.wallBottmEightDirections.Contains(typeAsInt)){

            tile = wallBottom;

        }
        if(tile!=null)
        paintSingleTile(wallTilemap,tile,position);
    }
}


//hello ApplyPathFinding