using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacer : MonoBehaviour
{
    [SerializeField]
    private List<Tower> towerPrefabs;

    [SerializeField]
    private GameObject ghostTower;

    // A dictionary to store the tower prefabs with their identifier strings.
    private Dictionary<string, Tower> towerDict = new Dictionary<string, Tower>();

    // The identifier of the currently selected tower.
    private string currentTowerId = "basic";
    public string CurrentTowerId { get => currentTowerId; set => currentTowerId = value; }
    public Dictionary<string, Tower> TowerDict { get => towerDict; set => towerDict = value; }
    public GameObject GhostTower { get => ghostTower; set => ghostTower = value; }

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

    public Tower PlaceTower(Vector3 position)
    {
        // Check if a tower is selected.
        if (CurrentTowerId != null)
        {
            // Get the current tower prefab from the dictionary.
            Tower towerPrefab = TowerDict[CurrentTowerId];

            // Instantiate the prefab associated with the tower at the given position.
            Tower newTower = Instantiate(towerPrefab.Prefab, position, Quaternion.identity, transform).GetComponent<Tower>();
            newTower.gameObject.AddComponent<DepthSorter>();

            Player.Instance.SpendGold(newTower.Cost);

            return newTower;
        }
        else
        {
            Debug.LogError("No tower selected!");
        }
        return null;
    }

    public void SelectTower(string id)
    {
        // Check if a tower with the given id exists.
        if (TowerDict.ContainsKey(id))
        {
            // Select the tower.
            currentTowerId = id;

            SetGhostTowerSprite(TowerDict[id]);
        }
        else
        {
            Debug.LogError("No tower with id " + id + " found!");
        }
    }

    public void DeselectTower()
    {
        currentTowerId = null;

        if (GhostTower != null)
        {
            GhostTower.SetActive(false);
        }
    }

    private void SetGhostTowerSprite(Tower tower)
    {
        SpriteRenderer ghostTowerRenderer = GhostTower.GetComponent<SpriteRenderer>();
        SpriteRenderer towerRenderer = tower.Prefab.GetComponent<SpriteRenderer>();

        if (ghostTowerRenderer != null && towerRenderer != null)
        {
            ghostTowerRenderer.sprite = towerRenderer.sprite;
        }
        else
        {
            Debug.LogError("Ghost Tower or Selected Tower has no SpriteRenderer attached.");
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            DeselectTower();
        }
    }

    private void Start()
    {
        GhostTower.SetActive(false);
    }
}


