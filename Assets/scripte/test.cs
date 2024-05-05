using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    [SerializeField] RoomFirstMapGenerator roomFirstMapGenerator;

    Grid g;
    // Start is called before the first frame update
    void Start()
    {
        int x = roomFirstMapGenerator.getMapWidth;
        int y = roomFirstMapGenerator.getMapHeight;
        float cell = 2f;

        g = new Grid((int)(x / cell) + 1, (int)(y / cell) + 1, cell , roomFirstMapGenerator.getFloor , Vector3.zero);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.G))
        {
            removeAll();
            int x = roomFirstMapGenerator.getMapWidth;
            int y = roomFirstMapGenerator.getMapHeight;
            float cell = 2f;

            g = new Grid((int)(x / cell) + 1, (int)(y / cell) + 1, cell, roomFirstMapGenerator.getFloor , Vector3.zero);

        }

    }

    void removeAll()
    {
        // Find all existing coins and enemies
        GameObject[] existingCoins = GameObject.FindGameObjectsWithTag("gridCell");

        // Destroy all existing coins and enemies except the original ones
        foreach (GameObject coin in existingCoins)
        {
                Destroy(coin);
        }

    }
}
