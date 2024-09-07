using UnityEngine;

public class ShortGun : Weapon
{
    public int pelletCount = 10;
    public float spreadAngle = 15f;
    public GameObject Bullet;
    public float SetBulletLifeTime;
    public bool isInWeaponSlot = false;
    void Start()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Bullet"), LayerMask.NameToLayer("Bullet"));
    }

    protected override void Attack()
    {
        for (int i = 0; i < pelletCount; i++)
        {
            float angle = Random.Range(-spreadAngle, spreadAngle);
            Quaternion pelletRotation = Quaternion.Euler(0, 0, angle);
            float spreadFactor = i / (float)pelletCount;
            GameObject pellet = Instantiate(Bullet, AttackPoint.position, weapon.transform.rotation);
            Vector2 shootDirection = pelletRotation * weapon.transform.right;
            pellet.GetComponent<Rigidbody2D>().AddForce(shootDirection * Force);

            pellet.GetComponent<Bullet>().BulletLifeTime = SetBulletLifeTime;
        }
        Debug.Log("Shortgun shooting!");
    }
}