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
            // Replace the Instantiate call with the SpawnFromPool method
            GameObject projectileObject = ProjectilePool.Instance.SpawnFromPool(ProjectileTag, transform.position, Quaternion.identity);

            if (projectileObject != null) // This checks if the pool returned an object (i.e., the pool wasn't empty)
            {
                Projectile projectileInstance = projectileObject.GetComponent<Projectile>();
                projectileInstance.Initialize(Target, transform.position, CalculateDamage());
            }
        }
    }
}


