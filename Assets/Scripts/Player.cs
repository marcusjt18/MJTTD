using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Player : MonoBehaviour
{
    [SerializeField]
    private TMP_Text healthText;
    [SerializeField]
    private TMP_Text goldText;

    public TextAnimations animator;

    //For the gold text animation
    private Coroutine scaleCoroutine;
    private Vector3 originalScale;
    private Coroutine healthShakeCoroutine;
    private Vector3 originalHealthTextPosition;


    [SerializeField]
    private int health;
    [SerializeField]
    private int gold;

    public int Health { get => health; set => health = value; }
    public int Gold { get => gold; set => gold = value; }

    

    public void LoseHealth(int amount)
    {
        health -= amount;
        healthText.text = health.ToString();

        // Stop the currently running ShakeText coroutine if it exists
        if (healthShakeCoroutine != null)
        {
            StopCoroutine(healthShakeCoroutine);
            healthText.transform.localPosition = originalHealthTextPosition;
        }
        // Start a new ShakeText coroutine
        originalHealthTextPosition = healthText.transform.localPosition;
        healthShakeCoroutine = StartCoroutine(animator.ShakeText(healthText, 0.3f, 10f));

        GameManager.Instance.CheckGameOver(health);
    }



    public void GainGold(int amount)
    {
        gold += amount;
        goldText.text = gold.ToString();
        if (scaleCoroutine != null) // if a scaleCoroutine is running, stop it
        {
            StopCoroutine(scaleCoroutine);
            goldText.transform.localScale = originalScale; // reset to original scale
        }
        originalScale = goldText.transform.localScale; // remember the original scale
        scaleCoroutine = StartCoroutine(animator.ScaleTextUpAndDown(goldText, 0.2f));
    }
    public void SpendGold(int amount)
    {
        gold -= amount;
        goldText.text = gold.ToString();
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
        healthText.text = health.ToString();
        goldText.text = gold.ToString();

    }

}
