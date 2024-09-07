using UnityEngine;

public class ThreeKnives : Weapon
{
    public float spreadAngle = 15f;
    public int KnifeCount = 10;
    public GameObject Spreadknife;
    public float SetBulletLifeTime;

    protected override void Attack()
    {
        for (int i = 0; i < KnifeCount; i++)
        {
            float angle = (i - 1) * spreadAngle; // Adjust the angle for each knife
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            float spreadFactor = i / (float)KnifeCount;
            GameObject knife = Instantiate(Spreadknife, AttackPoint.position, weapon.transform.rotation);
            Vector2 shootDirection = rotation * weapon.transform.right;
            knife.GetComponent<Rigidbody2D>().AddForce(shootDirection * Force);

            knife.GetComponent<Bullet>().lifetime = SetBulletLifeTime;
        }
    }
}
