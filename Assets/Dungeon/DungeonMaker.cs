using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class DungeonMaker : MonoBehaviour
{
    public Vector2Int roomSize;
    public Room[,] rooms;
    public float roomDistance;
    public GameObject bridge;
    public int roomCount;
    public int RandomDeleteAmount;
    public int Delay;
    public Room lastRoom;
    public Room startRoom;
    [SerializeField] bool CanShop;
    [SerializeField] bool CanChest;

    [Header("UI GIF")]
    public GameObject GIF;
    public LoadingText _LoadingText;

    [Header("Detect Drop Weapon")]
    public DetectDropWeapon Detect_Drop_Weapon;

    [Header("AllRoom")]
    public GameObject[] RoomsPrefab;

    [Header("Grid")]
    public GameObject Grid;
    [Header("ImpportantRoom")]
    public GameObject StartRoomPrefab;
    public GameObject EndRoomPrefab;
    public GameObject MiniBossRoomPrefab;
    public GameObject BossRoomrefab;

    [Header("Prefab")]
    public GameObject PortalPrefab;
    public GameObject DectectZonePrefab;
    public GameObject DectectZoneMiniPrefab;
    public GameObject DectectZoneBossPrefab;
    public GameObject SpawnEnermyAreaPrefab;
    public GameObject doorTopBottom;
    public GameObject doorLeft;
    public GameObject doorRight;
    public GameObject Wall_Top;
    public GameObject Wall_Bottom;
    public GameObject Wall_Left;
    public GameObject Wall_Right;

    private delegate void CustomAction(Room _room);
    public List<Room> DungeonRoom;
    public List<GameObject> bridgeFirst = new List<GameObject>();
    public List<GameObject> bridgeCreated = new List<GameObject>();
    public List<GameObject> AllGride = new List<GameObject>();

    private int initialRoomCount;
    private int Level;
    private float RangeFromWall;
    GameObject _player;

    void Start()
    {
        RangeFromWall = DungeonSystem.instance.RangeCentertoWall;
    _player = GameObject.FindGameObjectWithTag("Player");
        GIF.SetActive(true);
        _LoadingText.Start();
        Level = DungeonSystem.instance.Level;
        if (Level != 15)
        {
            initialRoomCount = roomCount;
            rooms = new Room[roomSize.x + 1, roomSize.y + 1];
            CreateGrid();
            CreateFirstRoom();
        }
        else
        {
            CreateFinalBossRoom();
        }
        
    }

    private void CreateFinalBossRoom()
    {
        GameObject BossRoom = Instantiate(BossRoomrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
        _player.transform.position = BossRoom.transform.position;
        Invoke("CloseUI", 1f);
    }

    public void CreateFirstRoom()
    {
        if(roomCount <= 0)
            return;
        Vector2Int randomPos = new Vector2Int(Random.Range(1,roomSize.x), Random.Range(1, roomSize.y));
        rooms[randomPos.x, randomPos.y].AssignGameObject(StartRoomPrefab);
        Room startPos = rooms[randomPos.x, randomPos.y];
        startRoom = startPos;
        _player.transform.position = startPos.gameObject.transform.position;
        DungeonRoom.Add(rooms[randomPos.x, randomPos.y]);
        roomCount--;
        CreateRooms(startPos);
    }
    public void CreateGrid()
    {
        for (int x = 1; x <= roomSize.x; x++)
        {
            for (int y = 1; y <= roomSize.y; y++)
            {
                GameObject grid = Instantiate(Grid, RoomPosition(x,y),Quaternion.identity);
                grid.name = $"Room {x},{y}";
                Room roomComponent = grid.AddComponent<Room>();
                AllGride.Add(grid);
                rooms[x, y] = roomComponent;
                rooms[x, y].pos = new Vector2Int(x, y);
            }
        }
    }
    public void CreateRooms(Room startPos)
    {
        //Debug.Log(startPos.pos);
        if(roomCount > 0 && roomCount < rooms.Length)
            CheckRoom(startPos);
        if (roomCount == 0)
        {
            lastRoom = DungeonRoom[DungeonRoom.Count - 1];
            RoomConnectBridge();
            StartCoroutine(DestroyNearBossRoom());
            Invoke("CreateDoorForAllRoom", 2);
            Invoke("CloseUI", 2.5f);
        }       
    }
    void DeactivateDuplicateBridges(List<GameObject> bridgeFirst)
    {
        // สร้าง Dictionary เพื่อเก็บชื่อของ GameObject และ List ของ GameObject ที่ชื่อซ้ำกัน
        Dictionary<string, List<GameObject>> bridgeGroups = new Dictionary<string, List<GameObject>>();

        // แยกกลุ่ม GameObject ตามชื่อ
        foreach (GameObject bridge in bridgeFirst)
        {
            if (bridge == null) continue; // ถ้า GameObject ไม่มีอยู่ให้ข้ามไป
            string bridgeName = bridge.name;
            if (!bridgeGroups.ContainsKey(bridgeName))
            {
                bridgeGroups[bridgeName] = new List<GameObject>();
            }
            bridgeGroups[bridgeName].Add(bridge);
        }

        // ทำการตรวจสอบในแต่ละกลุ่ม ถ้ามีมากกว่า 1 ตัว ให้ SetActive(false) ตัวที่ซ้ำกันหมด เหลือไว้ตัวเดียว
        foreach (var group in bridgeGroups)
        {
            List<GameObject> bridges = group.Value;
            if (bridges.Count > 1) // ถ้ามี GameObject ชื่อซ้ำกันมากกว่า 1
            {
                for (int i = 1; i < bridges.Count; i++) // เริ่มจาก index 1 เพราะต้องการให้เหลือตัวแรกไว้
                {
                    bridges[i].SetActive(false);
                }
            }
        }
    }
    public void CloseUI()
    {
        _LoadingText.StopLoading();
    }
    public void CheckRoom(Room _room)
    {
        List<CustomAction> actions = new List<CustomAction>();
        // Perform boundary checks for each direction
        if (_room.pos.y + 1 < rooms.GetLength(1) && rooms[_room.pos.x, _room.pos.y + 1].roomPrefab == null)
        { // Above
            actions.Add(CreateAbove);
        }
        if (_room.pos.x - 1 > 0 && rooms[_room.pos.x - 1, _room.pos.y].roomPrefab == null)
        { // Left
            actions.Add(CreateLeft);
        }
        if (_room.pos.x + 1 < rooms.GetLength(0) && rooms[_room.pos.x + 1, _room.pos.y].roomPrefab == null)
        { // Right
            actions.Add(CreateRight);
        }
        if (_room.pos.y - 1 > 0 && rooms[_room.pos.x, _room.pos.y - 1].roomPrefab == null)
        { // Below
            actions.Add(CreateBelow);
        }
        if (actions.Count > 0)
        {
            int randomIndex = Random.Range(0, actions.Count);
            //Debug.Log("randomIndex = " + actions[randomIndex].Method.Name);
            actions[randomIndex]?.Invoke(_room);
        }
        else
        {
            Vector2Int ranRoompos = DungeonRoom[Random.Range(3, DungeonRoom.Count)].pos;
            CreateRooms(rooms[ranRoompos.x,ranRoompos.y]);
        }
    }
    public async void CreateAbove(Room startpos)
    {
        await Task.Delay(Delay);
        int randomRoom = GetRandomForRoom();
        Vector2Int pos = new Vector2Int(startpos.pos.x, startpos.pos.y + 1);
        pos = CheckRoomForSure(randomRoom, pos);
        DungeonRoom.Add(rooms[pos.x, pos.y]);
        roomCount--;
        CreateRooms(rooms[pos.x, pos.y]);
        CreateBridge(startpos, pos);
    }
    public async void CreateLeft(Room startpos)
    {
        await Task.Delay(Delay);
        int randomRoom = GetRandomForRoom();
        Vector2Int pos = new Vector2Int(startpos.pos.x - 1, startpos.pos.y);
        pos = CheckRoomForSure(randomRoom, pos);
        DungeonRoom.Add(rooms[pos.x, pos.y]);
        roomCount--;
        CreateRooms(rooms[pos.x, pos.y]);
        CreateBridge(startpos, pos);
    }
    public async void CreateRight(Room startpos)
    {
        await Task.Delay(Delay);
        int randomRoom = GetRandomForRoom();
        Vector2Int pos = new Vector2Int(startpos.pos.x + 1, startpos.pos.y);
        pos = CheckRoomForSure(randomRoom, pos);
        DungeonRoom.Add(rooms[pos.x, pos.y]);
        roomCount--;
        CreateRooms(rooms[pos.x, pos.y]);
        CreateBridge(startpos, pos);
    }
    public async void CreateBelow(Room startpos)
    {
        await Task.Delay(Delay);
        int randomRoom = GetRandomForRoom();
        Vector2Int pos = new Vector2Int(startpos.pos.x, startpos.pos.y - 1);
        pos = CheckRoomForSure(randomRoom, pos);
        DungeonRoom.Add(rooms[pos.x, pos.y]);
        roomCount--;
        CreateRooms(rooms[pos.x, pos.y]);
        CreateBridge(startpos, pos);
    }

    private Vector2Int CheckRoomForSure(int randomRoom, Vector2Int pos)
    {
        if (roomCount != 1)
        {
            rooms[pos.x, pos.y].AssignGameObject(RoomsPrefab[randomRoom]);
        }
        else if (Level == 5 || Level == 10)
        {
            rooms[pos.x, pos.y].AssignGameObject(MiniBossRoomPrefab);
        }
        else if (Level == 15)
        {
            rooms[pos.x, pos.y].AssignGameObject(BossRoomrefab);
        }
        else if ((roomCount == 1) && (Level != 5 || Level != 10 || Level != 15))
        {
            rooms[pos.x, pos.y].AssignGameObject(EndRoomPrefab);
        }

        if ((randomRoom != 0 && randomRoom != 1) && roomCount -1 != 0)
        {
            CreateDetectionZone(rooms[pos.x, pos.y], DectectZonePrefab);
            CreateSpawnEnermy(rooms[pos.x, pos.y]);
        }else if((Level == 5 || Level == 10) && roomCount - 1 == 0)
        {
            CreateDetectionZone(rooms[pos.x, pos.y], DectectZoneMiniPrefab);
        }else if((randomRoom != 0 && randomRoom != 1) && roomCount - 1 == 0)
        {
            rooms[pos.x, pos.y].Portal = Instantiate(PortalPrefab, rooms[pos.x, pos.y].transform);
        }
        return pos;
    }

    private int GetRandomForRoom()
    {
        int randomRoom;

        if (roomCount == 3)
        {
            randomRoom = 0;
        }
        else if (roomCount == 5)
        {
            randomRoom = 1;
        }
        else
        {
            randomRoom = Random.Range(0, RoomsPrefab.Length);
        }

        if ((randomRoom == 0 && CanChest) || (randomRoom == 1 && CanShop))
        {
            randomRoom = Random.Range(2, RoomsPrefab.Length);
        }
        if (randomRoom == 0 && !CanChest)
        {
            CanChest = true;
        }
        if (randomRoom == 1 && !CanShop)
        {
            CanShop = true;
        }

        return randomRoom;
    }

    private void CreateDetectionZone(Room Detect,GameObject obj)
    {
        Detect.DetectZone = Instantiate(obj, Detect.transform);
        Detect.DetectZone.transform.SetParent(Detect.transform);
        Detect.DetectZone.name = $"{Detect.pos.x},{Detect.pos.y}";
    }
    private void CreateSpawnEnermy(Room SpawnEnermy)
    {
        SpawnEnermy.SawnEnermyArea = Instantiate(SpawnEnermyAreaPrefab, SpawnEnermy.transform);
        SpawnEnermy.SawnEnermyArea.transform.SetParent(SpawnEnermy.transform);
        SpawnEnermy.SawnEnermyArea.name = $"SpawnArea {SpawnEnermy.pos.x},{SpawnEnermy.pos.y}";
    }
    public GameObject CreateBridge(Room oldPos,Vector2Int curPos)
    {

        Vector3 pos = new Vector3(oldPos.pos.x + curPos.x, oldPos.pos.y + curPos.y, 0) / 2f * roomDistance;
        GameObject Bridge = null;

        if (oldPos.bridge.Exists(b => b.name == $"{oldPos.pos},{curPos}") ||
            rooms[curPos.x, curPos.y].bridge.Exists(b => b.name == $"{curPos},{oldPos.pos}"))
        {
            return null;
        }

        if (oldPos.pos.x > curPos.x || oldPos.pos.x < curPos.x)
        {
           Bridge = Instantiate(bridge, pos, Quaternion.identity);
        }
        if (oldPos.pos.y > curPos.y || oldPos.pos.y < curPos.y)
        {
            Bridge = Instantiate(bridge, pos, Quaternion.Euler(0,0,90));
        }
        Bridge.name = $"{oldPos.pos}&{curPos}";
        Bridge.transform.parent = GameObject.Find("Dungeon_Core").transform;
        bridgeFirst.Add(Bridge);
        oldPos.bridge.Add(Bridge);
        rooms[curPos.x,curPos.y].bridge.Add(Bridge);
        return Bridge;
    }
    public async void RoomConnectBridge()
    {
        await Task.Delay(100);
        List<(Room, Vector2Int)> bridgesToCreate = new List<(Room, Vector2Int)>();

        foreach (Room _room in DungeonRoom)
        {
            foreach (GameObject _bridge in _room.bridge)
            {
                // Above
                if (_room.pos.y + 1 < rooms.GetLength(1) && rooms[_room.pos.x, _room.pos.y + 1].roomPrefab != null)
                {
                    if (_bridge.name != $"{_room.pos},{rooms[_room.pos.x, _room.pos.y + 1].pos}")
                    {
                        bridgesToCreate.Add((_room, rooms[_room.pos.x, _room.pos.y + 1].pos));
                    }
                }
                // Left
                if (_room.pos.x - 1 > 0 && rooms[_room.pos.x - 1, _room.pos.y].roomPrefab != null)
                {
                    if (_bridge.name != $"{_room.pos},{rooms[_room.pos.x - 1, _room.pos.y].pos}")
                    {
                        bridgesToCreate.Add((_room, rooms[_room.pos.x - 1, _room.pos.y].pos));
                    }
                }
                // Right
                if (_room.pos.x + 1 < rooms.GetLength(0) && rooms[_room.pos.x + 1, _room.pos.y].roomPrefab != null)
                {
                    if (_bridge.name != $"{_room.pos},{rooms[_room.pos.x + 1, _room.pos.y].pos}")
                    {
                        bridgesToCreate.Add((_room, rooms[_room.pos.x + 1, _room.pos.y].pos));
                    }
                }
                // Below
                if (_room.pos.y - 1 > 0 && rooms[_room.pos.x, _room.pos.y - 1].roomPrefab != null)
                {
                    if (_bridge.name != $"{_room.pos},{rooms[_room.pos.x, _room.pos.y - 1].pos}")
                    {
                        bridgesToCreate.Add((_room, rooms[_room.pos.x, _room.pos.y - 1].pos));
                    }
                }
            }
        }
        // Now create the bridges after iterating
        foreach (var bridgeInfo in bridgesToCreate)
        {
            if (bridgeInfo.Item1 != null && bridgeInfo.Item2 != null)
            {
                if(isBridgeValid(bridgeInfo.Item1, bridgeInfo.Item2))
                    bridgeCreated.Add(CreateBridge(bridgeInfo.Item1, bridgeInfo.Item2));
            }
        }
        DeactivateDuplicateBridges(bridgeFirst);

    }
    public bool isBridgeValid(Room oldPos, Vector2Int curPos)
    {
        if (oldPos.bridge.Exists(b => b.name == $"{oldPos.pos},{curPos}") ||
            rooms[curPos.x, curPos.y].bridge.Exists(b => b.name == $"{curPos},{oldPos.pos}"))
        {
            return false;
        }
        return true;
    }
    public Room ConvertPosToXY(Vector2Int pos)
    {
        return rooms[pos.x,pos.y];
    }
    public Vector2 RoomPosition(int x,int y)
    {
        return new Vector2(x * roomDistance,y * roomDistance);
    }

    IEnumerator DestroyNearBossRoom()
    {
        yield return new WaitForSeconds(1);
        List<GameObject> newBridgeList = new List<GameObject>();
        int DestroybridgeCount = lastRoom.bridge.Count;
        foreach (GameObject bridge in lastRoom.bridge)
        {
            if (bridge.name == ($"{lastRoom.pos}&{startRoom.pos}"))
            {
                Destroy(bridge);
                DestroybridgeCount--;
            }
            else
            {
                newBridgeList.Add(bridge);
            }
        }
        List<GameObject> remainingBridges = new List<GameObject>(newBridgeList);
        newBridgeList.Clear();

        foreach (GameObject bridge in remainingBridges)
        {
            if (DestroybridgeCount > 1)
            {
                Destroy(bridge);
                DestroybridgeCount--;
            }
            else
            {
                newBridgeList.Add(bridge);
            }
        }

        lastRoom.bridge = newBridgeList;
    }

    public void CreateDoorForAllRoom()
    {
        foreach(Room room in DungeonRoom)
        {
            CheckRoomForDoor(room);
            CheckAroundlastRoom(room);
        }
    }

    public void CheckRoomForDoor(Room Door)
    {
        bool isLastRoom = Door == lastRoom; 
        // Top
        if (Door.pos.y + 1 < rooms.GetLength(1) && rooms[Door.pos.x, Door.pos.y + 1].roomPrefab != null)
        {
            if (isLastRoom)
            {
                if (Door.bridge.Count > 0 && Door.bridge[0].name == $"{Door.pos}&{new Vector2Int(Door.pos.x, Door.pos.y + 1)}")
                {
                    CreateDoorTop(Door);
                }
                else
                {
                    CreateWallTop(Door);
                }
            }
            else
            {
                CreateDoorTop(Door);
            }
        }
        else
        {
            CreateWallTop(Door);
        }
        // Bottom
        if (Door.pos.y - 1 > 0 && rooms[Door.pos.x, Door.pos.y - 1].roomPrefab != null)
        {
            if (isLastRoom)
            {
                if (Door.bridge.Count > 0 && Door.bridge[0].name == $"{Door.pos}&{new Vector2Int(Door.pos.x, Door.pos.y - 1)}")
                {
                    CreateDoorBottom(Door);
                }
                else
                {
                    CreateWallBottom(Door);
                }
            }
            else
            {
                CreateDoorBottom(Door);
            }
        }
        else
        {
            CreateWallBottom(Door);
        }
        // Right
        if (Door.pos.x + 1 < rooms.GetLength(0) && rooms[Door.pos.x + 1, Door.pos.y].roomPrefab != null)
        {
            if (isLastRoom)
            {
                if (Door.bridge.Count > 0 && Door.bridge[0].name == $"{Door.pos}&{new Vector2Int(Door.pos.x + 1, Door.pos.y)}")
                {
                    CreateDoorRight(Door);
                }
                else
                {
                    CreateWallRight(Door);
                }
            }
            else
            {
                CreateDoorRight(Door);
            }
        }
        else
        {
            CreateWallRight(Door);
        }
        // Left
        if (Door.pos.x - 1 > 0 && rooms[Door.pos.x - 1, Door.pos.y].roomPrefab != null)
        {
            if (isLastRoom)
            {
                if (Door.bridge.Count > 0 && Door.bridge[0].name == $"{Door.pos}&{new Vector2Int(Door.pos.x - 1, Door.pos.y)}")
                {
                    CreateDoorLeft(Door);  
                }
                else
                {
                    CreateWallLeft(Door);
                }
            }
            else
            {
                CreateDoorLeft(Door);
            }
        }
        else
        {
            CreateWallLeft(Door);
        }
    }

    private void CreateDoorLeft(Room Door)
    {
        Door.doorLeft = Instantiate(doorLeft, Door.transform.position + new Vector3(-RangeFromWall, 0f, 0f), Quaternion.identity);
        Door.doorLeft.transform.SetParent(Door.transform);
        Collider2D colDoor = Door.doorLeft.GetComponent<Collider2D>();
        colDoor.enabled = false;
        Door.door.Add(Door.doorLeft);

    }

    private void CreateDoorRight(Room Door)
    {
        Door.doorRight = Instantiate(doorRight, Door.transform.position + new Vector3(RangeFromWall, 0f, 0f), Quaternion.identity);
        Door.doorRight.transform.SetParent(Door.transform);
        Collider2D colDoor = Door.doorRight.GetComponent<Collider2D>();
        colDoor.enabled = false;
        Door.door.Add(Door.doorRight);
    }

    private void CreateDoorBottom(Room Door)
    {
        Door.doorBottom = Instantiate(doorTopBottom, Door.transform.position + new Vector3(0f, -RangeFromWall, 0f), Quaternion.identity);
        Door.doorBottom.transform.SetParent(Door.transform);
        Collider2D colDoor = Door.doorBottom.GetComponentInChildren<Collider2D>();
        colDoor.enabled = false;
        Door.door.Add(Door.doorBottom);
    }

    private void CreateDoorTop(Room Door)
    {
        Door.doorTop = Instantiate(doorTopBottom, Door.transform.position + new Vector3(0f, RangeFromWall, 0f), Quaternion.identity);
        Door.doorTop.transform.SetParent(Door.transform);
        Collider2D colDoor = Door.doorTop.GetComponentInChildren<Collider2D>();
        colDoor.enabled = false;
        Door.door.Add(Door.doorTop);
    }

    public void CheckAroundlastRoom(Room NearlastRoom)
    {
        string lastRoomBridgeName =  lastRoom.bridge[0].name;
        string[] SpiltPart = lastRoomBridgeName.Split('&');
        string nearRoomPart = SpiltPart[1];
        // Bottom of lastRoom
        if (NearlastRoom.pos.x == lastRoom.pos.x && NearlastRoom.pos.y + 1 == lastRoom.pos.y )
        {
            if(nearRoomPart != $"({NearlastRoom.pos.x}, {NearlastRoom.pos.y})")
            {
                Destroy(NearlastRoom.doorTop.gameObject);
                CreateWallTop(NearlastRoom);
            }
        }
        // Top of lastRoom
        if (NearlastRoom.pos.x == lastRoom.pos.x && NearlastRoom.pos.y - 1 == lastRoom.pos.y)
        {
            if (nearRoomPart != $"({NearlastRoom.pos.x}, {NearlastRoom.pos.y})")
            {
                Destroy(NearlastRoom.doorBottom.gameObject);
                CreateWallBottom(NearlastRoom);
            }
        }
        // Left of lastRoom
        if (NearlastRoom.pos.y == lastRoom.pos.y && NearlastRoom.pos.x + 1 == lastRoom.pos.x)
        {
            if (nearRoomPart != $"({NearlastRoom.pos.x}, {NearlastRoom.pos.y})")
            {
                Destroy(NearlastRoom.doorRight.gameObject);
                CreateWallRight(NearlastRoom);
            }
        }
        // Right of lastRoom
        if (NearlastRoom.pos.y == lastRoom.pos.y && NearlastRoom.pos.x - 1 == lastRoom.pos.x)
        {
            if (nearRoomPart != $"({NearlastRoom.pos.x}, {NearlastRoom.pos.y})")
            {
                Destroy(NearlastRoom.doorLeft.gameObject);
                CreateWallLeft(NearlastRoom);
            }
        }
    }
    private void CreateWallTop(Room Wall)
    {
        Wall.WallTop = Instantiate(Wall_Top, Wall.transform.position + new Vector3(0f, RangeFromWall, 0f), Wall_Top.transform.rotation);
        Wall.WallTop.transform.SetParent(Wall.transform);
    }

    private void CreateWallBottom(Room Wall)
    {
        Wall.WallBottom = Instantiate(Wall_Bottom, Wall.transform.position + new Vector3(0f, -RangeFromWall, 0f), Wall_Bottom.transform.rotation);
        Wall.WallBottom.transform.SetParent(Wall.transform);
    }

    private void CreateWallRight(Room Wall)
    {
        Wall.WallRight = Instantiate(Wall_Right, Wall.transform.position + new Vector3(RangeFromWall, 0f, 0f), Wall_Right.transform.rotation);
        Wall.WallRight.transform.SetParent(Wall.transform);
    }

    private void CreateWallLeft(Room Wall)
    {
        Wall.WallLeft = Instantiate(Wall_Left, Wall.transform.position + new Vector3(-RangeFromWall , 0f, 0f), Wall_Left.transform.rotation);
        Wall.WallLeft.transform.SetParent(Wall.transform);
    }
    public void DestroyDungeon()
    {
        Detect_Drop_Weapon.DetectDrop();
        foreach(GameObject grid in AllGride)
        {
            if(grid != null)
            {
                Destroy(grid);
            }
        }
        AllGride.Clear();
        foreach(GameObject bridge in bridgeFirst)
        {
            Destroy(bridge);
        }
        bridgeFirst.Clear();
        foreach (Room room in DungeonRoom)
        {
            if (room.DetectZone != null)
                Destroy(room.DetectZone);
            if (room.SawnEnermyArea != null)
                Destroy(room.SawnEnermyArea);
            foreach (GameObject door in room.door)
            {
                Destroy(door);
            }
            Destroy(room.gameObject);
        }
        DungeonRoom.Clear();
        foreach (GameObject bridge in bridgeCreated)
        {
            Destroy(bridge);
        }
        bridgeCreated.Clear();

        DungeonSystem.instance.Level++;
        CanShop = false;
        CanChest = false;
        roomCount = initialRoomCount;
        if (Level != 15)
            Start();
    }
}

public class Room : MonoBehaviour
{
    [Header("GameObject in this Room")]
    public GameObject roomPrefab;

    public List<GameObject> bridge = new List<GameObject>();
    public List<GameObject> door = new List<GameObject>();
    public Vector2Int pos;

    [Header("DetectZone")]
    public GameObject DetectZone;

    [Header("SpawnEnermyArea")]
    public GameObject SawnEnermyArea;

    [Header("Door")]
    public GameObject doorTop;
    public GameObject doorBottom;
    public GameObject doorLeft;
    public GameObject doorRight;

    [Header("Wall")]
    public GameObject WallTop;
    public GameObject WallBottom;
    public GameObject WallLeft;
    public GameObject WallRight;

    [Header("Box")]
    public GameObject Box;

    [Header("Portal")]
    public GameObject Portal;
    
    public void AssignGameObject(GameObject go)
    {
        roomPrefab = go;
        GameObject room = Instantiate(roomPrefab, transform);
        room.transform.position = transform.position;
    }
}