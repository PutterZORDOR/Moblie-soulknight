using UnityEngine;
using System.Collections;

public class DoubleSpiralBoss : Boss
{
    public int spiralBulletCount = 30;
    public float spiralRotationSpeed = 200f;
    public float spiralOffset = 180f;
    public float vortexSpeed = 50f; // Speed of the vortex
    public int shotsBeforeCooldown = 150; // Number of shots before cooldown
    public float cooldownDuration = 7f;  // Duration of cooldown in seconds

    private float currentAngle1 = 0f;
    private float currentAngle2 = 90f;
    private int shotsFired = 0;
    private bool isCoolingDown = false;

    protected override void Start()
    {
        base.Start();
        fireRate = 1.5f; // Adjust fire rate for this boss
        damage = 10; // Set custom damage value for this boss
        bulletSpeed = 10f;
        bulletLifetime = 10f;
    }

    protected override void ShootPlayer()
    {
        if (!isCoolingDown)
        {
            // Adjust angles to create vortex effect
            currentAngle1 += vortexSpeed * Time.deltaTime;
            currentAngle2 -= vortexSpeed * Time.deltaTime;

            ShootSpiral(currentAngle1);
            ShootSpiral(currentAngle2 + spiralOffset);

            shotsFired += spiralBulletCount;

            if (shotsFired >= shotsBeforeCooldown)
            {
                StartCoroutine(Cooldown());
            }
        }
    }

    protected override void AttackPlayer()
    {
        base.AttackPlayer();
    }

    private void ShootSpiral(float angle)
    {
        for (int i = 0; i < spiralBulletCount; i++)
        {
            float bulletAngle = angle + (360f / spiralBulletCount) * i;
            Vector2 bulletDirection = new Vector2(
                Mathf.Cos(bulletAngle * Mathf.Deg2Rad),
                Mathf.Sin(bulletAngle * Mathf.Deg2Rad)
            );

            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.linearVelocity = bulletDirection * bulletSpeed;

            // Apply a random angular velocity to make the bullet spin
            float randomSpin = Random.Range(-360f, 360f); // Random spin value between -360 and 360 degrees per second
            rb.angularVelocity = randomSpin;

            Destroy(bullet, bulletLifetime);
        }
    }

    private IEnumerator Cooldown()
    {
        isCoolingDown = true;
        yield return new WaitForSeconds(cooldownDuration);
        shotsFired = 0; // Reset the shot counter after cooldown
        isCoolingDown = false;
    }
}
