using UnityEngine;

public class Gun : Weapon
{
    public GameObject Bullet;

    protected override void Attack()
    {
        GameObject BulletIns = Instantiate(Bullet, ShootPoint.position, weapon.transform.rotation);
        Vector2 shootDirection = weapon.transform.right;
        BulletIns.GetComponent<Rigidbody2D>().AddForce(shootDirection * Force);
    }
    /*public virtual void Shoot()
    {
        GameObject BulletIns = Instantiate(Bullet, ShootPoint.position, weapon.transform.rotation);
        Vector2 shootDirection = weapon.transform.right;
        BulletIns.GetComponent<Rigidbody2D>().AddForce(shootDirection * Force);
    }*/
}
