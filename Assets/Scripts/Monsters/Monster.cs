using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : MonoBehaviour
{
    [SerializeField]
    private float speed = 1.0f;

    [SerializeField]
    private int health = 5;

    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private string id;

    [SerializeField]
    private int minWave = 1;

    [SerializeField]
    private int maxWave = int.MaxValue;


    protected List<TileScript> path;
    protected int currentTileIndex;

    public float Speed { get => speed; set => speed = value; }
    public int Health { get => health; set => health = value; }
    public GameObject Prefab { get => prefab; set => prefab = value; }
    public string Id { get => id; set => id = value; }
    public int MinWave { get => minWave; set => minWave = value; }
    public int MaxWave { get => maxWave; set => maxWave = value; }

    public virtual void Initialize(List<TileScript> path)
    {
        this.path = path;
        transform.position = LevelManager.Instance.StartTile.transform.position;
        currentTileIndex = 0;
        this.gameObject.AddComponent<DepthSorter>();
    }

    protected virtual void Update()
    {
        if (currentTileIndex < path.Count)
        {
            transform.position = Vector3.MoveTowards(transform.position, path[currentTileIndex].transform.position, Speed * Time.deltaTime);

            if (transform.position == path[currentTileIndex].transform.position)
            {
                currentTileIndex++;

                // If the monster has moved to the last tile, destroy the monster
                if (currentTileIndex >= path.Count)
                {
                    // Insert any other logic you want to occur when a monster reaches the end here
                    // For example, you might want to decrement the player's life total

                    Die(true);
                }
            }
        }

        GetComponent<DepthSorter>().UpdateOrder();
    }

    private void Die(bool reachedEnd) {
        Destroy(this.gameObject);
        GameManager.Instance.MonsterCounter--;
        if (reachedEnd)
        {
            // lose life
        }
        // Check if all monsters are dead
        if (GameManager.Instance.MonsterCounter == 0)
        {
            GameManager.Instance.WaveOngoing = false;
        }
    }

    // Other common functionality, such as taking damage, dying, etc. can be defined here as well.
}

