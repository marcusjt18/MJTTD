using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TowerPlacement : MonoBehaviour
{
    // Reference to the tower prefab
    public GameObject towerPrefab;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // On left mouse button click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GridSystem.GridCell cell = GetCellFromWorldPos(hit.point);
                if (cell != null && cell.walkable && !cell.isTower)
                {
                    // Instantiate a tower on the cell
                    Instantiate(towerPrefab, cell.worldPosition, Quaternion.identity);
                    cell.isTower = true; // There is now a tower on this cell
                }
            }
        }
    }

    // Helper function to get grid cell from world position
    GridSystem.GridCell GetCellFromWorldPos(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt(worldPosition.x / GridSystem.Instance.cellSize);
        int y = Mathf.FloorToInt(worldPosition.y / GridSystem.Instance.cellSize);
        if (x >= 0 && y >= 0 && x < GridSystem.Instance.width && y < GridSystem.Instance.height)
        {
            return GridSystem.Instance.gridArray[x, y];
        }
        return null; // If position is outside of grid bounds, return null
    }
}



