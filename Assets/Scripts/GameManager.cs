using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    // Access the singleton instance of GameManager
    public static GameManager Instance
    {
        get { return instance; }
    }

    private LevelManager levelManager;

    [SerializeField]
    private TowerPlacer towerPlacer;

    [SerializeField]
    private MonsterSpawner monsterSpawner;

    private List<TileScript> path;

    public List<Wave> waves;
    private int currentWaveIndex = 0;
    public TowerPlacer TowerPlacer { get => towerPlacer; }
    public MonsterSpawner MonsterSpawner { get => monsterSpawner; set => monsterSpawner = value; }
    public bool WaveOngoing { get => waveOngoing; set => waveOngoing = value; }
    public int MonsterCounter { get => monsterCounter; set => monsterCounter = value; }
    public List<TileScript> Path { get => path; set => path = value; }

    private bool waveOngoing = false;

    private int monsterCounter = 0;


    // This method is called before the first frame update
    void Awake()
    {
        if (instance != null && instance != this)
        {
            // If an instance already exists, destroy this GameManager
            Destroy(this.gameObject);
        }
        else
        {
            // Set the instance and persist it between scenes
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        levelManager = LevelManager.Instance;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!waveOngoing)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                monsterSpawner.Path = levelManager.PathFinder.FindPath(levelManager.StartTile, levelManager.EndTile);
                // Check if there are more waves to spawn
                if (currentWaveIndex < waves.Count)
                {
                    waveOngoing = true;
                    StartCoroutine(monsterSpawner.SpawnWave(waves[currentWaveIndex]));
                    currentWaveIndex++;
                }
                else
                {
                    Debug.Log("All waves spawned!");
                }
            }
        }

    }
}
