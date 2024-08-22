using UnityEngine;

public class MeteorBoss : Boss
{
    public GameObject meteorPrefab;
    public float meteorSpread = 5f; // Distance between the meteors

    protected override void Start()
    {
        base.Start();
        fireRate = 8f;
        damage = 50;
        bulletLifetime = 10f;
        bulletSpeed = 12f;
    }

    protected override void Update()
    {
        base.Update();

        // Check if the boss should flip based on the player's position
        if (player != null)
        {
            Vector3 scale = transform.localScale;

            // If the player is to the right of the boss and the boss is not already facing right
            if (transform.position.x < player.position.x && scale.x < 0)
            {
                scale.x = Mathf.Abs(scale.x); // Flip to face right
            }
            // If the player is to the left of the boss and the boss is not already facing left
            else if (transform.position.x > player.position.x && scale.x > 0)
            {
                scale.x = -Mathf.Abs(scale.x); // Flip to face left
            }

            transform.localScale = scale;
        }
    }

    protected override void AttackPlayer()
    {
        base.AttackPlayer();
    }

    protected override void ShootPlayer()
    {
        if (meteorPrefab != null)
        {
            float spawnHeightAbovePlayer = 10f; // ปรับค่าเพื่อควบคุมความสูงของการสร้างดาวตก

            for (int i = -1; i <= 1; i++) // ลูปเพื่อสร้างดาวตก 3 ลูก
            {
                // สุ่มระยะการกระจายดาวตก
                float randomSpread = meteorSpread + Random.Range(-5f, 5f); // ปรับขอบเขตของการสุ่ม

                Vector2 spawnPosition = new Vector2(player.position.x + i * randomSpread, player.position.y + spawnHeightAbovePlayer);
                GameObject meteor = Instantiate(meteorPrefab, spawnPosition, Quaternion.identity);
                Rigidbody2D rb = meteor.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector2.down * bulletSpeed; // ใช้ bulletSpeed ที่ลดลง
                }

                Destroy(meteor, bulletLifetime);
            }
        }
    }

}
