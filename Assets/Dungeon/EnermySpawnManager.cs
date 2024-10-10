using UnityEngine;

public class EnermySpawnManager : MonoBehaviour
{
    public static EnermySpawnManager instance;

    [Header("Prefab Monster")]
    public GameObject Melee_Enemy;
    public GameObject Mage_Enemy;
    public GameObject Rush_Enemy;
    public GameObject Shotgun_Enemy;
    public GameObject Sniper_Enemy;

    [Header("Pool Enemy Setting")]
    public GameObject[] melee;
    public GameObject[] mage;
    public GameObject[] rush;
    public GameObject[] shotgun;
    public GameObject[] sniper;

    [Space(7)]
    [SerializeField] private GameObject[] _enermyToSpawnIn;
    [SerializeField] private LayerMask layerNotSpawn;
    public int minEnermyInRoom;
    public int maxEnermyInRoom;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        melee = new GameObject[maxEnermyInRoom];
        mage = new GameObject[maxEnermyInRoom];
        rush = new GameObject[maxEnermyInRoom];
        shotgun = new GameObject[maxEnermyInRoom];
        sniper = new GameObject[maxEnermyInRoom];

        InitializePool(Melee_Enemy, melee);
        InitializePool(Mage_Enemy, mage);
        InitializePool(Rush_Enemy, rush);
        InitializePool(Shotgun_Enemy, shotgun);
        InitializePool(Sniper_Enemy, sniper);
    }
    private void InitializePool(GameObject prefab, GameObject[] pool)
    {
        for (int i = 0; i < pool.Length; i++)
        {
            pool[i] = Instantiate(prefab, transform.position, Quaternion.identity);
            pool[i].SetActive(false);
        }
    }
    public void SpawnEnermy(Collider2D spawnAbleAreaCollider)
    {
        int randomAmount = Random.Range(minEnermyInRoom, maxEnermyInRoom);
        DungeonSystem.instance.AllEnermyInRoom = randomAmount;
        int Allenermy = _enermyToSpawnIn.Length;

        for (int i = 0; i < randomAmount; ++i)
        {
            int randomIndex = Random.Range(0, Allenermy);
            GameObject enermyShouldSpawn = GetPooledEnermy(randomIndex);

            if (enermyShouldSpawn != null)
            {
                Vector2 spawnPosition = GetRandomSpawnPosition(spawnAbleAreaCollider);
                enermyShouldSpawn.transform.position = spawnPosition;
                enermyShouldSpawn.transform.rotation = Quaternion.identity; 
                enermyShouldSpawn.SetActive(true);
                EnemyBase component = enermyShouldSpawn.GetComponent<EnemyBase>();
                component.ResetStat();
            }
        }
    }

    private GameObject GetPooledEnermy(int index)
    {
        GameObject[] pool = null;

        switch (index)
        {
            case 0:
                pool = melee;
                break;
            case 1:
                pool = mage;
                break;
            case 2:
                pool = rush;
                break;
            case 3:
                pool = shotgun;
                break;
            case 4:
                pool = sniper;
                break;
            default:
                return null;
        }

        // Find an inactive enemy in the selected pool
        foreach (GameObject enermy in pool)
        {
            if (!enermy.activeInHierarchy)
            {
                return enermy; // Return the first inactive enemy found
            }
        }

        return null; // No available enemies in the pool
    }


    private Vector2 GetRandomSpawnPosition(Collider2D spawnAbleAreaCollider)
    {
        Vector2 spawnPosition = Vector2.zero;
        bool isSpawnPosValid = false;

        int attemptCount = 0;
        int maxAttempts = 200;

        while (!isSpawnPosValid && attemptCount < maxAttempts)
        {
            spawnPosition = GetRandomPointInCollider(spawnAbleAreaCollider);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(spawnPosition, 0.6f);

            bool isInvalidCollision = false;
            foreach (Collider2D collider in colliders)
            {
                if (((1 << collider.gameObject.layer) & layerNotSpawn) != 0)
                {
                    isInvalidCollision = true;
                    break;
                }
            }

            if (!isInvalidCollision)
            {
                isSpawnPosValid = true;
            }

            attemptCount++;
        }

        return spawnPosition;
    }

    private Vector2 GetRandomPointInCollider(Collider2D collider)
    {
        Bounds collBounds = collider.bounds;

        Vector2 minBounds = new Vector2(collBounds.min.x + DungeonSystem.instance.radiusSpawn, collBounds.min.y + DungeonSystem.instance.radiusSpawn);
        Vector2 maxBounds = new Vector2(collBounds.max.x - DungeonSystem.instance.radiusSpawn, collBounds.max.y - DungeonSystem.instance.radiusSpawn);

        float randomX = Random.Range(minBounds.x, maxBounds.x);
        float randomY = Random.Range(minBounds.y, maxBounds.y);

        return new Vector2(randomX, randomY);
    }
}
