using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridCellBehaviour : MonoBehaviour
{
    public GridSystem.GridCell GridCell { get; set; }

    public GameObject towerPrefab;

    private void OnMouseDown()
    {
        if (GridCell.walkable && !GridCell.isTower)
        {
            Instantiate(towerPrefab, transform.position, Quaternion.identity);
            GridCell.isTower = true;
        }
    }
}

