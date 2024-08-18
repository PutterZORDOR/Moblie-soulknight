using UnityEngine;

public class MachineGun : Gun
{
    public float fireRate = 0.1f; // เวลาระหว่างการยิงแต่ละครั้ง
    private float nextFireTime = 0f;

    /*void Update()
    {
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
    }*/

    public override void Shoot()
    {
        base.Shoot();
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
        }
        Debug.Log("Machine gun shooting!");
    }
}
