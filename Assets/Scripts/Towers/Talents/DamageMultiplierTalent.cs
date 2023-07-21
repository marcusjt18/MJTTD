using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageMultiplierTalent : Talent
{
    [SerializeField]
    private float multi = 0.2f;

    private void Awake()
    {
        Title = "Damage Multiplier";
        Description = $"Increases base damage by {multi*100}%.";
    }

    public override void ApplyEffect(Tower tower)
    {
        tower.DamageMultiplier += multi;
    }
}
