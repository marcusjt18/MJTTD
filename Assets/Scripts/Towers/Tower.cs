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

    public string Id { get => id; set => id = value; }
    public float Range { get => range; set => range = value; }
    public int Damage { get => damage; set => damage = value; }
    public GameObject Prefab { get => prefab; set => prefab = value; }


    // An abstract method for firing at enemies.
    public abstract void FireAtEnemy();
}

