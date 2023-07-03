using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField]
    private int health;
    [SerializeField]
    private int gold;

    public int Health { get => health; set => health = value; }
    public int Gold { get => gold; set => gold = value; }


    #region Singleton
    private static Player instance;

    // Access the singleton instance of GameManager
    public static Player Instance
    {
        get { return instance; }
    }


    #endregion

    private void Awake()
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



}
