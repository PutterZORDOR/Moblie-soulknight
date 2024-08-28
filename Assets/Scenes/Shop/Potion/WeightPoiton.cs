using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeightPotion
{
    [System.Serializable]
    public struct Pair
    {
        public All_Potion itemData;
        public float weight;

        public Pair(float weight, All_Potion itemData)
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

    public void Add(float weight, All_Potion itemData)
    {
        list.Add(new Pair(weight, itemData));
    }

    public All_Potion GetRandom()
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

