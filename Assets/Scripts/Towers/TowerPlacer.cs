using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacer : MonoBehaviour
{
    [SerializeField]
    private List<Tower> towerPrefabs;

    // A dictionary to store the tower prefabs with their identifier strings.
    private Dictionary<string, Tower> towerDict = new Dictionary<string, Tower>();

    // The identifier of the currently selected tower.
    private string currentTowerId = "basic";
    public string CurrentTowerId { get => currentTowerId; set => currentTowerId = value; }
    public Dictionary<string, Tower> TowerDict { get => towerDict; set => towerDict = value; }

    private void Awake()
    {
        // Fill the dictionary with towers from the towerPrefabs list.
        foreach (Tower tower in towerPrefabs)
        {
            TowerDict.Add(tower.Id, tower);
        }

        // Select the first tower by default.
        if (towerPrefabs.Count > 0)
        {
            CurrentTowerId = towerPrefabs[0].Id;
        }
    }

    public void PlaceTower(Vector3 position)
    {
        // Check if a tower is selected.
        if (CurrentTowerId != null)
        {
            // Get the current tower prefab from the dictionary.
            Tower towerPrefab = TowerDict[CurrentTowerId];

            // Instantiate the tower at the given position.
            Tower newTower = Instantiate(towerPrefab, position, Quaternion.identity);
            newTower.gameObject.AddComponent<DepthSorter>();

            // You could add additional logic here, like subtracting the cost of the tower from the player's resources.
        }
        else
        {
            Debug.LogError("No tower selected!");
        }
    }

    public void SelectTower(string id)
    {
        // Check if a tower with the given id exists.
        if (TowerDict.ContainsKey(id))
        {
            // Select the tower.
            currentTowerId = id;
        }
        else
        {
            Debug.LogError("No tower with id " + id + " found!");
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            SelectTower("wolf");
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            SelectTower("basic");
        }
    }


}

