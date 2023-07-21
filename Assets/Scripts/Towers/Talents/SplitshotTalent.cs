using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitshotTalent : Talent
{

    [SerializeField]
    private int extraProjectiles = 2;

    private void Awake()
    {
        Title = "Split Shot";
        Description = $"Fires an additional {extraProjectiles} projectiles.";
    }

    public override void ApplyEffect(Tower tower)
    {
        return;
    }
}
