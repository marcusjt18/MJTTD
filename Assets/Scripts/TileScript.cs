using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    private bool isWalkable = true;
    private bool hasTower = false;
    private int x;
    private int y;
    private Vector3 worldPosition;

    public bool IsWalkable { get => isWalkable; set => isWalkable = value; }
    public bool HasTower { get => hasTower; set => hasTower = value; }
    public int X { get => x; set => x = value; }
    public int Y { get => y; set => y = value; }
    public Vector3 WorldPosition { get => worldPosition; set => worldPosition = value; }

    public int gCost; // cost from the start node to this node
    public int hCost; // heuristic cost from this node to the end node
    public TileScript parent; // parent node of this node in the path

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

    public void ShowOverlay(Color color)
    {
        if (overlay == null)
        {
            overlay = Instantiate(overlayPrefab, transform.position + new Vector3(0, 0, -1), Quaternion.identity, transform);
        }

        SpriteRenderer overlayRenderer = overlay.GetComponent<SpriteRenderer>();
        overlayRenderer.color = color;
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
        if (Input.GetMouseButtonDown(0))
        {
            if (!hasTower && isWalkable)
            {
                Vector3 tilePosition = transform.position;
                Vector3 towerPosition = new Vector3(tilePosition.x, tilePosition.y + GameManager.Instance.TowerPrefab.transform.localScale.y / 2, tilePosition.z);
                Debug.Log(x + " " + y);
                Instantiate(GameManager.Instance.TowerPrefab, towerPosition, Quaternion.identity);
                hasTower = true;
                isWalkable = false;
            }
        }
    }

}
