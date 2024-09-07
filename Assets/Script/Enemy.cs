using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 100;
    public float speed = 2f;
    public float moveDistance = 2f;

    private Vector2 startPosition;
    private bool movingRight = true;
    private bool movingUp = true;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        Vector2 position = transform.position;

        // Horizontal movement
        if (movingRight)
        {
            position.x += speed * Time.deltaTime;
            if (position.x >= startPosition.x + moveDistance)
                movingRight = false;
        }
        else
        {
            position.x -= speed * Time.deltaTime;
            if (position.x <= startPosition.x - moveDistance)
                movingRight = true;
        }

        transform.position = position;
    }

    public void TakeDamage(int damage = 10)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Destroy the enemy
        Destroy(gameObject);
    }
}
