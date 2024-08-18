using UnityEngine;

public class Gun : MonoBehaviour
{
    public float Range;
    bool Detected = false;
    Vector2 Direction;
    public GameObject gun;
    public float rotationSpeed = 5f;

    public GameObject Bullet;
    public Transform ShootPoint;
    public float Force;

    private bool flipped = false;
    public Transform characterTransform;

    void Update()
    {
        // Find the closest enemy
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0) return;

        GameObject closestEnemy = enemies[0];
        float closestDistance = Vector2.Distance(transform.position, closestEnemy.transform.position);

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestEnemy = enemy;
                closestDistance = distance;
            }
        }

        // Set targetPos to the closest enemy's position
        Vector2 targetPos = closestEnemy.transform.position;
        Direction = targetPos - (Vector2)transform.position;
        RaycastHit2D rayInfo = Physics2D.Raycast(transform.position, Direction, Range);

        if (rayInfo)
        {
            if (rayInfo.collider.gameObject.tag == "Enemy")
            {
                if (!Detected)
                {
                    Detected = true;
                    Debug.Log("Detected Enemy");
                }
            }
        }
        else
        {
            if (Detected)
            {
                Detected = false;
                Debug.Log("Not Detected Enemy");
            }
        }

        if (Detected)
        {
            // Rotate gun towards the closest enemy
            Vector3 direction = closestEnemy.transform.position - gun.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            gun.transform.rotation = Quaternion.Slerp(gun.transform.rotation, rotation, rotationSpeed * Time.deltaTime);

            // Adjust character flip based on gun rotation
            float currentAngle = gun.transform.eulerAngles.z;
            if (currentAngle > 180) currentAngle -= 360;

            if (Mathf.Abs(currentAngle) > 90 && !flipped)
            {
                flipped = true;
                FlipCharacter();
            }
            else if (Mathf.Abs(currentAngle) <= 90 && flipped)
            {
                flipped = false;
                UnflipCharacter();
            }
        }
    }

    void FlipCharacter()
    {
        if (characterTransform != null)
        {
            Vector3 scale = characterTransform.localScale;
            scale.x *= -1;
            characterTransform.localScale = scale;
        }
        Debug.Log("Character Flipped");
    }

    void UnflipCharacter()
    {
        if (characterTransform != null)
        {
            Vector3 scale = characterTransform.localScale;
            scale.x *= -1;
            characterTransform.localScale = scale;
        }
        Debug.Log("Character Unflipped");
    }

    public void shoot()
    {
        GameObject BulletIns = Instantiate(Bullet, ShootPoint.position, gun.transform.rotation);
        Vector2 shootDirection = gun.transform.right;
        BulletIns.GetComponent<Rigidbody2D>().AddForce(shootDirection * Force);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, Range);
    }
}
