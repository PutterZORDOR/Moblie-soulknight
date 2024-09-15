using UnityEngine;

public class HomingOrb : MonoBehaviour
{
    private Transform target;
    private float speed;
    private int damage;
    private Animator anim;
    private bool Stop;

    public void SetTarget(Transform player)
    {
        target = player;
        anim = GetComponent<Animator>();
    }

    public void SetSpeed(float orbSpeed)
    {
        speed = orbSpeed;
    }

    public void SetDamage(int orbDamage)
    {
        damage = orbDamage;
    }

    private void Update()
    {
        if (target == null) return;

        if (!Stop)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

            // Optionally rotate towards player if needed
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerManager.instance.TakeDamgeAll(damage);
            Stop = true;
            anim.Play("DestroyOrb");
        }
    }
    public void DestroyOrb()
    {
        Destroy(gameObject);
    }
}
