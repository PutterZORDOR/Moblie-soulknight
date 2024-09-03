using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float BulletLifeTime;
    Weapon weapon;
    public int Damage { get; set; }
    void Start()
    {
        Destroy(gameObject, BulletLifeTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // ตรวจจับวัตถุที่มีแท็ก enemy
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(Damage);
            }

            // ทำลายกระสุน
            Destroy(gameObject);
        }
    }

}
