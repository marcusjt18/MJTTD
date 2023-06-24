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
    private int damage;
    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private GameObject projectilePrefab;

    public Transform ProjectilesParent;


    [SerializeField]
    private float attackSpeed = 1.0f;

    // The time since the tower last attacked
    private float attackCooldown = 0.0f;

    public Monster Target { get; set; }

    public string Id { get => id; set => id = value; }
    public float Range { get => range; set => range = value; }
    public int Damage { get => damage; set => damage = value; }
    public GameObject Prefab { get => prefab; set => prefab = value; }
    public GameObject ProjectilePrefab { get => projectilePrefab; set => projectilePrefab = value; }

    private List<Monster> monstersInRange = new List<Monster>();

    void Start()
    {
        ProjectilesParent = GameObject.Find("ProjectilesParent").transform;
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
                attackCooldown = attackSpeed;
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
            Debug.Log(Target.Id);
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
            Debug.Log(monstersInRange);
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Monster monster = other.GetComponent<Monster>();
        if (monster != null)
        {
            monstersInRange.Remove(monster);
            Debug.Log(monster + "REMOVED");
        }
    }
}

