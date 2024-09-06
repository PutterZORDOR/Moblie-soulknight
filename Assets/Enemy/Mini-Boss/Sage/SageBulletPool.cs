using System.Collections.Generic;
using UnityEngine;

public class SageBulletPool : MonoBehaviour
{
    public GameObject bulletPrefab;
    public int poolSize = 600;
    private List<GameObject> bulletPool;
    


    void Awake()
    {
        bulletPool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            bulletPool.Add(bullet);
        }
    }

    public GameObject GetBullet()
    {
        foreach (GameObject bullet in bulletPool)
        {
            if (!bullet.activeInHierarchy)
            {
                return bullet;
            }
        }

        // Optionally expand the pool or return null if you want to limit the pool size
        GameObject newBullet = Instantiate(bulletPrefab);
        newBullet.SetActive(false);
        bulletPool.Add(newBullet);
        return newBullet;
    }


}
