using UnityEngine;

public class BulletPlayer : MonoBehaviour
{
    public float BulletLifeTime;
    public int Damage { get; set; }
    void Start()
    {
        CancelInvoke("DisableBullet");
        Invoke("DisableBullet", BulletLifeTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyBase enemy = other.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                enemy.TakeDamage(Damage);
                gameObject.SetActive(false);
            }
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("wall map"))
        {
            gameObject.SetActive(false);
        }
    }

    void DisableBullet()
    {
        gameObject.SetActive(false);
    }

}
