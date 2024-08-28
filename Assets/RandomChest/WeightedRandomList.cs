using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeightedRandomList
{
    [System.Serializable]
    public struct Pair
    {
        public Weapon_Item itemData;
        public float weight;

        public Pair(float weight, Weapon_Item itemData)
        {
            this.weight = weight;
            this.itemData = itemData;
        }
    }

    public List<Pair> list = new List<Pair>();

    public int Count
    {
        get => list.Count;
    }

    public void Add(float weight, Weapon_Item itemData)
    {
        list.Add(new Pair(weight, itemData));
    }

    public Weapon_Item GetRandom()
    {
        float totalWeight = 0;

        foreach (Pair p in list)
        {
            totalWeight += p.weight;
        }

        float value = Random.value * totalWeight;

        float sumWeight = 0;

        foreach (Pair p in list)
        {
            sumWeight += p.weight;

            if (sumWeight >= value)
            {
                return p.itemData;
            }
        }

        return null;
    }
}

