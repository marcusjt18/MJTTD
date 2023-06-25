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

    [SerializeField]
    private GameObject damageTextPrefab;

    private Vector3 startPosition;
    private Vector3 direction;

    public Vector3 Direction { get => direction; set => direction = value; }

    public virtual void Initialize(Monster target, Vector3 startPosition)
    {
        this.target = target;
        this.startPosition = startPosition;

        // Determine the initial direction to the target.
        direction = target.transform.position - startPosition;
        direction.Normalize();

        // Rotate the projectile to face the initial direction.
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    protected virtual void Update()
    {
        if (Vector3.Distance(startPosition, transform.position) > maxDistance)
        {
            // If the projectile has traveled more than its max distance, destroy it.
            Destroy(gameObject);
            return;
        }

        transform.position += direction * speed * Time.deltaTime;

    }


    void OnTriggerEnter2D(Collider2D other)
    {
        Monster monster = other.GetComponent<Monster>();
        if (monster != null)
        {
            // If the projectile hits a monster, damage it and destroy the projectile.
            monster.TakeDamage(damage);

            // First, get a reference to the Canvas
            Canvas canvas = FindObjectOfType<Canvas>();

            GameObject damageText = Instantiate(damageTextPrefab, transform.position, Quaternion.identity, canvas.transform);
            damageText.GetComponent<TMPro.TextMeshProUGUI>().text = damage.ToString();
            Destroy(damageText, 0.2f); // Destroy after half a second
           
            Destroy(gameObject); //destroy this projectile
        }
    }
}
