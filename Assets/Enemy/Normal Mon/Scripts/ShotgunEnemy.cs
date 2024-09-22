﻿using UnityEngine;
using System.Collections;


public class ShotgunMon : EnemyBase
{
    public float shootRange = 5f; // Range for shooting
    public float attackCooldown = 2f; // Cooldown between attacks
    public GameObject bulletPrefab; // Bullet prefab for ranged attack
    public Transform shootPoint; // Point from where bullets are shot
    public int numberOfBullets = 5; // Number of bullets per shot
    public float spreadAngle = 15f; // Spread angle for shotgun effect
    public float meleeDamage = 20f; // Melee damage
    public float bulletSpeed = 10f; // Bullet speed
    public float bulletLifetime = 2f; // Bullet lifetime
    public int bulletDamage = 10; // Bullet damage

    private float lastAttackTime;
    private bool playerInRange = false;
    private bool isAttacking = false;
    public Animator anim;
    public float stoppingDistance = 3f;
    public float attackDelay = 1f; // Delay before shooting bullets

    protected override void Start()
    {
        base.Start();
        CircleCollider2D detectionCollider = gameObject.AddComponent<CircleCollider2D>();
        detectionCollider.isTrigger = true;
        detectionCollider.radius = detectionRange;
        anim = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();

        // If not attacking, handle movement and attack logic
        if (!isAttacking)
        {
            HandleMovementAndAttack();
        }
    }

    private void HandleMovementAndAttack()
    {
        if (playerInRange)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer <= shootRange && Time.time >= lastAttackTime + attackCooldown)
            {
                ShootPlayer(); // Perform ranged attack with delay
            }
            else
            {
                MoveTowardsPlayer(); // Move towards the player if not in range
            }
        }
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
            anim.SetBool("isWalking", false);
        }
    }

    private void ShootPlayer()
    {
        isAttacking = true;
        anim.SetTrigger("Attack"); // Trigger shoot animation
        anim.SetBool("isWalking", false); // Stop walking during attack
    }
    public void isShooting()
    {
        Vector3 shootDirection = (player.position - shootPoint.position).normalized;

        for (int i = 0; i < numberOfBullets; i++)
        {
            float angle = Random.Range(-spreadAngle / 2, spreadAngle / 2);
            Vector3 direction = Quaternion.Euler(0, 0, angle) * shootDirection;
            foreach (GameObject bullets in Bullet_Manager_Pool.instance.enemy_Shotgun)
            {
                if (!bullets.activeSelf)
                {
                    bullets.transform.position = shootPoint.position;
                    Bullet bulletScript = bullets.GetComponent<Bullet>();
                    bulletScript.Initialize(direction, bulletDamage, bulletSpeed, bulletLifetime);
                    bullets.SetActive(true);
                    break;
                }
            }
        }

        lastAttackTime = Time.time;
        isAttacking = false;
    }

    protected override void OnPlayerDetected()
    {
        playerInRange = true;
        if (!isAttacking)
        {
            MoveTowardsPlayer(); // Move towards player when detected
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
