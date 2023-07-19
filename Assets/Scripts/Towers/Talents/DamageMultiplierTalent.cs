using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageMultiplierTalent : Talent
{
    public override void ApplyEffect(Tower tower)
    {
        tower.DamageMultiplier += 0.2f;
    }
}
