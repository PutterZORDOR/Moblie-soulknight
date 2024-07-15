using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int HungerMax = 100;
    public int CurrentHunger;
    public int HpMax = 100;
    public int CurrentHp;
    public bool CanDecrease;
    public bool CanEat;
    public Food food;
    public Potion potion;

    private void Awake()
    {
        food = FindAnyObjectByType<Food>();
        potion = FindAnyObjectByType<Potion>();
    }
    void Start()
    {
        CurrentHp = HpMax;
        CurrentHunger = HungerMax;
    }
    void Update()
    {
        if (CanDecrease)
        {
            StartCoroutine(HungerDecrease());
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (CanEat)
            {
                StartCoroutine(CanEating());
            }
        }
        if(Input.GetKeyDown(KeyCode.G))
        {
            potion.Regenaration();
            if (CurrentHp > HpMax)
            {
                CurrentHp = HpMax;
                Debug.Log($"Current = {CurrentHp}");
            }else
            {
                Debug.Log($"Current = {CurrentHp}");
            }
        }
        if(Input.GetKeyDown(KeyCode.T))
        {
            CurrentHp -= 20;
            Debug.Log($"Try to suicide: CurrentHp = {CurrentHp}");
        }
    }
    IEnumerator CanEating()
    {
        Debug.Log("Eat");
        CanEat = false;
        food.Eat();
        if (CurrentHunger > HungerMax)
            CurrentHunger = HungerMax;
        yield return new WaitForSeconds(4);
        CanEat = true;
    }

    IEnumerator HungerDecrease()
    {
        CanDecrease = false;
        CurrentHunger--;
        yield return new WaitForSeconds(1);
        CanDecrease = true;
        
    }
}
