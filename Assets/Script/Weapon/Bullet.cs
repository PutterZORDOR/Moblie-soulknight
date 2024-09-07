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
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(Damage);
                Debug.Log("Enemy take damage");
                Destroy(gameObject);
            }
        }

        if (other.CompareTag("Wall"))
        {
            Debug.Log("Smash");
            Destroy(gameObject);
        }
    }

}
