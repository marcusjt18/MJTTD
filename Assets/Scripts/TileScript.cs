using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    private bool isWalkable = true;
    private bool hasTower = false;
    private int x;
    private int y;
    private LevelManager levelManager;

    public bool IsWalkable { get => isWalkable; set => isWalkable = value; }
    public bool HasTower { get => hasTower; set => hasTower = value; }
    public int X { get => x; set => x = value; }
    public int Y { get => y; set => y = value; }

    public int gCost; // cost from the start node to this node
    public int hCost; // heuristic cost from this node to the end node
    public TileScript parent; // parent node of this node in the path

    private void Awake()
    {
        levelManager = LevelManager.Instance;
    }

    public int FCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    // Reference to the overlay prefab.
    public GameObject overlayPrefab;

    // Reference to the current overlay object.
    private GameObject overlay;

    public void ShowOverlay()
    {
        if (overlay == null)
        {
            overlay = Instantiate(overlayPrefab, transform.position + new Vector3(0, 0, -1), Quaternion.identity, transform);
        }
    }


    public void HideOverlay()
    {
        // Destroy the overlay object.
        if (overlay != null)
        {
            Destroy(overlay);
            overlay = null;
        }
    }


    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && !GameManager.Instance.WaveOngoing)
        {
            if (!HasTower && IsWalkable)
            {
                isWalkable = false;
                if (levelManager.PathFinder.FindPath(levelManager.StartTile, levelManager.EndTile) == null)
                {
                    isWalkable = true;
                    Debug.Log("This would hinder the path completely. ILLEGAL!!!");
                }
                else
                {
                    string currentTower = GameManager.Instance.TowerPlacer.CurrentTowerId;
                    Vector3 tilePosition = transform.position;
                    Vector3 towerPosition = new Vector3(tilePosition.x, tilePosition.y + GameManager.Instance.TowerPlacer.TowerDict[currentTower].transform.localScale.y / 2, tilePosition.z);

                    // Call the PlaceTower method of the TowerPlacer.
                    GameManager.Instance.TowerPlacer.PlaceTower(towerPosition);
                    HasTower = true;
                }
            
            }
        }
    }


}
