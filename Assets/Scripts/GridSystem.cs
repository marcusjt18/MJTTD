using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridSystem : MonoBehaviour
{
    // Singleton instance
    public static GridSystem Instance { get; private set; }
    public class GridCell
    {
        public Vector3 worldPosition; // World position of the cell
        public int gridX; // X position in the grid
        public int gridY; // Y position in the grid
        public bool walkable; // Can enemies walk on this cell?
        public bool isTower; // Is there a tower here?
        // ... add more properties as needed
    }

    // Define grid properties
    public int width; // Width of the grid
    public int height; // Height of the grid
    public float cellSize; // Size of each cell
    public GridCell[,] gridArray; // The grid itself

    public Tilemap grassTilemap;

    public TileBase grassTile; // Reference to the grass Tile

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the singleton instance
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Multiple instances of GridSystem found. Destroying this instance.");
            Destroy(this);
            return;
        }

        width = 15; // Example value
        height = 10; // Example value
        cellSize = 1.0f; // Example value
        InitializeGrid();
        RenderGrid();
    }

    // Create a method to initialize the grid
    private void InitializeGrid()
    {
        gridArray = new GridCell[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                Vector3 worldPosition = new Vector3(x, y) * cellSize;
                gridArray[x, y] = new GridCell
                {
                    worldPosition = worldPosition,
                    gridX = x,
                    gridY = y,
                    walkable = true, // Default to all cells being walkable
                    isTower = false // Default to no towers
                };
            }
        }
    }

    // Method to render the grid
    private void RenderGrid()
    {
        if (grassTilemap != null && grassTile != null)
        {
            grassTilemap.ClearAllTiles();

            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    Vector3Int tilePosition = new Vector3Int(x, y, 0);
                    grassTilemap.SetTile(tilePosition, grassTile);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Here you might handle user input, for example to place a tower when the player clicks a grid cell
    }
}





