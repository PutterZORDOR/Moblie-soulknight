using UnityEngine;

public class Gun : Weapon
{
    public GameObject Bullet;

    public virtual void Shoot()
    {
        GameObject BulletIns = Instantiate(Bullet, ShootPoint.position, weapon.transform.rotation);
        Vector2 shootDirection = weapon.transform.right;
        BulletIns.GetComponent<Rigidbody2D>().AddForce(shootDirection * Force);
    }
}
