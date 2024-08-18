using UnityEngine;

public class Pistol : Gun
{
    public override void Shoot()
    {
        base.Shoot();
        Debug.Log("Pistol shooting!");
    }
}
