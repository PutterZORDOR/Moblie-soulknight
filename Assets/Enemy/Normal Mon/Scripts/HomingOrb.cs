using UnityEngine;

public class HomingOrb : MonoBehaviour
{
    private Transform target;
    private float speed;
    private int damage;
    private float slows;
    private Animator anim;
    private bool Stop;

    public void SetTarget(Transform player)
    {
        target = player;
        anim = GetComponent<Animator>();
    }
    public void SetDebuff(float slow)
    {
        slows = slow;
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

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            JoystickMove move = other.GetComponent<JoystickMove>();
            move.Debuff_Slow(slows);
            PlayerManager.instance.TakeDamgeAll(damage);
            PlayerManager.instance.ApplyDebuff("Slowness", Mathf.FloorToInt(slows));
            Stop = true;
            anim.Play("DestroyOrb");
        }
    }
    public void DestroyOrb()
    {
        Stop = false;
        gameObject.SetActive(false);
    }
}
