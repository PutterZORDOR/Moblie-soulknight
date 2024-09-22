﻿using System;
using System.Collections;
using UnityEngine;

public class MageEnemy : EnemyBase
{
    public GameObject magicOrbPrefab; // Assign the magic orb prefab in the inspector
    public Transform firePoint; // Assign the fire point in the inspector
    public float summonCooldown = 5f; // Time between summoning orbs
    public float orbSpeed = 5f; // Speed of the homing orb
    public float orbLifetime = 10f; // Lifetime of the orb
    public int orbDamage = 30;
    public Animator anim;
    public float stoppingDistance = 3f;
    public float DebuffTime;

    private float lastSummonTime;
    private bool isAttacking = false; // ตัวแปรสำหรับเช็คสถานะการโจมตี

    Animator anims;

    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        moveSpeed = 1.5f; // Mage moves slower but has powerful magic attacks
        Physics2D.IgnoreCollision(col_Player, col_Enemy, true);
    }

    protected override void Update()
    {
        base.Update();

        // ถ้าโจมตีอยู่ ให้หยุดเคลื่อนไหว
        if (isAttacking)
        {
            anim.SetBool("isWalking", false); // หยุดแอนิเมชันการเดินขณะโจมตี
            return; // ออกจากฟังก์ชัน Update เพื่อหยุดการเคลื่อนไหว
        }

        // ตรวจสอบว่าเวลาที่จะเรียกใช้งาน Magic Orb ถึงหรือยัง
        if (Time.time >= lastSummonTime + summonCooldown & !isDie)
        {
            SummonMagicOrb();
            lastSummonTime = Time.time;
        }
    }
    protected override void OnDefeated()
    {
        gameObject.tag = "Untagged";
        anim.Play("MonMageDie");
    }
    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    protected override void OnPlayerDetected()
    {
        if (playerDetected && !isAttacking) // ถ้าผู้เล่นถูกตรวจจับและไม่ได้โจมตี
        {
            MoveTowardsPlayer(); // เคลื่อนที่ไปหาผู้เล่น
        }
        else
        {
            anim.SetBool("isWalking", false); // หยุดการเคลื่อนไหวถ้าไม่เจอผู้เล่น
        }
    }

    private void SummonMagicOrb()
    {
        isAttacking = true;
        anim.SetBool("isWalking", false);
        icon.SetActive(true);
        StartCoroutine(DelayBeforeAttack());
    }
    private IEnumerator DelayBeforeAttack()
    {
        yield return new WaitForSeconds(1f);
        icon.SetActive(false);
        anim.SetTrigger("Attack");
    }

    public void ShootOrb()
    {
        foreach (GameObject orb in Bullet_Manager_Pool.instance.Orb)
        {
            if (!orb.activeSelf)
            {
                HomingOrb homingOrbScript = orb.GetComponent<HomingOrb>();
                anims = orb.GetComponent<Animator>();
                homingOrbScript.SetTarget(player);
                homingOrbScript.SetSpeed(orbSpeed);
                homingOrbScript.SetDamage(orbDamage);
                homingOrbScript.SetDebuff(DebuffTime);
                orb.transform.position = firePoint.transform.position;
                orb.SetActive(true);
                break;
            }
        }
        StartCoroutine(DestroyAfterDelay(orbLifetime, anims));
    }

    private IEnumerator DestroyAfterDelay(float delay, Animator anim)
    {
        yield return new WaitForSeconds(delay);
        if (anim != null && anim.gameObject != null)
        {
            anim.Play("DestroyOrb");
        }
    }
    // ฟังก์ชันที่จะถูกเรียกเมื่อแอนิเมชันการโจมตีเสร็จสิ้น
    public void OnAttackComplete()
    {
        isAttacking = false; // การโจมตีเสร็จสิ้นแล้ว
    }

    private void MoveTowardsPlayer()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer > stoppingDistance)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            Vector2 newPosition = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
            anim.SetBool("isWalking", true);
            transform.position = newPosition;
        }
        else
        {
            // Stop the walking animation when close enough
            anim.SetBool("isWalking", false);
        }
    }
}
