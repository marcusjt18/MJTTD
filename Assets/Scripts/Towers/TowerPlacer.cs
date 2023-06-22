using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacer : MonoBehaviour
{
    // Method to place a tower at a given position.
    public Tower PlaceTower(Tower towerPrefab, Vector3 position)
    {
        // Instantiate the tower at the given position.
        Tower newTower = Instantiate(towerPrefab, position, Quaternion.identity);

        // You could add additional logic here, like subtracting the cost of the tower from the player's resources.

        return newTower;
    }
}

