using UnityEngine;
using System.Collections;
public class BurstGun : Weapon
{
    public GameObject Bullet;
    public float fireRate = 0.1f; // ช่วงเวลาระหว่างแต่ละนัด
    private float nextFireTime = 0f;
    public int bulletsPerRound = 3; // จำนวนกระสุนต่อรอบการโจมตี
    public float burstDelay = 0.05f; // หน่วงเวลาเล็กน้อยระหว่างการยิงแต่ละกระสุน
    public float SetBulletLifeTime;
    protected override void Attack()
    {
        if (isInWeaponSlot && PlayerManager.instance.Mana >= Mana && !joystickMoveScript.isDashing)
        {
            PlayerManager.instance.UseMana(Mana);
            if (Time.time >= nextFireTime)
            {
                StartCoroutine(FireBurst());
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    private IEnumerator FireBurst()
    {
        for (int i = 0; i < bulletsPerRound; i++)
        {
            Vector2 shootDirection = weapon.transform.right;
            foreach (GameObject bullet in Bullet_Manager_Pool.instance.single)
            {
                if (!bullet.activeSelf)
                {
                    bullet.transform.position = AttackPoint.position;
                    bullet.transform.rotation = weapon.transform.rotation;
                    bullet.SetActive(true);
                    bullet.GetComponent<Rigidbody2D>().AddForce(shootDirection * Force, ForceMode2D.Impulse);
                    bullet.GetComponent<BulletPlayer>().BulletLifeTime = SetBulletLifeTime;
                    bullet.GetComponent<BulletPlayer>().Damage = Damage * PlayerManager.instance.damgeMulti;
                    break;
                }
            }
            yield return new WaitForSeconds(burstDelay);
        }
    }
}
