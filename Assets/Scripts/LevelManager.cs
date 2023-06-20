using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public int width, height;

    private TileScript[,] tiles;

    void Start()
    {
        CreateLevel();
    }

    private void CreateLevel()
    {
        tiles = new TileScript[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                // Instantiate a new Tile at each grid space, with no rotation.
                GameObject newTile = Instantiate(tilePrefab, new Vector3(i, j, 0), Quaternion.identity);

                // Ensure that the new Tile is a child of this LevelManager for cleanliness in the hierarchy.
                newTile.transform.parent = this.transform;

                // Get the TileScript component and set the X and Y coordinates
                TileScript tileScript = newTile.GetComponent<TileScript>();
                tileScript.X = i;
                tileScript.Y = j;

                // Add the TileScript to the tiles array for later reference
                tiles[i, j] = tileScript;
            }
        }
    }

    public TileScript GetTile(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
        {
            return null; // return null if the x or y coordinate is out of bounds
        }

        return tiles[x, y];
    }

    void Update()
    {

    }
}

