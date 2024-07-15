using UnityEngine;

public class DetectZoneAllBoss : MonoBehaviour
{
    [Header("BoxPrefab")]
    public GameObject BoxPrefab;

    [Header("MiniBoss")]
    public GameObject MiniBossPrefab;

    [Header("Boss")]
    public GameObject BossPrefab;

    [Header("Portal")]
    public GameObject Portal;

    [Header("EndPortal")]
    public GameObject EndPortal;

    private int Level;
    private Room _currentRoom;
    public bool CanSpawnEnermy = true;
    public bool CanDestroy;

    GameObject portal;

    private void Start()
    {
        Level = DungeonSystem.instance.Level;
        GameObject FindRoom = GameObject.Find($"Room {gameObject.name}");
        if (FindRoom != null)
            _currentRoom = FindRoom.GetComponent<Room>();

    }
    private void OnTriggerStay2D(Collider2D player)
    {
        if (player.CompareTag("Player"))
        {
            Vector3 playerPosition = player.transform.position;
            Vector3 detectZoneCenter = transform.position;

            float distance = Vector3.Distance(playerPosition, detectZoneCenter);
            if (distance <= DungeonSystem.instance.detectionRadius && CanSpawnEnermy)
            {
                CanDestroy = true;
                for (int i = 0; i < _currentRoom.door.Count; i++)
                {
                    if (_currentRoom.door[i] != null)
                        _currentRoom.door[i].gameObject.SetActive(true);
                }
                if(Level == 5 || Level == 10)
                {
                    DungeonSystem.instance.AllBossStatus = true;
                    GameObject Boss = Instantiate(MiniBossPrefab,gameObject.transform.position, Quaternion.identity);
                }else if(Level == 15)
                {
                    DungeonSystem.instance.AllBossStatus = true;
                    GameObject Boss = Instantiate(BossPrefab, gameObject.transform.position, Quaternion.identity);
                }

                CanSpawnEnermy = false;
            }
        }
    }
    private void Update()
    {
        if (DungeonSystem.instance.AllBossStatus == false && CanDestroy)
        {
            if(Level == 5 || Level == 10)
            {
                portal = Portal;
            }else if(Level == 15)
            {
                portal = EndPortal;
            }
            for (int i = 0; i < _currentRoom.door.Count; i++)
            {
                if (_currentRoom.door[i] != null)
                    _currentRoom.door[i].gameObject.SetActive(false);
            }
            _currentRoom.Portal = Instantiate(portal,_currentRoom.transform.position, Quaternion.identity);
            _currentRoom.Portal.transform.SetParent(_currentRoom.transform); 
            _currentRoom.Box = Instantiate(BoxPrefab,new Vector3(_currentRoom.transform.position.x,_currentRoom.transform.position.y - DungeonSystem.instance.RangeCentertoWall / 2,0), Quaternion.identity);
            _currentRoom.Box.transform.SetParent(_currentRoom.transform);
            Destroy(this.gameObject);
        }
    }
}
