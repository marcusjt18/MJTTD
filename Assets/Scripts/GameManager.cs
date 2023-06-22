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

    [SerializeField]
    private TowerPlacer towerPlacer;
    public TowerPlacer TowerPlacer { get => towerPlacer; }

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
    }

    // Update is called once per frame
    void Update()
    {

    }
}
