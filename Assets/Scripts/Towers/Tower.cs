using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    [SerializeField]
    private string id; // The ID of this tower
    // Properties common to all towers, like range and damage.
    public float Range { get; set; }
    public int Damage { get; set; }
    public string Id { get => id; }

    // An abstract method for firing at enemies.
    public abstract void FireAtEnemy();
}

