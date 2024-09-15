using UnityEngine;

public class Gun : Weapon
{
    public GameObject Bullet;
    public float SetBulletLifeTime;

    protected override void Attack()
    {
        if (isInWeaponSlot && PlayerManager.instance.Mana >= Mana)
        {
            PlayerManager.instance.UseMana(Mana);
            GameObject BulletIns = Instantiate(Bullet, AttackPoint.position, weapon.transform.rotation);
            Vector2 shootDirection = weapon.transform.right;
            BulletIns.GetComponent<Rigidbody2D>().AddForce(shootDirection * Force, ForceMode2D.Impulse);
            BulletIns.GetComponent<BulletPlayer>().BulletLifeTime = SetBulletLifeTime;
            BulletIns.GetComponent<BulletPlayer>().Damage = Damage;
        }
    }
}
