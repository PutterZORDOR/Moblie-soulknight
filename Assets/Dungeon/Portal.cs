using UnityEngine;

public class Portal : MonoBehaviour
{
    DungeonMaker dungeon;
    private void Start()
    {
        dungeon = FindAnyObjectByType<DungeonMaker>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            dungeon.DestroyDungeon();
        }
    }
}
