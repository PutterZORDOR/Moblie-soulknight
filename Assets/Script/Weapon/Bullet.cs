using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float BulletLifeTime;
    void Start()
    {
        Destroy(gameObject, BulletLifeTime);
    }
}
