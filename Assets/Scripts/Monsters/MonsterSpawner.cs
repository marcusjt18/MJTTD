using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class MonsterSpawner : MonoBehaviour
{

    [SerializeField]
    private MonsterTiers tiers;

    private List<TileScript> path;

    private MonsterPool monsterPool;

    public List<TileScript> Path { get => path; set => path = value; }

    private void Awake()
    {
        monsterPool = MonsterPool.Instance;
    }

    public void SpawnMonster(string tag, List<TileScript> path)
    {

        // Instantiate the monster.
        Monster monster = monsterPool.SpawnFromPool(tag, LevelManager.Instance.StartTile.transform.position, Quaternion.identity).GetComponent<Monster>();
        monster.Initialize(path);
        GameManager.Instance.MonsterCounter++;
    }

    public IEnumerator SpawnWave(int level)
    {
        Wave wave = GenerateWave(level);

        for (int i = 0; i < wave.monstersToSpawn.Count; i++)
        {

            SpawnMonster(wave.monstersToSpawn[i], path);

            yield return new WaitForSeconds(wave.spawnInterval);
        }
    }

    public Wave GenerateWave(int level)
    {
        Wave wave = new Wave();

        int monsterCount = 2 + Mathf.RoundToInt(level * 1.4f);
        wave.monsterCount = monsterCount;
        switch (level)
        {
            case < 5:
                wave.spawnInterval = 0.7f;
                break;
            case < 10:
                wave.spawnInterval = 0.5f;
                break;
            case < 15:
                wave.spawnInterval = 0.4f;
                break;
            default:
                wave.spawnInterval = 0.3f;
                break;
        }

        List<MonsterTiers.MonsterTier> eligibleTiers = tiers.Tiers.Where(tier => level >= tier.minLevel && level <= tier.maxLevel).ToList();

        if (eligibleTiers.Count == 0)
        {
            Debug.LogError("No eligible tiers found for level " + level);
            return wave;
        }

        // Calculate the total weight
        int totalWeight = eligibleTiers.Sum(tier => level - tier.minLevel + 1); // Add 1 to ensure there's at least a minimum weight

        for (int i = 0; i < monsterCount; i++)
        {
            // Pick a random number within the total weight
            int randomNumber = Random.Range(0, totalWeight);

            MonsterTiers.MonsterTier chosenTier = null;

            // Find which tier the random number falls into
            foreach (var tier in eligibleTiers)
            {
                int weight = level - tier.minLevel + 1;
                if (randomNumber < weight)
                {
                    chosenTier = tier;
                    break;
                }

                randomNumber -= weight;
            }

            string monster = chosenTier.monsters[Random.Range(0, chosenTier.monsters.Count)];
            wave.monstersToSpawn.Add(monster);
        }

        return wave;
    }


}

[System.Serializable]
public class Wave
{
    public List<string> monstersToSpawn = new List<string>();
    public int monsterCount;
    public float spawnInterval;
}




