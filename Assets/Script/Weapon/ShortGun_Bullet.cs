using UnityEngine;

public class ShortGun_Bullet : MonoBehaviour
{
    public float BulletLifeTime;


    void Start()
    {
        Destroy(gameObject,BulletLifeTime);
    }
}
