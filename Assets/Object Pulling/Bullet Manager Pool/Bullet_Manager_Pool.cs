using UnityEngine;

public class Bullet_Manager_Pool : MonoBehaviour
{
    [Header("Prefab Player Bullet")]
    public GameObject normal_Bullet;
    public GameObject single_Bullet;
    public GameObject blast_Fire;

    [Space(7)]
    public GameObject[] normal;
    public GameObject[] single;
    public GameObject[] blast;

    [Header("Prefab Enemy Bullet")]
    public GameObject enemy_Bullet_Sniper;
    public GameObject enemy_Bullet_Shotgun;
    public GameObject enemy_Orb;

    [Space(7)]
    public GameObject[] enemy_Sniper;
    public GameObject[] enemy_Shotgun;
    public GameObject[] Orb;

    [Header("Pool Settings")]
    public int num_Bullet = 7;
    public int num_Bullet_Enemy = 15;

    public static Bullet_Manager_Pool instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        normal = new GameObject[num_Bullet];
        single = new GameObject[num_Bullet];
        blast = new GameObject[num_Bullet];
        enemy_Sniper = new GameObject[num_Bullet_Enemy];
        enemy_Shotgun = new GameObject[num_Bullet_Enemy];
        Orb = new GameObject[num_Bullet_Enemy];

        InitializePool(normal_Bullet, normal);
        InitializePool(single_Bullet, single);
        InitializePool(blast_Fire, blast);
        InitializePool(enemy_Bullet_Sniper, enemy_Sniper);
        InitializePool(enemy_Bullet_Shotgun, enemy_Shotgun);
        InitializePool(enemy_Orb, Orb);
    }

    private void InitializePool(GameObject prefab, GameObject[] pool)
    {
        for (int i = 0; i < pool.Length; i++)
        {
            pool[i] = Instantiate(prefab, transform.position, Quaternion.identity);
            pool[i].SetActive(false);
        }
    }
}
