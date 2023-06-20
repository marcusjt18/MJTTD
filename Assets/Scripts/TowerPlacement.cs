using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
    public GameObject towerPrefab; // Reference to the tower prefab to be placed

    // Update is called once per frame
    void Update()
    {
        // Check if the player clicked the mouse button
        if (Input.GetMouseButtonDown(0))
        {
            // Convert the mouse position to world position
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Round the mouse position to the nearest cell position
            Vector3Int cellPosition = GridSystem.Instance.tilemap.WorldToCell(mousePosition);

            // Check if the cell position is within the valid grid range
            if (cellPosition.x >= 0 && cellPosition.x < GridSystem.Instance.width &&
                cellPosition.y >= 0 && cellPosition.y < GridSystem.Instance.height)
            {
                // Get the corresponding grid cell
                GridSystem.GridCell cell = GridSystem.Instance.gridArray[cellPosition.x, cellPosition.y];

                // Check if the cell is walkable and doesn't have a tower already
                if (cell.walkable && !cell.isTower)
                {
                    // Instantiate the tower prefab at the cell's world position
                    GameObject newTower = Instantiate(towerPrefab, cell.worldPosition, Quaternion.identity);

                    // Mark the cell as occupied by a tower
                    cell.isTower = true;

                    // You can also store a reference to the tower in the grid cell or perform other tower-related logic here

                    // Optional: Destroy this script to prevent further tower placement
                    Destroy(this);
                }
            }
        }
    }
}


