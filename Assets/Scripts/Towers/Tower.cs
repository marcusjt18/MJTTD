using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    [SerializeField]
    private string id;
    [SerializeField]
    private float range = 10f;
    [SerializeField]
    private int minDamage;
    [SerializeField]
    private int maxDamage;
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private string projectileTag;

    [SerializeField]
    private float attackSpeed = 1.0f;

    // The time since the tower last attacked
    private float attackCooldown = 0.0f;

    public Monster Target { get; set; }

    public string Id { get => id; set => id = value; }
    public float Range { get => range; set => range = value; }

    public GameObject Prefab { get => prefab; set => prefab = value; }
    public int MaxDamage { get => maxDamage; set => maxDamage = value; }
    public int MinDamage { get => minDamage; set => minDamage = value; }
    public string ProjectileTag { get => projectileTag; set => projectileTag = value; }
    public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }

    private List<Monster> monstersInRange = new List<Monster>();

    void Start()
    {

        CircleCollider2D circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.radius = range;

    }


    private void Update()
    {
        // If there's no target or the target is no longer in range...
        if (Target == null || !monstersInRange.Contains(Target))
        {
            // ...try to get a new target.
            GetNewTarget();
        }

        // If there is a target, the tower can potentially attack
        if (Target != null)
        {
            // If the attack is off cooldown, the tower attacks and the cooldown is reset
            if (attackCooldown <= 0)
            {
                Attack();
                attackCooldown = AttackSpeed;
            }
        }

        // Decrease the attack cooldown
        attackCooldown -= Time.deltaTime;
    }


    // An abstract method for firing at enemies.
    public abstract void Attack();

    public virtual void GetNewTarget()
    {
        if (monstersInRange.Count > 0)
        {
            // The first monster in the list is the new target.
            Target = monstersInRange[0];
        }
        else
        {
            // If there are no monsters in range, there is no target.
            Target = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Monster monster = other.GetComponent<Monster>();
        if (monster != null)
        {
            monstersInRange.Add(monster);
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Monster monster = other.GetComponent<Monster>();
        if (monster != null)
        {
            monstersInRange.Remove(monster);
        }
    }

    public int CalculateDamage()
    {
        return UnityEngine.Random.Range(minDamage, maxDamage + 1);
    }

    public void DisplayTowerUI()
    {
        UIManager.Instance.ShowTowerUI(this);
    }
}

