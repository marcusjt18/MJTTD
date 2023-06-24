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
    public Monster Target { get; set; }

    public string Id { get => id; set => id = value; }
    public float Range { get => range; set => range = value; }
    public int Damage { get => damage; set => damage = value; }
    public GameObject Prefab { get => prefab; set => prefab = value; }

    private List<Monster> monstersInRange = new List<Monster>();

    private void Update()
    {
        // If there's no target or the target is no longer in range...
        if (Target == null || !monstersInRange.Contains(Target))
        {
            // ...try to get a new target.
            GetNewTarget();
        }

        // Now you can use Target to aim and fire the tower's weapon.
        // But make sure to check if Target is not null before you do so!
    }


    // An abstract method for firing at enemies.
    public abstract void FireAtEnemy();

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

