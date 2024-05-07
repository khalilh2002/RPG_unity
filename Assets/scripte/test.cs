using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class test : MonoBehaviour
{
    [SerializeField] RoomFirstMapGenerator roomFirstMapGenerator;
    [SerializeField] Transform player;

    private PathFinding pFinding;

    private void Start()
    {
        this.pFinding = new PathFinding(roomFirstMapGenerator.getMapWidth, roomFirstMapGenerator.getMapHeight );
        this.pFinding.createWalkabelGrid(roomFirstMapGenerator.getFloor);
        this.pFinding.createGrid(roomFirstMapGenerator.getFloor);


    }
    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            if (pFinding == null)
            {
                Debug.Log(" in K pFindfin is null 789 ");

            }


            Vector3 mousDownPosition = GetMouseWorldPosition();
            pFinding.GetGrid().GetXY(mousDownPosition, out int x, out int y);
            
            Debug.Log("mouse is wlakbale 789"+ pFinding.GetNode(x, y).isWalkable.ToString());
            


            pFinding.GetGrid().GetXY(player.position, out int x1, out int y1);
            
                Debug.Log("playe is wlakbale 789 " + pFinding.GetNode(x1, y1).isWalkable.ToString());   
            

            List<Node> path = pFinding.FindPath(x1, y1, x, y);
            if (path != null)
            {
                for (int i = 0; i < path.Count-1; i++)
                {
                    Debug.DrawLine(new Vector3(path[i].X, path[i].Y) * 2f + Vector3.one * 1f, new Vector3(path[i + 1].X, path[i + 1].Y) * 2f + Vector3.one * 1f, Color.red,100f);
                }
            }
            else
            {
                Debug.Log("path is null 789 " );

            }

        }
        if (Input.GetKey(KeyCode.G))
        {
            //pFinding = new PathFinding(roomFirstMapGenerator.getMapWidth, roomFirstMapGenerator.getMapHeight);

            if (roomFirstMapGenerator.getFloor == null)
            {
                Debug.Log(" floor is null 789 ");
            }
            else if (pFinding == null) {
                Debug.Log(" pFindfin is null 789 ");

            }
            else
            {
                this.pFinding.createWalkabelGrid(roomFirstMapGenerator.getFloor);
                pFinding.createGrid(roomFirstMapGenerator.getFloor);

            }

        }

       


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
