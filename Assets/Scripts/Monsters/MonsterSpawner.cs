using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{

    [SerializeField]
    private MonsterTiers tiers;

    private List<TileScript> path;

    public List<TileScript> Path { get => path; set => path = value; }

    private void Awake()
    {

    }

    public void SpawnMonster(GameObject monster, List<TileScript> path)
    {

        // Instantiate the monster.
        Monster newMonster = Instantiate(monster, LevelManager.Instance.StartTile.transform.position, Quaternion.identity, transform).GetComponent<Monster>();
        newMonster.Initialize(path);
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

        int monsterCount = 4 + Mathf.RoundToInt(level * 2.4f);
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
                wave.spawnInterval = 0.3f;
                break;
            default:
                wave.spawnInterval = 0.2f;
                break;
        }

        for (int i = 0; i < monsterCount; i++)
        {
            GameObject monster = tiers.Tiers[0][Random.Range(0, tiers.Tiers[0].Count)];
            wave.monstersToSpawn.Add(monster);
        }


        return wave;
    }
}

[System.Serializable]
public class Wave
{
    public List<GameObject> monstersToSpawn = new List<GameObject>();
    public int monsterCount;
    public float spawnInterval;
}




