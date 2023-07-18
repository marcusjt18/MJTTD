using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text healthText;
    [SerializeField]
    private TMP_Text goldText;
    [SerializeField]
    private TMP_Text cannotPlaceTowerText;
    [SerializeField]
    private TMP_Text noFundsText;

    [SerializeField]
    private List<TowerUIWithTag> TowerUIs;

    private Dictionary<string, GameObject> towerUIDict;

    public TextAnimations animator;

    //For the gold text animation
    private Coroutine scaleCoroutine;
    private Vector3 originalScale;
    private Coroutine healthShakeCoroutine;
    private Vector3 originalHealthTextPosition;

    private static UIManager instance;

    // Access the singleton instance of GameManager
    public static UIManager Instance
    {
        get { return instance; }
    }

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
        towerUIDict = new Dictionary<string, GameObject>();
        foreach (TowerUIWithTag ui in TowerUIs)
        {
            towerUIDict.Add(ui.id, ui.towerUI);
        }
    }

    public void UpdateGoldTextWithAnimation(int gold)
    {
        goldText.text = gold.ToString();
        if (scaleCoroutine != null) // if a scaleCoroutine is running, stop it
        {
            StopCoroutine(scaleCoroutine);
            goldText.transform.localScale = originalScale; // reset to original scale
        }
        originalScale = goldText.transform.localScale; // remember the original scale
        scaleCoroutine = StartCoroutine(animator.ScaleTextUpAndDown(goldText, 0.2f));
    }

    public void UpdateHealthTextWithAnimation(int health)
    {
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
    }

    public void UpdateGoldText(int gold)
    {
        goldText.text = gold.ToString();

    }

    public void UpdateHealthText(int health)
    {
        healthText.text = health.ToString();

    }

    public void DisplayCannotPlaceTowerText()
    {
        // Make sure the text is not already displaying.
        if (!cannotPlaceTowerText.gameObject.activeInHierarchy)
        {
            // Display the text.
            cannotPlaceTowerText.gameObject.SetActive(true);

            // Start the coroutine to hide the text after 1 second.
            StartCoroutine(HideTextAfter1Second(cannotPlaceTowerText));
        }
    }

    public void DisplayNoFundsText()
    {
        // Make sure the text is not already displaying.
        if (!noFundsText.gameObject.activeInHierarchy)
        {
            // Display the text.
            noFundsText.gameObject.SetActive(true);

            // Start the coroutine to hide the text after 1 second.
            StartCoroutine(HideTextAfter1Second(noFundsText));
        }
    }

    private IEnumerator HideTextAfter1Second(TMP_Text text)
    {
        // Wait for 1 second.
        yield return new WaitForSeconds(1f);

        // Hide the text.
        text.gameObject.SetActive(false);
    }

    internal void ShowTowerUI(Tower tower, TileScript tile)
    {
        // Check if the tag exists in the dictionary
        if (towerUIDict.ContainsKey(tower.Id))
        {

            // We can deselect a tower if we show a ui, no need for the ghost tower while we look at ui
            GameManager.Instance.DeselectTower();

            TowerUI tui = towerUIDict[tower.Id].GetComponent<TowerUI>();
            tui.SelectTowerForUI(tower);
            tui.SelectTileForUI(tile);
            tui.UpdateInfoText(tower);
            tui.Show();
        }
        else
        {
            // Handle the case when the tag is not found
            Debug.LogWarning("TowerUI with tag " + tower.Id + " not found!");
        }
    }
}


[System.Serializable]
public class TowerUIWithTag
{
    public string id;
    public GameObject towerUI;
}
