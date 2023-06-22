using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField]
    private List<Monster> monsterPrefabs;

    // A dictionary to store the monster prefabs with their identifier strings.
    private Dictionary<string, Monster> monsterDict = new Dictionary<string, Monster>();

    private List<TileScript> path;

    public List<TileScript> Path { get => path; set => path = value; }

    private void Awake()
    {
        // Fill the dictionary with monsters from the monsterPrefabs list.
        foreach (Monster monster in monsterPrefabs)
        {
            monsterDict.Add(monster.Id, monster);
        }
    }

    public void SpawnMonster(string id, List<TileScript> path)
    {
        // Check if a monster with the given id exists.
        if (monsterDict.ContainsKey(id))
        {
            // Get the monster prefab from the dictionary.
            Monster monsterPrefab = monsterDict[id];

            // Instantiate the monster.
            Monster newMonster = Instantiate(monsterPrefab.Prefab, LevelManager.Instance.StartTile.transform.position, Quaternion.identity).GetComponent<Monster>();
            newMonster.Initialize(path);
            GameManager.Instance.MonsterCounter++;
        }
        else
        {
            Debug.LogError("No monster with id " + id + " found!");
        }
    }

    public IEnumerator SpawnWave(Wave wave)
    {
        for (int i = 0; i < wave.monsterCount; i++)
        {
            // Compute the total weight.
            int totalWeight = 0;
            foreach (Wave.WaveEntry entry in wave.potentialMonsters)
            {
                totalWeight += entry.weight;
            }

            // Choose a random value between 0 and the total weight.
            int choice = Random.Range(0, totalWeight);

            // Find which monster this corresponds to.
            string chosenMonsterId = null;
            foreach (Wave.WaveEntry entry in wave.potentialMonsters)
            {
                if (choice < entry.weight)
                {
                    chosenMonsterId = entry.monsterId;
                    break;
                }
                choice -= entry.weight;
            }

            // Spawn the chosen monster.
            SpawnMonster(chosenMonsterId, path);

            yield return new WaitForSeconds(wave.spawnInterval);
        }
    }
}


[System.Serializable]
public class Wave
{
    [System.Serializable]
    public struct WaveEntry
    {
        public string monsterId;
        public int weight;
    }

    public List<WaveEntry> potentialMonsters;
    public int monsterCount;
    public float spawnInterval;
}


