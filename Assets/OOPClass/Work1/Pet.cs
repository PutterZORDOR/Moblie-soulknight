using UnityEngine;

public class Pet : Character
{
    public void Bark()
    {
        Debug.Log("Bark!");
    }
    public override void Start()
    {
        MaxHp = 20;
        AttackPower = 5;
        base.Start();
        base.Sleep(name);
        base.Attack();
        base.TakeDmg();
        this.Bark();
    }
}
