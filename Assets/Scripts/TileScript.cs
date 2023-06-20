using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    private bool isWalkable = true;
    private bool hasTower = false;
    private int x;
    private int y;

    public bool IsWalkable { get => isWalkable; set => isWalkable = value; }
    public bool HasTower { get => hasTower; set => hasTower = value; }
    public int X { get => x; set => x = value; }
    public int Y { get => y; set => y = value; }

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
