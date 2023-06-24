using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTower : Tower
{
    // Override the FireAtEnemy method.
    public override void Attack()
    {
        if (Target != null)
        {
            Projectile projectileInstance = Instantiate(ProjectilePrefab, transform.position, Quaternion.identity, ProjectilesParent).GetComponent<Projectile>();
            projectileInstance.Initialize(Target, transform.position);
        }

    }
}

