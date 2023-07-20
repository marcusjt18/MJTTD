using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageMultiplierTalent : Talent
{
    [SerializeField]
    private float multi = 0.2f;

    public override void ApplyEffect(Tower tower)
    {
        tower.DamageMultiplier += multi;
    }
}
