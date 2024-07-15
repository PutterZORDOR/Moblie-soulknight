using UnityEngine;

public class Food: Eatable
{
    public Player player;

    private void Awake()
    {
        player = FindAnyObjectByType<Player>();
    }
    private void Start()
    {
        Cname = "Cake";
        amount = 10;
        fillHunger = 25;
    }

    public override void Eat()
    {
        base.Eat();
        player.CurrentHunger += fillHunger;

    }
}
