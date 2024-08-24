using UnityEngine;

public class MachineGun : Weapon
{
    public GameObject Bullet;
    public float fireRate = 0.1f; // Time between each bullet
    private float nextFireTime = 0f;

    protected override void Attack()
    {
        // โค้ดสำหรับการโจมตีของปืน
        Debug.Log("Shooting with Gun!");
        // เพิ่มโค้ดการยิงที่นี่
    }
    public virtual void Shoot()
    {
        GameObject bulletInstance = Instantiate(Bullet, ShootPoint.position, weapon.transform.rotation);
        Vector2 shootDirection = weapon.transform.right;
        bulletInstance.GetComponent<Rigidbody2D>().AddForce(shootDirection * Force);
        nextFireTime = Time.time + fireRate;
    }
}
