using UnityEngine;

public class Animal : Character
{
    public void DropItem()
    {
        Debug.Log("Item drop");
    }
    public override void Start()
    {
        MaxHp = 10;
        base.Start();
        base.Sleep(name);
        base.TakeDmg();
        this.DropItem();
    }
}
