using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Player : MonoBehaviour
{
    [SerializeField]
    private int health;
    [SerializeField]
    private int gold;

    public int Health { get => health; set => health = value; }
    public int Gold { get => gold; set => gold = value; }

    

    public void LoseHealth(int amount)
    {
        health -= amount;
        UIManager.Instance.UpdateHealthTextWithAnimation(health);
        GameManager.Instance.CheckGameOver(health);
    }



    public void GainGold(int amount)
    {
        gold += amount;
        UIManager.Instance.UpdateGoldTextWithAnimation(gold);
    }
    public void SpendGold(int amount)
    {
        gold -= amount;
        UIManager.Instance.UpdateGoldText(gold);
    }


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
        }

    }


    private void Start()
    {
        UIManager.Instance.UpdateHealthText(health);
        UIManager.Instance.UpdateGoldText(gold);

    }

}
