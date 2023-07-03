using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemPool : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public static ParticleSystemPool Instance { get; private set; }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    void Awake()
    {
        // Check if there's already an instance of ParticleSystemPool
        if (Instance == null)
        {
            // If not, this instance becomes the singleton instance
            Instance = this;
        }
        else if (Instance != this)
        {
            // If an instance already exists and it's not this one, destroy this one
            Destroy(gameObject);
        }

        // Don't destroy this object when changing scenes

    }

    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        // Check if the pool is empty
        if (poolDictionary[tag].Count == 0)
        {
            // If it is, instantiate a new object, add it to the pool, and return it
            GameObject obj = Instantiate(pools.Find(pool => pool.tag == tag).prefab, transform);
            obj.SetActive(false);
            poolDictionary[tag].Enqueue(obj);
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        return objectToSpawn;
    }


    public GameObject SpawnFromPoolWithReturn(string tag, Vector3 position, Quaternion rotation, float delay)
    {
        GameObject obj = SpawnFromPool(tag, position, rotation);

        // Start the coroutine to return the object to the pool after a delay
        StartCoroutine(ReturnToPoolAfterDelay(tag, obj, delay));

        return obj;
    }


    private IEnumerator ReturnToPoolAfterDelay(string tag, GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
        poolDictionary[tag].Enqueue(obj);
    }

}

