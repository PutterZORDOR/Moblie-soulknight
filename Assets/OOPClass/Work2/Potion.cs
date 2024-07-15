using UnityEngine;

public class Potion : Eatable
{
    public Player player;

    private void Awake()
    {
        player = FindAnyObjectByType<Player>();
    }
    private void Start()
    {
        Cname = "Heal Potion";
        amount = 10;
        heal = 10;
    }
    public override void Regenaration()
    {
        base.Regenaration();
        player.CurrentHp += heal;
    }

}
