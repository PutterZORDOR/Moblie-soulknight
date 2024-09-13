﻿using UnityEngine;
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
        PlayerManager.instance.UseMana(Mana);
        if (Time.time >= nextFireTime && isInWeaponSlot)
        {
            StartCoroutine(FireBurst());
            nextFireTime = Time.time + fireRate;
        }
    }

    private IEnumerator FireBurst()
    {
        for (int i = 0; i < bulletsPerRound; i++)
        {
            GameObject BulletIns = Instantiate(Bullet, AttackPoint.position, weapon.transform.rotation);
            Vector2 shootDirection = weapon.transform.right;
            BulletIns.GetComponent<Rigidbody2D>().AddForce(shootDirection * Force);
            BulletIns.GetComponent<BulletPlayer>().BulletLifeTime = SetBulletLifeTime;
            yield return new WaitForSeconds(burstDelay);
        }
    }
}
