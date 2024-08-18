using UnityEngine;

public class ShortGun : Gun
{
    public int pelletCount = 10;
    public float spreadAngle = 15f;

    public override void Shoot()
    {
        for (int i = 0; i < pelletCount; i++)
        {
            // Randomly calculate spread angle
            float angle = Random.Range(-spreadAngle, spreadAngle);
            Quaternion pelletRotation = Quaternion.Euler(0, 0, angle);
            GameObject pellet = Instantiate(Bullet, ShootPoint.position, pelletRotation);
            Vector2 shootDirection = pellet.transform.right;
            pellet.GetComponent<Rigidbody2D>().AddForce(shootDirection * Force);
        }
        Debug.Log("Shotgun shooting!");
    }
}
