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

    [SerializeField]
    public List<GameObject> Talents = new List<GameObject>();

    private Tower currentTower;
    private TileScript currentTile;

    public Tower CurrentTower { get => currentTower; set => currentTower = value; }

    public void Show()
    {
        gameObject.SetActive(true);

        UpdateTalents();
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    internal void UpdateInfoText(Tower tower)
    {
        damageText.text = $"Damage: {tower.ApplyMultiplierToDamage(tower.MinDamage)}-{tower.ApplyMultiplierToDamage(tower.MaxDamage)}";
        speedText.text = $"Attack cooldown: {tower.AttackSpeed}";
        rangeText.text = $"Range: {tower.Range}";
        levelText.text = $"Level: {tower.Level}/{tower.MaxLevel}";
        upgradePriceText.text = $"Upgrade price: {tower.UpgradeCost}";
        sellPriceText.text = $"Sell price: {tower.SellPrice}";
        talentPointsText.text = $"Talent points: {tower.TalentPoints}";
    }

    private void UpdateTalents()
    {

        foreach (GameObject talentObject in Talents)
        {
            Talent talent = talentObject.GetComponent<Talent>();

            if (currentTower.TalentIds.Contains(talent.Id))
            {
                talent.IsActivated = true;
            }
            else
            {
                talent.IsActivated = false;
            }
            
            talent.UpdateColor();
        }
    }

    public void SelectTowerForUI(Tower tower)
    {
        CurrentTower = tower;
    }

    public void SelectTileForUI(TileScript tile)
    {
        currentTile = tile;
    }

    public void LevelUpTower()
    {
        if (CurrentTower)
        {
            CurrentTower.LevelUp();
            UpdateInfoText(CurrentTower);
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

            CurrentTower.SellTower();

            CurrentTower = null;
            currentTile = null;

            Hide();

        }
        else
        {
            Debug.Log("No tile is currently selected in the UI.");
        }

    }
}
