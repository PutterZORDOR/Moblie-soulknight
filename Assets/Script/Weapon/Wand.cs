using UnityEngine;

public class Wand : Weapon
{
    public GameObject Thunder;
    public float SetBulletLifeTime;
    public bool isInWeaponSlot = false;
    
    protected override void Attack()
    {
        Instantiate(Thunder, AttackPoint.position, Quaternion.identity);
        Debug.Log("Attacked");
    }
}
