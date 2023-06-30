using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTiers : MonoBehaviour
{
    [SerializeField] private List<GameObject> tier1;
    [SerializeField] private List<GameObject> tier2;
    [SerializeField] private List<GameObject> tier3;
    [SerializeField] private List<GameObject> tier4;
    [SerializeField] private List<GameObject> tier5;
    [SerializeField] private List<GameObject> tier6;
    [SerializeField] private List<GameObject> tier7;
    [SerializeField] private List<GameObject> tier8;
    [SerializeField] private List<GameObject> tier9;

    public List<List<GameObject>> Tiers { get; private set; }

    private void Awake()
    {
        Tiers = new List<List<GameObject>>
        {
            tier1,
            tier2,
            tier3,
            tier4,
            tier5,
            tier6,
            tier7,
            tier8,
            tier9
        };
    }
}



