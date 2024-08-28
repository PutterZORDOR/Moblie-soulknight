using UnityEngine;

public class Wand : Weapon
{
    public GameObject Bullet;
    public float SetBulletLifeTime;
    public bool isInWeaponSlot = false;
    protected override void Attack()
    {
        Debug.Log("Attack method called");
        GameObject BulletIns = Instantiate(Bullet, AttackPoint.position, weapon.transform.rotation);
        Vector2 shootDirection = weapon.transform.right;
        BulletIns.GetComponent<Rigidbody2D>().AddForce(shootDirection * Force);
        BulletIns.GetComponent<Bullet>().BulletLifeTime = SetBulletLifeTime;
    }
}
