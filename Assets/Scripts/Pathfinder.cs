using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    private LevelManager levelManager;

    void Start()
    {
        levelManager = LevelManager.Instance;

    }

    // Colors for different types of nodes.
    private Color startColor = Color.green;
    private Color endColor = Color.red;
    private Color openSetColor = Color.blue;
    private Color closedSetColor = Color.gray;
    private Color pathColor = Color.yellow;

    public List<TileScript> FindPath(TileScript startTile, TileScript endTile)
    {
        startTile.ShowOverlay(startColor);
        endTile.ShowOverlay(endColor);

        List<TileScript> openSet = new List<TileScript>();
        HashSet<TileScript> closedSet = new HashSet<TileScript>();
        openSet.Add(startTile);

        while (openSet.Count > 0)
        {
            TileScript currentTile = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost < currentTile.FCost || openSet[i].FCost == currentTile.FCost && openSet[i].hCost < currentTile.hCost)
                {
                    currentTile = openSet[i];
                }
            }

            openSet.Remove(currentTile);
            closedSet.Add(currentTile);

            // Show the overlay on the nodes in the open set.
            foreach (TileScript tile in openSet)
            {
                tile.ShowOverlay(openSetColor);
            }

            // Show the overlay on the nodes in the closed set.
            foreach (TileScript tile in closedSet)
            {
                tile.ShowOverlay(closedSetColor);
            }

            if (currentTile == endTile)
            {
                // Get the final path.
                List<TileScript> path = RetracePath(startTile, endTile);

                // Color the final path.
                foreach (TileScript tile in path)
                {
                    tile.ShowOverlay(pathColor);
                }
                // Return the final path.
                return path;
            }

            foreach (TileScript neighbour in levelManager.GetNeighbours(currentTile))
            {
                if (!neighbour.IsWalkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newMovementCostToNeighbour = currentTile.gCost + GetDistance(currentTile, neighbour);
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, endTile);
                    neighbour.parent = currentTile;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }

        return null;
    }

    List<TileScript> RetracePath(TileScript startTile, TileScript endTile)
    {
        List<TileScript> path = new List<TileScript>();
        TileScript currentTile = endTile;

        while (currentTile != startTile)
        {
            path.Add(currentTile);
            currentTile = currentTile.parent;
        }
        path.Reverse();

        return path;
    }

    int GetDistance(TileScript tileA, TileScript tileB)
    {
        int distX = Mathf.Abs(tileA.X - tileB.X);
        int distY = Mathf.Abs(tileA.Y - tileB.Y);

        if (distX > distY)
        {
            return 14 * distY + 10 * (distX - distY);
        }

        return 14 * distX + 10 * (distY - distX);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            TileScript startTile = levelManager.GetTile(0, 0);
            TileScript endTile = levelManager.GetTile(14, 9);
            List<TileScript> path = FindPath(startTile, endTile);
            foreach (TileScript tile in path)
            {
                Debug.Log($"Tile at position: {tile.transform.position}");
            }
        }
    }
}

