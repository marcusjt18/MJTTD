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
        speedText.text = $"Speed: {tower.AttackSpeed}";
        rangeText.text = $"Range: {tower.Range}";
    }
}
