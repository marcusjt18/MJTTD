using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    private bool isWalkable;
    private bool hasTower;

    public bool IsWalkable { get => isWalkable; set => isWalkable = value; }
    public bool HasTower { get => hasTower; set => hasTower = value; }
}
