using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTiers : MonoBehaviour
{
    [System.Serializable]
    public class MonsterTier
    {
        public List<string> monsters;
        public int minLevel;
        public int maxLevel;
    }

    [SerializeField]
    private List<MonsterTier> tiers;
    public List<MonsterTier> Tiers { get => tiers; set => tiers = value; }

}





