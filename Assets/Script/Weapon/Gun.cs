using UnityEngine;

public class Gun : Weapon
{
    public GameObject Bullet;
    public float SetBulletLifeTime;
    protected override void Attack()
    {
        Debug.Log("Attack method called");
        GameObject BulletIns = Instantiate(Bullet, ShootPoint.position, weapon.transform.rotation);
        Vector2 shootDirection = weapon.transform.right;
        BulletIns.GetComponent<Rigidbody2D>().AddForce(shootDirection * Force);
        BulletIns.GetComponent<Bullet>().BulletLifeTime = SetBulletLifeTime;
    }
    /*public virtual void Shoot()
    {
        GameObject BulletIns = Instantiate(Bullet, ShootPoint.position, weapon.transform.rotation);
        Vector2 shootDirection = weapon.transform.right;
        BulletIns.GetComponent<Rigidbody2D>().AddForce(shootDirection * Force);
    }*/
}
