using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class MonsterSpawner : MonoBehaviour
{

    [SerializeField]
    private List<MonsterTier> tiers;

    private List<TileScript> path;

    private GameManager gameManager;

    private MonsterPool monsterPool;

    private List<int> lotteryBowl;

    private Wave nextWave;

    public List<TileScript> Path { get => path; set => path = value; }
    public Wave NextWave { get => nextWave; set => nextWave = value; }

    private void Awake()
    {
        monsterPool = MonsterPool.Instance;
        gameManager = GameManager.Instance;
        lotteryBowl = new List<int>();
    }

    private void Start()
    {
        SetNextWave();
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

        for (int i = 0; i < nextWave.monstersToSpawn.Count; i++)
        {

            SpawnMonster(nextWave.monstersToSpawn[i], path);

            yield return new WaitForSeconds(nextWave.spawnInterval);
        }
    }

    public void SetNextWave()
    {
        PopulateBowl();
        nextWave = GenerateWave(gameManager.CurrentWaveIndex);
    }

    public Wave GenerateWave(int level)
    {
        Wave wave = new Wave();

        int monsterCount = 2 + Mathf.RoundToInt(level * 1.3f);
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

        if (lotteryBowl.Count == 0)
        {
            Debug.LogError("No eligible tiers found for level " + level);
            return wave;
        }

        for (int i = 0; i < monsterCount; i++)
        {
            // Pick a random index from the lottery bowl
            int tierIndex = lotteryBowl[Random.Range(0, lotteryBowl.Count)];
            string monster = tiers[tierIndex].monsters[Random.Range(0, tiers[tierIndex].monsters.Count)];
            wave.monstersToSpawn.Add(monster);
        }

        return wave;
    }


    #region Populate Bowl
    private int activeNonEndTiers = 0;
    public void PopulateBowl()
    {
        if (gameManager.CurrentWaveIndex >= tiers[tiers.Count - 1].startLevel && activeNonEndTiers <= 0)
        {
            return;
        }

        for (int i = 0; i < tiers.Count; i++)
        {
            if (tiers[i].startLevel <= gameManager.CurrentWaveIndex && gameManager.CurrentWaveIndex < tiers[i].stopAddingLevel && tiers[i].endLevel > gameManager.CurrentWaveIndex)
            {
                lotteryBowl.Add(i);
                if (i != (tiers.Count-1))
                {
                    activeNonEndTiers++;
                }
            }
            else if (tiers[i].endLevel <= gameManager.CurrentWaveIndex)
            {
                // if it contains the index, remove one instance of that index
                if (lotteryBowl.Contains(i))
                {
                    lotteryBowl.Remove(i);
                    activeNonEndTiers--;
                }

            }
        }
        //Debug.Log("ANET: " + activeNonEndTiers);
        //Debug.Log("Bowl: " + string.Join(", ", lotteryBowl));
    }
    #endregion

}

[System.Serializable]
public class Wave
{
    public List<string> monstersToSpawn = new List<string>();
    public int monsterCount;
    public float spawnInterval;
}

[System.Serializable]
public class MonsterTier
{
    public List<string> monsters;
    public int startLevel;
    public int stopAddingLevel;
    public int endLevel;
}




