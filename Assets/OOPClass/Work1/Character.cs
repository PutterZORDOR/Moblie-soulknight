using UnityEngine;

public class Character : MonoBehaviour
{
    public int MaxHp;
    public int CurrentHp;
    public int AttackPower;

    public virtual void Start()
    {
        CurrentHp = MaxHp;
    }
    public void Movement(string name)
    {
        Debug.Log($"Walk : {name}");
    } 
    public void Eat(string name)
    {
        Debug.Log($"Eat : {name}");
    }
    public void TakeDmg()
    {
        CurrentHp--;
        Debug.Log($"Hp : {CurrentHp}");
    }
    public void Attack()
    {
        Debug.Log($"Attack : {AttackPower}");
    }
    public void Sleep(string name)
    {
        Debug.Log($"Sleep : {name}");
    }

    protected virtual void Update()
    {
        
    }
}
