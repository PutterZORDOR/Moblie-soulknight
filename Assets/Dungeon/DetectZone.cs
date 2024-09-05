using UnityEngine;

public class DetectZone : MonoBehaviour
{
    [SerializeField] private GameObject[] _enermyToSpawnIn;
    [SerializeField] private Collider2D _currentRoomSpawnAble;

    [Header("BoxPrefab")]
    public GameObject BoxPrefab;

    [Header("Animator")]
    public Animator doorAnim1;
    public Animator doorAnim2;
    public Animator doorAnim3;
    public Animator doorAnim4;

    public Room _currentRoom;
    public bool CanSpawnEnermy = true;
    public bool CanDestroy;
    public Collider2D colDoor;

    private void Start()
    {
        GameObject SpawnAreaName = GameObject.Find($"SpawnArea {gameObject.name}");
        if (SpawnAreaName != null)
            _currentRoomSpawnAble = SpawnAreaName.GetComponent<Collider2D>();
        GameObject FindRoom = GameObject.Find($"Room {gameObject.name}");
        if( FindRoom != null)
        {
            _currentRoom = FindRoom.GetComponent<Room>();
        }
    }
    private void OnTriggerStay2D(Collider2D player)
    {
        if (player.CompareTag("Player") && CanSpawnEnermy)
        {
            Vector3 playerPosition = player.transform.position;
            Vector3 detectZoneCenter = transform.position;

            float distance = Vector3.Distance(playerPosition, detectZoneCenter);
            if (distance <= DungeonSystem.instance.detectionRadius)
            {
                CanDestroy = true;
                for (int i = 0; i < _currentRoom.door.Count; i++)
                {
                    if (_currentRoom.door[i] != null)
                    {
                        Animator doorAnim = _currentRoom.door[i].GetComponentInChildren<Animator>();
                        colDoor = _currentRoom.door[i].GetComponentInChildren<Collider2D>();

                        switch (i)
                        {
                            case 0:
                                doorAnim1 = doorAnim;
                                doorAnim1.Play("DoorClose");
                                colDoor.enabled = true;
                                break;
                            case 1:
                                doorAnim2 = doorAnim;
                                doorAnim2.Play("DoorClose");
                                colDoor.enabled = true;
                                break;
                            case 2:
                                doorAnim3 = doorAnim;
                                doorAnim3.Play("DoorClose");
                                colDoor.enabled = true;
                                break;
                            case 3:
                                doorAnim4 = doorAnim;
                                doorAnim4.Play("DoorClose");
                                colDoor.enabled = true;
                                break;
                        }
                    }
                }
                EnermySpawnManager.instance.SpawnEnermy(_currentRoomSpawnAble, _enermyToSpawnIn);
                CanSpawnEnermy = false;
            }
        }
    }
    private void Update()
    {
        if(DungeonSystem.instance.AllEnermyInRoom == 0 && CanDestroy)
        {
            for (int i = 0; i < _currentRoom.door.Count; i++)
            {
                if (_currentRoom.door[i] != null)
                {
                    Animator doorAnim = _currentRoom.door[i].GetComponentInChildren<Animator>();

                    switch (i)
                    {
                        case 0:
                            doorAnim1 = doorAnim;
                            doorAnim1.Play("DoorOpen");
                            colDoor.enabled = false;
                            break;
                        case 1:
                            doorAnim2 = doorAnim;
                            doorAnim2.Play("DoorOpen");
                            colDoor.enabled = false;
                            break;
                        case 2:
                            doorAnim3 = doorAnim;
                            doorAnim3.Play("DoorOpen");
                            colDoor.enabled = false;
                            break;
                        case 3:
                            doorAnim4 = doorAnim;
                            doorAnim4.Play("DoorOpen");
                            colDoor.enabled = false;
                            break;
                    }
                }
            }
            _currentRoom.Box = Instantiate(BoxPrefab, _currentRoom.transform.position, Quaternion.identity);
            _currentRoom.Box.transform.SetParent(_currentRoom.transform);
            _currentRoomSpawnAble.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
        }
    }
}