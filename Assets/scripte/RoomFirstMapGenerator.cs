using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random ;

public class RoomFirstMapGenerator : simpleWalkMapGenerator
{
    //var to send to the Main in wich he will be posioned
    public static BoundsInt FirstRoom ;
    public static List<BoundsInt>  listRoomOrigin ;
    public static Dictionary<BoundsInt, double> djikstra_result ;

    //addede for djikstra
    Graph graph_main = new Graph();

    

    [SerializeField]
    private int minRoomWidth = 10 , minRoomHeight = 10 ;
    [SerializeField]
    private int mapWidth = 53 , mapHeight = 53;
    [SerializeField][Range(0,10)]
    private int offset = 3 ;
    //private bool randomWalkRooms = false ;

    protected override void RunProceduralGeneration()
    {
        createRooms();
    }

    private void createRooms()
    {
        var roomlist = GenerateMapAlgorithm.BinarySpacePartition( new BoundsInt((Vector3Int)startPosition , new Vector3Int(mapWidth,mapHeight,0))
                                                                    ,minRoomWidth ,minRoomHeight);
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        floor = createSimpleRooms(roomlist);
        
        //add the first room createdd to the var firstroom to send it to the player

        FirstRoom = roomlist[0];
        listRoomOrigin = roomlist;

        //list of centers of rooms
        List<Vector2Int> roomCenters = new List<Vector2Int>();
        foreach (var room in roomlist)
        {
            Vector2Int center = (Vector2Int)Vector3Int.RoundToInt(room.center); 
            roomCenters.Add(center);
        }

        HashSet<Vector2Int> corridors = ConnectRooms(roomCenters , roomlist);
        floor.UnionWith(corridors);
        Debug.Log("hello this is floor content " + floor);
        tilmapVisulaizer.paintFloorTiles(floor);
        WallGenerator.createWalls(floor,tilmapVisulaizer);
    }

 

    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters , List<BoundsInt> listrooms)
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        var currentRoomCenter = roomCenters[0];
        roomCenters.Remove(currentRoomCenter);
        while (roomCenters.Count > 0)
        {
            Vector2Int closest = FindClosestTo(currentRoomCenter , roomCenters);

            foreach (var room in listRoomOrigin)
            {
                if ((Vector2Int)Vector3Int.RoundToInt(room.center) == currentRoomCenter)
                {
                    foreach (var otherRoom in listRoomOrigin)
                    {
                        if ( !room.Equals(otherRoom) && (Vector2Int)Vector3Int.RoundToInt(otherRoom.center) == closest)
                        {
                            double weight = UnityEngine.Vector3.Distance(room.center, otherRoom.center);
                            graph_main.addEdge(room, otherRoom, weight);
                            Debug.Log("add edge wright : " + weight);
                            
                        }
                    }
                }
            }



            roomCenters.Remove(closest);
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter , closest);
           
            currentRoomCenter = closest ;
            corridors.UnionWith(newCorridor);
        }
        

        return corridors;
    }

    //need to triple the corridor
    
     private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        var postion = currentRoomCenter; //start point;
        corridor.Add(postion);
        while (postion.y != destination.y)
        {
            if (postion.y > destination.y)
            {
                postion += Vector2Int.down;
            }else{
                postion += Vector2Int.up;
            }
            
            corridor.Add(postion);
            corridor.Add(postion+Vector2Int.left);
            corridor.Add(postion+Vector2Int.right);

            corridor.Add(postion + Vector2Int.left + Vector2Int.left);
            corridor.Add(postion + Vector2Int.right + Vector2Int.right);
        }
        while (postion.x != destination.x)
        {
            if (postion.x > destination.x)
            {
                postion += Vector2Int.left;
            }else{
                postion += Vector2Int.right;
            }
            corridor.Add(postion);

            corridor.Add(postion + Vector2Int.up);
            corridor.Add(postion + Vector2Int.down);

            corridor.Add(postion + Vector2Int.up + Vector2Int.up);
            corridor.Add(postion + Vector2Int.down + Vector2Int.down);

        }
    
        return corridor;
    }
  



    private Vector2Int FindClosestTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters)
    {
        Vector2Int closest = Vector2Int.zero;
        float distance = float.MaxValue;
        foreach (var position in roomCenters)
        {
            float distance_test = Vector2Int.Distance( position , currentRoomCenter);
            if (distance > distance_test)
            {
                distance = distance_test ;
                closest = position;
            }
        }
        return closest ; 
    }



    private HashSet<Vector2Int> createSimpleRooms(List<BoundsInt> roomlist)
    {   HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        foreach (var room in roomlist)
        {
            //Vector3Int min = room.min + new Vector3Int(offset, offset, 0);
            //Vector3Int max = room.max - new Vector3Int(offset, offset, 0);
            //BoundsInt adjustedBounds = new BoundsInt(min, max - min);
            //try {
            //    listRoomOrigin.Add(adjustedBounds);

            //}
            //catch (NullReferenceException e)
            //{
            //    Debug.Log("room adjustBounds :" + adjustedBounds);

            //    Debug.Log("room adjustBounds max :" + min);
            //    Debug.Log("room adjustBounds min :" + max);
            //    Debug.LogError(e.Message);
            //}
            // Add the adjusted room bounds to the list


            for (int column = offset; column < room.size.x - offset; column++)
            {
                for (int row = offset; row < room.size.y - offset; row++){
                    Vector2Int position =  (Vector2Int)room.min + new Vector2Int(column , row );
                    //BoundsInt adjustedBounds = new BoundsInt(new Vector3Int(position.x,position.y,0) , room.max- new Vector3Int(position.x, position.y, 0) );

                    floor.Add(position);
                }
            }
        } 

        return floor;
    }

    //funtion to call the run Procedural Map and clear the previeous one 


    public void runRoomFirstMapGeneratorClass(){
        tilmapVisulaizer.clear();
        graph_main.clear();
        RunProceduralGeneration();
        Debug.Log("runfirstvoid END END");
        djikstra_result = graph_main.Dijkstra(FirstRoom);
        foreach (var item in djikstra_result)
        {
            Debug.Log("item : " + item);
        }

    }




}



public class Graph
{
    Dictionary<BoundsInt, List<(BoundsInt, double)>> vertex;

    public Graph()
    {
        vertex = new Dictionary<BoundsInt, List<(BoundsInt, double)>>();
    }

    public void addEdge(BoundsInt source, BoundsInt target, double weight)
    {
        if (!vertex.ContainsKey(source))
        {
            vertex[source] = new List<(BoundsInt, double)>();
        }

        vertex[source].Add((target, weight));

        // Since it's an undirected graph, add an edge from target to source as well
        if (!vertex.ContainsKey(target))
        {
            vertex[target] = new List<(BoundsInt, double)>();
        }

        vertex[target].Add((source, weight));
    }

    public List<(BoundsInt, double)> GetNeighbors(BoundsInt vertex)
    {
        if (this.vertex.ContainsKey(vertex))
        {
            return this.vertex[vertex];
        }
        else
        {
            return new List<(BoundsInt, double)>();
        }
    }
    public void clear()
    {
        vertex.Clear();
    }

    public void Display()
    {
        foreach (var item in vertex)
        {
            Debug.Log($"Vertex: {item.Key}");

            foreach (var neighbor in item.Value)
            {
                Debug.Log($"  Neighbor: {neighbor.Item1}, Weight: {neighbor.Item2}");
            }
        }
    }

    public Dictionary<BoundsInt, double> Dijkstra(BoundsInt start)
    {
        // Initialize distances dictionary with infinity for all vertices except the start vertex
        Dictionary<BoundsInt, double> distances = new Dictionary<BoundsInt, double>();
        foreach (var vertex in vertex.Keys)
        {
            distances[vertex] = double.PositiveInfinity;
        }
        distances[start] = 0;

        // Priority queue to keep track of vertices to visit next
        var queue = new PriorityQueue<BoundsInt>();
        queue.Enqueue(start, 0);

        while (!queue.IsEmpty)
        {
            var currentVertex = queue.Dequeue();

            // Check all neighbors of the current vertex
            foreach (var (neighbor, weight) in vertex[currentVertex])
            {
                // Calculate the new distance
                double newDistance = distances[currentVertex] + weight;

                // Update distance if newDistance is shorter
                if (newDistance < distances[neighbor])
                {
                    distances[neighbor] = newDistance;
                    queue.Enqueue(neighbor, newDistance);
                }
            }
        }

        return distances;
    }
}

// Helper class for priority queue
public class PriorityQueue<T>
    {
        private SortedDictionary<double, Queue<T>> dict;

        public PriorityQueue()
        {
            dict = new SortedDictionary<double, Queue<T>>();
        }

        public bool IsEmpty => dict.Count == 0;

        public void Enqueue(T item, double priority)
        {
            if (!dict.ContainsKey(priority))
            {
                dict[priority] = new Queue<T>();
            }
            dict[priority].Enqueue(item);
        }

        public T Dequeue()
        {
            var pair = dict.First();
            var item = pair.Value.Dequeue();
            if (pair.Value.Count == 0)
            {
                dict.Remove(pair.Key);
            }
            return item;
        }
    }




//public class Graph
//{
//    // Dictionary to store the adjacency list
//    private Dictionary<BoundsInt, List<(BoundsInt, double)>> adjacencyList;

//    // Constructor to initialize the graph
//    public Graph()
//    {
//        adjacencyList = new Dictionary<BoundsInt, List<(BoundsInt, double)>>();
//    }

//    // Method to add an edge between two vertices with a given weight
//    public void AddEdge(BoundsInt source, BoundsInt destination, double weight)
//    {
//        // Check if the source vertex already exists in the graph
//        if (!adjacencyList.ContainsKey(source))
//        {
//            adjacencyList[source] = new List<(BoundsInt, double)>();
//        }

//        // Add the destination vertex and its weight to the adjacency list of the source vertex
//        adjacencyList[source].Add((destination, weight));

//        // Since it's an undirected graph, add an edge from the destination to the source as well
//        if (!adjacencyList.ContainsKey(destination))
//        {
//            adjacencyList[destination] = new List<(BoundsInt, double)>();
//        }

//        // Add the source vertex and its weight to the adjacency list of the destination vertex
//        adjacencyList[destination].Add((source, weight));
//    }

//    // Method to get the neighbors of a vertex
//    public List<(BoundsInt, double)> GetNeighbors(BoundsInt vertex)
//    {
//        // Check if the vertex exists in the graph
//        if (adjacencyList.ContainsKey(vertex))
//        {
//            return adjacencyList[vertex];
//        }
//        else
//        {
//            // If the vertex doesn't exist, return an empty list
//            return new List<(BoundsInt, double)>();
//        }
//    }
//}



//hello test