using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    // Access the singleton instance of GameManager
    public static GameManager Instance
    {
        get { return instance; }
    }

    [SerializeField]
    private GameObject gameOverPanel;

    [SerializeField]
    private TMP_Text resultText;

    private LevelManager levelManager;

    [SerializeField]
    private TowerPlacer towerPlacer;

    [SerializeField]
    private MonsterSpawner monsterSpawner;

    private List<TileScript> path;

    private int currentWaveIndex = 0;
    public TowerPlacer TowerPlacer { get => towerPlacer; }
    public MonsterSpawner MonsterSpawner { get => monsterSpawner; set => monsterSpawner = value; }
    public bool WaveOngoing { get => waveOngoing; set => waveOngoing = value; }
    public int MonsterCounter { get => monsterCounter; set => monsterCounter = value; }
    public List<TileScript> Path { get => path; set => path = value; }
    public int CurrentWaveIndex { get => currentWaveIndex; set => currentWaveIndex = value; }
    public GameObject GameOverPanel { get => gameOverPanel; set => gameOverPanel = value; }

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
            instance = this;
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
                waveOngoing = true;
                StartCoroutine(monsterSpawner.SpawnWave(CurrentWaveIndex));
                CurrentWaveIndex++;
                monsterSpawner.SetNextWave();

            }
        }

        if (monsterCounter <= 0)
        {
            waveOngoing = false;
        }

    }

    public void CheckGameOver(int health)
    {
        if (health <= 0)
        {
            resultText.text = (currentWaveIndex - 1).ToString();
            GameOverPanel.SetActive(true);

            Time.timeScale = 0;
        }
    }

    public void Replay()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;

    }


}
