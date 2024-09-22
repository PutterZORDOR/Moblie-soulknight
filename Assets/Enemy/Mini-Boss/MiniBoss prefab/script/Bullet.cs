using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 direction; // Direction of the bullet
    public int damage; // Damage value for this bullet
    public float speed; // Speed of the bullet
    public float lifetime; // Lifetime of the bullet

    public void Initialize(Vector3 dir, int dmg, float spd, float life)
    {
        direction = dir;
        damage = dmg;
        speed = spd;
        lifetime = life;
        CancelInvoke("DisableBullet");
        Invoke("DisableBullet", lifetime);
    }
    void DisableBullet()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerManager.instance.TakeDamgeAll(damage);
            gameObject.SetActive(false);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("wall map"))
        {
            gameObject.SetActive(false);
        }
    }
}
