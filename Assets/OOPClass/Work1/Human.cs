using UnityEngine;

public class Human: Character
{
    public void Speak()
    {
        Debug.Log("Speak!");
    }
    public override void Start()
    {
        MaxHp = 100;
        AttackPower = 20;
        base.Start();
        base.Sleep(name);
        base.Attack();
        base.TakeDmg();
        this.Speak();
        
    }
}
