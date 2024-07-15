using UnityEngine;

public class EnermySpawnManager : MonoBehaviour
{
    public static EnermySpawnManager instance;

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

    public void SpawnEnermy(Collider2D spawnAbleAreaCollider, GameObject[] enermy)
    {
        int randomAmount = Random.Range(minEnermyInRoom, maxEnermyInRoom);
        DungeonSystem.instance.AllEnermyInRoom = randomAmount;
        int Allenermy = enermy.Length;
        for (int i = 0; i < randomAmount; ++i)
        {
            int randomIndex = Random.Range(0, Allenermy);
            GameObject enermyShouldSpawn = enermy[randomIndex];
            Vector2 spawnPosition = GetRandomSpawnPosition(spawnAbleAreaCollider);
            GameObject spawnedEnermy = Instantiate(enermyShouldSpawn, spawnPosition, Quaternion.identity);
        }
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
