using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;

    // Access the singleton instance of LevelManager
    public static LevelManager Instance
    {
        get { return instance; }
    }


    public GameObject tilePrefab;
    private int width = 15;
    private int height = 10;

    private TileScript[,] tiles;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

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

    public List<TileScript> GetNeighbours(TileScript tile)
    {
        List<TileScript> neighbours = new List<TileScript>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue; // This is the tile itself, not a neighbour
                }

                int checkX = tile.X + x;
                int checkY = tile.Y + y;

                // Check if the neighbour is inside the grid
                if (checkX >= 0 && checkX < width && checkY >= 0 && checkY < height)
                {
                    // Check if there is a tower in a diagonally adjacent tile, if so, skip this iteration
                    if (Mathf.Abs(x) == 1 && Mathf.Abs(y) == 1)
                    {
                        // Checking the tiles that would be skipped if we moved diagonally
                        TileScript adjacent1 = tiles[tile.X + x, tile.Y];
                        TileScript adjacent2 = tiles[tile.X, tile.Y + y];

                        if (!adjacent1.IsWalkable || !adjacent2.IsWalkable)
                        {
                            continue; // Skip this neighbour as it would mean moving diagonally adjacent to a tower
                        }
                    }

                    neighbours.Add(tiles[checkX, checkY]);
                }
            }
        }

        return neighbours;
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

