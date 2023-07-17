using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TowerUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text damageText;
    [SerializeField]
    private TMP_Text speedText;
    [SerializeField]
    private TMP_Text rangeText;
    [SerializeField]
    private TMP_Text levelText;

    private Tower currentTower;

    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    internal void UpdateInfoText(Tower tower)
    {
        damageText.text = $"Damage: {tower.MinDamage}-{tower.MaxDamage}";
        speedText.text = $"Attack cooldown: {tower.AttackSpeed}";
        rangeText.text = $"Range: {tower.Range}";
        levelText.text = $"Level: {tower.Level}/{tower.MaxLevel}";
    }

    public void SelectTowerForUI(Tower tower)
    {
        currentTower = tower;
    }

    public void LevelUpTower()
    {
        if (currentTower)
        {

        }
        else
        {
            Debug.Log("No tower is currently selected in the UI.");
        }
    }
}
