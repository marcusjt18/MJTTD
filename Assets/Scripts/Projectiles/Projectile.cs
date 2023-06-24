using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [SerializeField]
    protected int damage = 1;
    [SerializeField]
    protected float speed = 5f;
    protected Monster target;
    [SerializeField]
    protected float maxDistance = 10f;
    private Vector3 startPosition;

    public void Initialize(Monster target)
    {
        this.target = target;
        this.startPosition = this.transform.position;
    }

    protected virtual void Update()
    {
        if (Vector3.Distance(startPosition, transform.position) > maxDistance)
        {
            // If the projectile has traveled more than its max distance, destroy it.
            Destroy(gameObject);
            return;
        }

        if (target != null)
        {
            // Determine the direction to the target.
            Vector3 direction = target.transform.position - transform.position;
            direction.Normalize();

            // Rotate the projectile to face the target.
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            // Move towards the target.
            transform.position += direction * speed * Time.deltaTime;
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        Monster monster = other.GetComponent<Monster>();
        if (monster != null)
        {
            // If the projectile hits a monster, damage it and destroy the projectile.
            monster.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
