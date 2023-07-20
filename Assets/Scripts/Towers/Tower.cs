using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    private float damageMultiplier = 1.0f;


    [SerializeField]
    private GameObject prefab;

    private TMP_Text levelLabel;

    [SerializeField]
    private string projectileTag;

    [SerializeField]
    private float attackSpeed = 1.0f;

    // The time since the tower last attacked
    private float attackCooldown = 0.0f;

    private int level = 1;
    private int maxLevel = 10;

    private Color[] levelColors = new Color[]
    {
        new Color(1f, 1f, 1f, 1f), // Level 1 - white
        new Color(1f, 0.977f, 0.610f, 1f),
        new Color(1f, 0.961f, 0.364f, 1f),
        new Color(1f, 0.751f, 0.364f, 1f),
        new Color(1f, 0.534f, 0.364f, 1f),
        new Color(1f, 0.325f, 0.289f, 1f),
        new Color(1f, 0f, 0.093f, 1f),
        new Color(0.64f, 0f, 0f, 1f),
        new Color(0.45f, 0f, 0f, 1f),
        new Color(0.251f, 0.247f, 0.288f, 1f)

    };


    [SerializeField]
    private int cost = 10;
    [SerializeField]
    private int upgradeCost = 10;

    private int sellPrice;

    private int talentPoints = 0;

    private HashSet<int> talentIds = new HashSet<int>();

    public Monster Target { get; set; }

    public string Id { get => id; set => id = value; }
    public float Range { get => range; set => range = value; }

    public GameObject Prefab { get => prefab; set => prefab = value; }
    public int MaxDamage { get => maxDamage; set => maxDamage = value; }
    public int MinDamage { get => minDamage; set => minDamage = value; }
    public string ProjectileTag { get => projectileTag; set => projectileTag = value; }
    public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    public int Level { get => level; set => level = value; }
    public int MaxLevel { get => maxLevel; set => maxLevel = value; }
    public int SellPrice { get => sellPrice; set => sellPrice = value; }
    public int UpgradeCost { get => upgradeCost; set => upgradeCost = value; }
    public int TalentPoints { get => talentPoints; set => talentPoints = value; }
    public float DamageMultiplier { get => damageMultiplier; set => damageMultiplier = value; }
    public HashSet<int> TalentIds { get => talentIds; set => talentIds = value; }
    public int Cost { get => cost; set => cost = value; }

    private List<Monster> monstersInRange = new List<Monster>();

    void Start()
    {

        CircleCollider2D circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.radius = range;

        SellPrice = Cost / 2;

        levelLabel = GetComponentInChildren<TMP_Text>();
        UpdateLevelLabel();
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

    public int ApplyMultiplierToDamage(int dmg)
    {
        return Mathf.RoundToInt(dmg*damageMultiplier);
    }
    public int CalculateDamage()
    {
        int min = ApplyMultiplierToDamage(minDamage);
        int max = ApplyMultiplierToDamage(maxDamage);

        return UnityEngine.Random.Range(min, max + 1);
    }

    public void AddTalentId(int id)
    {
        talentIds.Add(id);
    }

    public void LevelUp()
    {
        if (level >= maxLevel)
        {
            return;
        }

        if (Player.Instance.Gold >= UpgradeCost)
        {
            Player.Instance.SpendGold(UpgradeCost);

            // Apply color tinting
            if (level < levelColors.Length)
            {
                GetComponent<SpriteRenderer>().color = levelColors[level];
            }

            if (level >= 7)
            {
                float scaleFactor = 1.06f;
                transform.localScale *= scaleFactor;
            }

            //increase damage in a sensible way
            minDamage *= 2;
            maxDamage = minDamage + (level * 2);

            TalentPoints++;

            level++;
            SellPrice = UpgradeCost / 2;
            UpgradeCost *= 2;
            UpdateLevelLabel();
        }
        else
        {
            UIManager.Instance.DisplayNoFundsText();
        }
    }

    public void SellTower()
    {
        Player.Instance.GainGold(sellPrice);
        Destroy(gameObject);
    }

    private void UpdateLevelLabel()
    {
        levelLabel.text = $"Lv. {level}";
    }
}

