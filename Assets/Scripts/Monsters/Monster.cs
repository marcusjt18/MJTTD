using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Monster : MonoBehaviour
{
    [SerializeField]
    private float speed = 1.0f;

    private float currentSpeed;

    private int health;

    [SerializeField]
    private int maxHealth = 5;

    [SerializeField]
    private int goldYield = 5;

    [SerializeField]
    private int damageToPlayer = 1;


    [SerializeField]
    private MonsterHealthBar healthBar;

    [SerializeField]
    private string id;

    private ParticleSystemPool particleSystemPool;

    private float deathEffectDuration = 2f;


    protected List<TileScript> path;
    protected int currentTileIndex;

    public string Id { get => id; set => id = value; }
    public int MaxHealth { get => maxHealth; set => maxHealth = value; }
    public float CurrentSpeed { get => currentSpeed; set => currentSpeed = value; }
    public int GoldYield { get => goldYield; set => goldYield = value; }

    public virtual void Initialize(List<TileScript> path)
    {
        this.path = path;
        transform.position = LevelManager.Instance.StartTile.transform.position;
        currentTileIndex = 0;
        this.gameObject.AddComponent<DepthSorter>();
    }

    private void Awake()
    {
        health = this.maxHealth;
        CurrentSpeed = speed;
        particleSystemPool = ParticleSystemPool.Instance;
    }

    protected virtual void Update()
    {
        GetComponent<DepthSorter>().UpdateOrderForMonster();
        if (health <= 0)
        {
            Die(false);
        }

        if (currentTileIndex < path.Count)
        {
            transform.position = Vector3.MoveTowards(transform.position, path[currentTileIndex].transform.position, CurrentSpeed * Time.deltaTime);

            if (transform.position == path[currentTileIndex].transform.position)
            {
                currentTileIndex++;

                // If the monster has moved to the last tile, destroy the monster
                if (currentTileIndex >= path.Count)
                {
                    // Insert any other logic you want to occur when a monster reaches the end here
                    // For example, you might want to decrement the player's life total

                    Die(true);
                }
            }
        }
        
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthBar.UpdateHealthbar((float)health, (float)maxHealth);
    }

    private void Die(bool reachedEnd)
    {
        MonsterPool.Instance.ReturnToPool(this.Id, this.gameObject);
        GameManager.Instance.MonsterCounter--;
        if (reachedEnd)
        {
            Player.Instance.LoseHealth(damageToPlayer);
        }
        else
        {
            Player.Instance.GainGold(goldYield);
            TextPool.Instance.SpawnFromPool("FloatingGoldText", transform.position, Quaternion.identity, "+" + goldYield.ToString());
            particleSystemPool.SpawnFromPoolWithReturn("deathEffect", transform.position, Quaternion.identity, deathEffectDuration);
        }

    }

    public void ResetMonster()
    {
        health = this.maxHealth;
        CurrentSpeed = speed;
        healthBar.UpdateHealthbar((float)health, (float)maxHealth);

    }

    #region SlowEffect

    private Dictionary<int, SlowEffect> activeSlows = new Dictionary<int, SlowEffect>();
    public class SlowEffect
    {
        public float Slow { get; set; }
        public Coroutine Coroutine { get; set; }
    }

    public void ApplySlow(float slow, float duration)
    {
        var slowEffect = new SlowEffect() { Slow = slow };
        slowEffect.Coroutine = StartCoroutine(ApplySlowCoroutine(slowEffect, duration));
        activeSlows.Add(slowEffect.Coroutine.GetHashCode(), slowEffect);
        CurrentSpeed = CalculateCurrentSpeed();
    }

    private float CalculateCurrentSpeed()
    {
        if (activeSlows.Any())
        {
            float strongestSlow = activeSlows.Values.Max(effect => effect.Slow);
            return Mathf.Max(speed - strongestSlow, 0.2f);
        }
        else
        {
            return speed;
        }
    }

    private IEnumerator ApplySlowCoroutine(SlowEffect slowEffect, float duration)
    {
        yield return new WaitForSeconds(duration);
        activeSlows.Remove(slowEffect.Coroutine.GetHashCode());
        CurrentSpeed = CalculateCurrentSpeed();
    }

    #endregion

}

