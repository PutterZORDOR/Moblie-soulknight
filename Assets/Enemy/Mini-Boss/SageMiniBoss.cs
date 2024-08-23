using UnityEngine;
using System.Collections;

public class SageBoss : MiniBoss
{
    public int spiralBulletCount = 1000;
    public float spiralRotationSpeed = 200f;
    public float spiralOffset = 175f;
    public float vortexSpeed = 50f;
    public float cooldownDuration = 4f; // Cooldown before switching patterns

    public float minPatternDuration = 5f;
    public float maxPatternDuration = 10f;

    // Firing rates for each pattern
    public float spiralFireRate = 3f;
    public float surroundFireRate = 3f;
    public float spinningFireRate = 0.1f;

    private float currentAngle1 = 0f;
    private float currentAngle2 = 90f;
    private bool isCoolingDown = false;
    private int currentPattern = 0; // 0 = Spiral, 1 = Surround, 2 = Spinning

    private Coroutine shootingCoroutine;

    protected override void Start()
    {
        base.Start();
        fireRate = 2f;
        damage = 10;
        bulletSpeed = 9f;
        bulletLifetime = 15f;

        // Start the pattern switching coroutine
        StartCoroutine(SwitchAttackPatterns());
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void ShootPlayer()
    {
        if (!isCoolingDown)
        {
            StartShootingPattern();
        }
    }

    private void StartShootingPattern()
    {
        if (shootingCoroutine != null)
        {
            StopCoroutine(shootingCoroutine);
        }

        switch (currentPattern)
        {
            case 0:
                shootingCoroutine = StartCoroutine(ShootSpiral());
                break;
            case 1:
                shootingCoroutine = StartCoroutine(ShootSurround());
                break;
            case 2:
                shootingCoroutine = StartCoroutine(ShootSpinning());
                break;
        }
    }

    private IEnumerator ShootSpiral()
    {
        while (currentPattern == 0)
        {
            currentAngle1 += vortexSpeed * Time.deltaTime;
            currentAngle2 -= vortexSpeed * Time.deltaTime;

            ShootSpiralBullets(currentAngle1);
            ShootSpiralBullets(currentAngle2 + spiralOffset);

            yield return new WaitForSeconds(spiralFireRate);
        }
    }

    private IEnumerator ShootSurround()
    {
        while (currentPattern == 1)
        {
            for (int i = 0; i < spiralBulletCount; i++)
            {
                float bulletAngle = (360f / spiralBulletCount) * i;
                Vector2 bulletDirection = new Vector2(
                    Mathf.Cos(bulletAngle * Mathf.Deg2Rad),
                    Mathf.Sin(bulletAngle * Mathf.Deg2Rad)
                );

                ShootBullet(bulletDirection);
            }

            yield return new WaitForSeconds(surroundFireRate);
        }
    }

    private IEnumerator ShootSpinning()
    {
        while (currentPattern == 2)
        {
            // Shoot in two directions
            float angle1 = Time.time * spiralRotationSpeed;
            float angle2 = (Time.time * spiralRotationSpeed) + 180f; // Offset by 180 degrees

            ShootBulletInDirection(angle1);
            ShootBulletInDirection(angle2);

            // Wait for the next shot
            yield return new WaitForSeconds(spinningFireRate);
        }
    }

    private void ShootSpiralBullets(float angle)
    {
        for (int i = 0; i < spiralBulletCount; i++)
        {
            float bulletAngle = angle + (360f / spiralBulletCount) * i;
            Vector2 bulletDirection = new Vector2(
                Mathf.Cos(bulletAngle * Mathf.Deg2Rad),
                Mathf.Sin(bulletAngle * Mathf.Deg2Rad)
            );

            ShootBullet(bulletDirection);
        }
    }

    private void ShootBulletInDirection(float angle)
    {
        Vector2 bulletDirection = new Vector2(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad)
        );

        ShootBullet(bulletDirection);
    }

    private void ShootBullet(Vector2 direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = direction * bulletSpeed;

        float randomSpin = Random.Range(-360f, 360f);
        rb.angularVelocity = randomSpin;

        Destroy(bullet, bulletLifetime);
    }

    private IEnumerator SwitchAttackPatterns()
    {
        while (true)
        {
            // Determine the duration for the current pattern
            float randomPatternDuration = Random.Range(minPatternDuration, maxPatternDuration);

            // Wait for the pattern duration
            yield return new WaitForSeconds(randomPatternDuration);

            // Start cooldown before switching to the next pattern
            yield return Cooldown();

            // Switch to the next pattern
            currentPattern = (currentPattern + 1) % 3; // Cycle through 0, 1, 2
            Debug.Log($"Changed to pattern {currentPattern}");

            // Restart the shooting pattern with the new pattern
            StartShootingPattern();
        }
    }

    private IEnumerator Cooldown()
    {
        isCoolingDown = true;
        yield return new WaitForSeconds(cooldownDuration);
        isCoolingDown = false;
    }
}
