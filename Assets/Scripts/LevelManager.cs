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

    public int Width { get => width; set => width = value; }
    public int Height { get => height; set => height = value; }
    public TileScript StartTile { get => startTile; set => startTile = value; }
    public TileScript EndTile { get => endTile; set => endTile = value; }
    public Pathfinder PathFinder { get => pathFinder; set => pathFinder = value; }

    public GameObject tilePrefab;

    public GameObject startPortalPrefab;
    public GameObject endPortalPrefab;


    public GameObject rockPrefab;
    private float rockProbability = 0.11f; // chance of rock
    public Transform RockParent;

    [SerializeField]
    private int width = 15;
    [SerializeField]
    private int height = 10;

    private TileScript startTile;
    private TileScript endTile;

    private TileScript[,] tiles;

    [SerializeField]
    private Pathfinder pathFinder;

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
        RockParent = GameObject.Find("RockParent").transform;

        CreateLevel();
        StartTile = GetTile(0, 0);
        EndTile = GetTile(Width-1, Height-1);
        DrawPortals();
        StartCoroutine(GenerateRocksAndFindPath());

    }

    private void CreateLevel()
    {
        tiles = new TileScript[Width, Height];


        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
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

    IEnumerator GenerateRocksAndFindPath()
    {
        yield return null; // Wait for one frame
        int attempts = 0;
        List<TileScript> path = null;
        do
        {
            if (attempts > 100)
            {
                break;
            }
            attempts++;
            ClearRocks();
            GenerateRocks();
            path = PathFinder.FindPath(StartTile, EndTile);
        } while (path == null || path.Count == 0);
        Debug.Log("ATTEMPTS: " + attempts);
    }

    private void ClearRocks()
    {
        foreach (var rock in GameObject.FindGameObjectsWithTag("Rock"))
        {
            Destroy(rock);
        }

        // Reset all tiles to be walkable
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                tiles[i, j].IsWalkable = true;
            }
        }
    }

    private void GenerateRocks()
    {
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                TileScript currentTile = tiles[i, j];

                // Do not add rocks to the start or end tiles
                if (currentTile == StartTile || currentTile == EndTile)
                {
                    continue;
                }

                // 20% chance to spawn a rock
                if (UnityEngine.Random.value < rockProbability)
                {
                    // Make sure that a path is still possible with the rock in place
                    currentTile.IsWalkable = false;
                    GameObject rock = Instantiate(rockPrefab, currentTile.transform.position + new Vector3(0, 0, -1), Quaternion.identity, RockParent);
                    rock.AddComponent<DepthSorter>();
                    rock.tag = "Rock";
                }
            }
        }
    }

    private void DrawPortals()
    {
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                if (tiles[i, j] == StartTile)
                {
                    GameObject sp = Instantiate(startPortalPrefab, tiles[i, j].transform.position + new Vector3(0, 0, -1), Quaternion.identity);
                    sp.AddComponent<DepthSorter>();
                }
                else if (tiles[i, j] == EndTile)
                {
                    GameObject ep = Instantiate(endPortalPrefab, tiles[i, j].transform.position + new Vector3(0, 0, -1), Quaternion.identity);
                    ep.AddComponent<DepthSorter>();
                }
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
                if (checkX >= 0 && checkX < Width && checkY >= 0 && checkY < Height)
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
        if (x < 0 || x >= Width || y < 0 || y >= Height)
        {
            return null; // return null if the x or y coordinate is out of bounds
        }

        return tiles[x, y];
    }

    public bool IsOutsideBounds(Vector3 position)
    {
        // Convert the world position to grid position, plus one tile width/height
        int x = Mathf.FloorToInt(position.x);
        int y = Mathf.FloorToInt(position.y);

        // Use GetTile to check if the grid position is valid
        GameObject tile = GetTile(x, y).gameObject;

        if (tile == null)
        {
            return true; // Position is outside of the bounds
        }
        return false; // Position is inside the bounds
    }





    void Update()
    {

    }
}

