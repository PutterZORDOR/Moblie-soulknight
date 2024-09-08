using UnityEngine;

public class Gun : Weapon
{
    public GameObject Bullet;
    public float SetBulletLifeTime;

    protected override void Attack()
    {
        if (isInWeaponSlot)
        {
            Debug.Log("Attack method called");
            GameObject BulletIns = Instantiate(Bullet, AttackPoint.position, weapon.transform.rotation);
            Vector2 shootDirection = weapon.transform.right;
            BulletIns.GetComponent<Rigidbody2D>().AddForce(shootDirection * Force, ForceMode2D.Impulse);
            BulletIns.GetComponent<BulletPlayer>().BulletLifeTime = SetBulletLifeTime;
        }
    }
}
