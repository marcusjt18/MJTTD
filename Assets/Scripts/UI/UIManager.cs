using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text healthText;
    [SerializeField]
    private TMP_Text goldText;
    [SerializeField]
    private TMP_Text cannotPlaceTowerText;

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
            StartCoroutine(HideCannotPlaceTowerTextAfter1Second());
        }
    }

    private IEnumerator HideCannotPlaceTowerTextAfter1Second()
    {
        // Wait for 1 second.
        yield return new WaitForSeconds(1f);

        // Hide the text.
        cannotPlaceTowerText.gameObject.SetActive(false);
    }



}
