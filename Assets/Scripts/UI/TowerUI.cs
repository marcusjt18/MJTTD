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
    [SerializeField]
    private TMP_Text upgradePriceText;
    [SerializeField]
    private TMP_Text sellPriceText;
    [SerializeField]
    private TMP_Text talentPointsText;

    private Tower currentTower;
    private TileScript currentTile;

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
        upgradePriceText.text = $"Upgrade price: {tower.UpgradeCost}";
        sellPriceText.text = $"Sell price: {tower.SellPrice}";
        talentPointsText.text = $"Talent points: {tower.TalentPoints}";
    }

    public void SelectTowerForUI(Tower tower)
    {
        currentTower = tower;
    }

    public void SelectTileForUI(TileScript tile)
    {
        currentTile = tile;
    }

    public void LevelUpTower()
    {
        if (currentTower)
        {
            currentTower.LevelUp();
            UpdateInfoText(currentTower);
        }
        else
        {
            Debug.Log("No tower is currently selected in the UI.");
        }
    }

    public void SellTower()
    {
        if (currentTile)
        {
            currentTile.Tower = null;
            currentTile.IsWalkable = true;

            currentTower.SellTower();

            currentTower = null;
            currentTile = null;

            Hide();

        }
        else
        {
            Debug.Log("No tile is currently selected in the UI.");
        }

    }
}
