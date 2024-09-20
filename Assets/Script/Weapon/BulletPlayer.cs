using UnityEngine;

public class BulletPlayer : MonoBehaviour
{
    public float BulletLifeTime;
    public int Damage { get; set; }
    void Start()
    {
        Destroy(gameObject, BulletLifeTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyBase enemy = other.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                enemy.TakeDamage(Damage);
                Debug.Log("Enemy take damage");
                Destroy(gameObject);
            }
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("wall map"))
        {
            Destroy(gameObject);
        }
    }

}
