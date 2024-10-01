using UnityEngine;

public class ShortGun : Weapon
{
    public int pelletCount = 10;
    public float spreadAngle = 15f;
    public GameObject Bullet;
    public float SetBulletLifeTime;
    void Start()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Bullet"), LayerMask.NameToLayer("Bullet"));
    }

    protected override void Attack()
    {
        if (isInWeaponSlot && PlayerManager.instance.Mana >= Mana && !joystickMoveScript.isDashing)
        {
            PlayerManager.instance.UseMana(Mana);
            for (int i = 0; i < pelletCount; i++)
            {
                float angle = Random.Range(-spreadAngle, spreadAngle);
                Quaternion pelletRotation = Quaternion.Euler(0, 0, angle);
                Vector2 shootDirection = pelletRotation * weapon.transform.right;
                foreach (GameObject bullet in Bullet_Manager_Pool.instance.blast)
                {
                    if (!bullet.activeSelf)
                    {
                        bullet.transform.position = AttackPoint.position;
                        bullet.transform.rotation = weapon.transform.rotation;
                        bullet.SetActive(true);
                        bullet.GetComponent<Rigidbody2D>().AddForce(shootDirection * Force, ForceMode2D.Impulse);
                        bullet.GetComponent<BulletPlayer>().BulletLifeTime = SetBulletLifeTime;
                        bullet.GetComponent<BulletPlayer>().Damage = Damage * PlayerManager.instance.damgeMulti;
                        break;
                    }
                }
            }
        }
    }
}