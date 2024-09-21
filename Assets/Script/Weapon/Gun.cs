using UnityEngine;

public class Gun : Weapon
{
    public GameObject Bullet;
    public float SetBulletLifeTime;

    protected override void Attack()
    {
        if (isInWeaponSlot && PlayerManager.instance.Mana >= Mana && !joystickMoveScript.isDashing)
        {
            PlayerManager.instance.UseMana(Mana);
            Vector2 shootDirection = weapon.transform.right;
            foreach (GameObject bullet in Bullet_Manager_Pool.instance.normal)
            {
                if (!bullet.activeSelf)
                {
                    bullet.transform.position = AttackPoint.position;
                    bullet.transform.rotation = weapon.transform.rotation;
                    bullet.SetActive(true);
                    bullet.GetComponent<Rigidbody2D>().AddForce(shootDirection * Force, ForceMode2D.Impulse);
                    bullet.GetComponent<BulletPlayer>().BulletLifeTime = SetBulletLifeTime;
                    bullet.GetComponent<BulletPlayer>().Damage = Damage;
                    break;
                }
            }
        }
    }
}
