using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    private bool isWalkable = true;
    private int x;
    private int y;
    private LevelManager levelManager;

    private SpriteRenderer spriteRenderer;

    public bool IsWalkable { get => isWalkable; set => isWalkable = value; }
    public int X { get => x; set => x = value; }
    public int Y { get => y; set => y = value; }

    public Tower Tower { get; set; }


    public int gCost; // cost from the start node to this node
    public int hCost; // heuristic cost from this node to the end node
    public TileScript parent; // parent node of this node in the path

    private void Awake()
    {
        levelManager = LevelManager.Instance;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public int FCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public SpriteRenderer SpriteRenderer { get => spriteRenderer; set => spriteRenderer = value; }

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

    private void OnMouseEnter()
    {
        if (!GameManager.Instance.WaveOngoing && GameManager.Instance.TowerPlacer.CurrentTowerId != null && GameManager.Instance.TowerPlacer.GhostTower != null)
        {
            Vector3 tilePosition = transform.position;
            Vector3 ghostTowerPosition = new Vector3(tilePosition.x, tilePosition.y + GameManager.Instance.TowerPlacer.GhostTower.transform.localScale.y / 2, tilePosition.z);
            GameManager.Instance.TowerPlacer.GhostTower.transform.position = ghostTowerPosition;
            GameManager.Instance.TowerPlacer.GhostTower.SetActive(true);
        }
    }

    private void OnMouseExit()
    {
        if (GameManager.Instance.TowerPlacer.GhostTower != null)
        {
            GameManager.Instance.TowerPlacer.GhostTower.SetActive(false);
        }
    }

    private void OnMouseOver()
    {
        if (GameManager.Instance.TowerPlacer.GhostTower != null)
        {
            SpriteRenderer ghostTowerRenderer = GameManager.Instance.TowerPlacer.GhostTower.GetComponent<SpriteRenderer>();
            float currentAlpha = ghostTowerRenderer.color.a; // Store the current alpha value

            // Check if the tile is walkable and doesn't already have a tower
            if (IsWalkable && Tower == null)
            {
                ghostTowerRenderer.color = new Color(0, 1, 0, currentAlpha); // Green for placeable, while preserving alpha
            }
            else
            {
                ghostTowerRenderer.color = new Color(1, 0, 0, currentAlpha); // Red for not placeable, while preserving alpha
            }

            Vector3 tilePosition = transform.position;
            Vector3 ghostTowerPosition = new Vector3(tilePosition.x, tilePosition.y + GameManager.Instance.TowerPlacer.GhostTower.transform.localScale.y / 2, tilePosition.z);
            GameManager.Instance.TowerPlacer.GhostTower.transform.position = ghostTowerPosition;
        }

        if (Input.GetMouseButtonUp(0) && !GameManager.Instance.WaveOngoing)
        {
            if (Tower)
            {
                UIManager.Instance.ShowTowerUI(Tower, this);
            }
            else
            {
                PlaceTowerOnTile();
            }
        }

    }




    private void PlaceTowerOnTile()
    {
        if (GameManager.Instance.TowerPlacer.CurrentTowerId == null)
        {
            return;
        }

        if (!Tower && IsWalkable)
        {
            isWalkable = false;
            if (levelManager.PathFinder.FindPath(levelManager.StartTile, levelManager.EndTile) == null)
            {
                isWalkable = true;
                UIManager.Instance.DisplayCannotPlaceTowerText();
            }
            else
            {
                string currentTower = GameManager.Instance.TowerPlacer.CurrentTowerId;
                Vector3 tilePosition = transform.position;
                Vector3 towerPosition = new Vector3(tilePosition.x, tilePosition.y + GameManager.Instance.TowerPlacer.TowerDict[currentTower].transform.localScale.y / 2, tilePosition.z);

                // Call the PlaceTower method of the TowerPlacer.
                Tower tower = GameManager.Instance.TowerPlacer.PlaceTower(towerPosition);
                Tower = tower;
                GameManager.Instance.TowerPlacer.GhostTower.SetActive(false);
            }

        }
    }


}
