using UnityEngine;

public class DungeonSystem : MonoBehaviour
{
    public static DungeonSystem instance;
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
    public float RangeCentertoWall;
    public float radiusSpawn;
    public int AllEnermyInRoom;

    public int Level = 1;
    public float detectionRadius = 5.0f;
    public bool AllBossStatus;

    [Header("Dialogue")]
    public int shop_count;
}
