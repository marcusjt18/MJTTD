using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    private LineDrawer lineDrawer;

    private List<Talent> Talents = new List<Talent>();

    private Tower currentTower;
    private TileScript currentTile;
    [SerializeField]
    private TalentTooltip talentTooltip;

    public Tower CurrentTower { get => currentTower; set => currentTower = value; }
    public TalentTooltip TalentTooltip { get => talentTooltip; set => talentTooltip = value; }

    private void Awake()
    {
        lineDrawer = GetComponentInChildren<LineDrawer>();
        AddTalents();
    }

    public void Show()
    {

        UpdateTalents();
        UpdateTalentLines();
        UpdateTalentColors();
        gameObject.SetActive(true);
        //initTalentRectTransforms();
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
        if(tower.Level >= tower.MaxLevel)
        {
            upgradePriceText.text = $"Upgrade price: --";
        }
        else
        {
            upgradePriceText.text = $"Upgrade price: {tower.UpgradeCost}";
        }
        sellPriceText.text = $"Sell price: {tower.SellPrice}";
        talentPointsText.text = $"Talent points: {tower.TalentPoints}";
    }

    private void UpdateTalents()
    {
        foreach (Talent talent in Talents)
        {
            if (currentTower.TalentIds.Contains(talent.Id))
            {
                talent.IsActivated = true;
            }
            else
            {
                talent.IsActivated = false;
            }
            
        }
    }

    public void AddTalents()
    {
        Talent[] talents = GetComponentsInChildren<Talent>(true);
        foreach (Talent t in talents)
        {
            Talents.Add(t);
            t.Initialize(lineDrawer);
            foreach (GameObject dependency in t.Dependencies)
            {
                lineDrawer.DrawLine(t.GetComponent<RectTransform>(), dependency.GetComponent<RectTransform>(), t.IsActivated && dependency.GetComponent<Talent>().IsActivated);
            }
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
    public void UpdateTalentLines()
    {
        foreach (Talent talent in Talents)
        {
            foreach (GameObject dependency in talent.Dependencies)
            {
                lineDrawer.UpdateLineThickness(talent.GetComponent<RectTransform>(), dependency.GetComponent<RectTransform>(), talent.IsActivated && dependency.GetComponent<Talent>().IsActivated);
            }
        }
    }

    public void UpdateTalentColors()
    {
        foreach (Talent talent in Talents)
        {
            talent.UpdateColor();
        }
    }

    public void initTalentRectTransforms()
    {
        foreach (Talent talent in Talents)
        {
            talent.initRectTransform();
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Hide();
        }
    }

}
