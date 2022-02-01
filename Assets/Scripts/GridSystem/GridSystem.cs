using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GridSystem : MonoBehaviour
{

    public static GridSystem Instance { get; private set; }

    public event EventHandler OnSelectedChanged;
    public event EventHandler OnObjectPlaced;

    private GridXZ<GridObj> grid;
    [SerializeField] private RoomSO[] roomSOList = null;
    [SerializeField] private List<RoomSO> placedRoomsList = null;
    [SerializeField] private RoomSO roomSO;
    private RoomSO.Dir dir;

    [SerializeField] private int widht = 4;
    [SerializeField] private int height = 3;
    [SerializeField] private float cellSize = 10f;
    [SerializeField] private Vector3 origin = new Vector3(-100,0,200);

    [SerializeField] private GameObject camera;
    [SerializeField] private GameObject pieGraph;

    [SerializeField] private AppController appController;

    public float ClickDuration = 2;
    public UnityEvent OnLongClick;

    bool clicking = false;
    float totalDownTime = 0;

    private void Awake()
    {
        Instance = this;

        grid = new GridXZ<GridObj>(widht, height, cellSize, gameObject, origin, (GridXZ<GridObj> g, int x, int z) => new GridObj(g, x, z));

        roomSO = null;
    }

    public void Start()
    {
        //Set all Rooms
        for(int x = 0; x < placedRoomsList.Count; x++)
        {
            InsertRoom(placedRoomsList[x]);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            totalDownTime = 0;
            clicking = true;
        }

        if(clicking && Input.GetMouseButton(0))
        {
            totalDownTime += Time.deltaTime;
            if(totalDownTime >= ClickDuration)
            {
                clicking = false;
                OnLongClick.Invoke();
            }
        }

        if(clicking && Input.GetMouseButtonUp(0))
        {
            clicking = false;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = MouseUtils.GetMouseWorldPosition();
            grid.GetXZ(mousePosition, out int x, out int z);
            PlacedRoom_Done placedRoom = grid.GetGridObject(x, z).GetPlacedRoom();
            if (placedRoom != null)
            {
                if (appController.editMode)
                {
                    roomSO = placedRoom.GetRoomSO();
                    DeleteRoom(placedRoom);
                    RefreshSelectedObjectType();
                }
                else
                {
                    //Set responsabilities correctly
                    CameraController controller = camera.GetComponent<CameraController>();
                    controller.SetObjectToView(placedRoom.GetToBeViewed());
                    controller.SetCameraToRoom(placedRoom.gameObject.transform.position);
                    //Set PieGraph
                    RoomSO roomSO = placedRoom.GetRoomSO();
                    appController.roomViewed = roomSO;
                    appController.goBackButton.SetActive(true);
                    List<float> sensorList = new List<float>();
                    foreach (SensorSO sensorSO in roomSO.sensors)
                    {
                        sensorList.Add(sensorSO.sensorValue);
                    }
                    pieGraph.GetComponent<PieGraph>().FillGraph(sensorList);
                }     
            }
        }

        /*
        if (Input.GetMouseButtonDown(0) && roomSO != null)
        {
            InsertRoom(roomSO);
        }
        if (Input.GetMouseButtonDown(1))
        {
            DeleteRoom();
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            dir = RoomSO.GetNextDir(dir);
            Debug.Log("" + dir);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) { roomSO = roomSOList[0]; RefreshSelectedObjectType(); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { roomSO = roomSOList[1]; RefreshSelectedObjectType(); }

        if (Input.GetKeyDown(KeyCode.Alpha0)) { DeselectObjectType(); }
        */
    }

    void Click()
    {

    }

    public void InsertRoom(RoomSO room)
    {
        //This is used if you put room by mousePosition
        //Vector3 mousePosition = MouseUtils.GetMouseWorldPosition();
        //grid.GetXZ(mousePosition, out int x, out int z);

        int x = room.roomPosition.x;
        int z = room.roomPosition.y;

        Vector2Int placedRoomOrigin = new Vector2Int(x, z);
        placedRoomOrigin = grid.ValidateGridPosition(placedRoomOrigin);

        List<Vector2Int> gridPositionList = room.GetGridPositionList(placedRoomOrigin, dir);

        //Test CanBuild
        bool canBuild = true;
        foreach (Vector2Int gridPosition in gridPositionList)
        {
            if (!grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild())
            {
                //Cannot build here
                canBuild = false;
                break;
            }
        }

        if (canBuild)
        {
            Vector2Int rotationOffset = room.GetRotationOffset(dir);
            Vector3 roomWorldPosition = grid.GetWorldPosition(placedRoomOrigin.x, placedRoomOrigin.y) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();

            PlacedRoom_Done placedRoom = PlacedRoom_Done.Create(roomWorldPosition, placedRoomOrigin, dir, room);

            foreach (Vector2Int gridPosition in gridPositionList)
            {
                grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedRoom(placedRoom);
            }

            placedRoomsList.Add(room);

            OnObjectPlaced?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Debug.Log("Can't build");
        }
    }

    public bool NextToRoom(Vector2Int position)
    {
        //Check each side to know is next to room
        //Down
        if (grid.GetGridObject(position.x, position.y + 1).GetPlacedRoom() != null)
        {
            return true;
        }
        //Up
        if (grid.GetGridObject(position.x, position.y - 1).GetPlacedRoom() != null)
        {
            return true;
        }
        //Left
        if (grid.GetGridObject(position.x + 1, position.y).GetPlacedRoom() != null)
        {
            return true;
        }
        //Right
        if (grid.GetGridObject(position.x - 1, position.y).GetPlacedRoom() != null)
        {
            return true;
        }
        Debug.Log("Next to Room False");
        return false;
    }

    public void DeleteRoom(PlacedRoom_Done placedRoom)
    {
        /*
        Vector3 mousePosition = MouseUtils.GetMouseWorldPosition();
        if (grid.GetGridObject(mousePosition) != null)
        {
            PlacedRoom_Done placedRoom = grid.GetGridObject(mousePosition).GetPlacedRoom();
        */
            if (placedRoom != null)
            {
                placedRoom.DestroySelf();

                List<Vector2Int> gridPositionList = placedRoom.GetGridPositionList();
                foreach (Vector2Int gridPosition in gridPositionList)
                {
                    grid.GetGridObject(gridPosition.x, gridPosition.y).ClearPlacedRoom();
                }

                placedRoomsList.Remove(placedRoom.GetRoomSO());
            }
        //}
    }

    private void DeselectObjectType()
    {
        roomSO = null; RefreshSelectedObjectType();
    }

    private void RefreshSelectedObjectType()
    {
        OnSelectedChanged?.Invoke(this, EventArgs.Empty);
    }

    public Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        grid.GetXZ(worldPosition, out int x, out int z);
        return new Vector2Int(x, z);
    }

    public Vector3 GetMouseWorldSnappedPosition()
    {
        Vector3 mousePosition = MouseUtils.GetMouseWorldPosition();
        grid.GetXZ(mousePosition, out int x, out int z);

        if (roomSO != null)
        {
            Vector2Int rotationOffset = roomSO.GetRotationOffset(dir);
            Vector3 placedObjectWorldPosition = grid.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();
            return placedObjectWorldPosition;
        }
        else
        {
            return mousePosition;
        }
    }

    public Quaternion GetPlacedObjectRotation()
    {
        if (roomSO != null)
        {
            return Quaternion.Euler(0, roomSO.GetRotationAngle(dir), 0);
        }
        else
        {
            return Quaternion.identity;
        }
    }

    public RoomSO GetRoomSO()
    {
        return roomSO;
    }
}

public class GridObj
{
    private GridXZ<GridObj> grid;
    private int x;
    private int z;
    private PlacedRoom_Done placedRoom;

    public GridObj(GridXZ<GridObj> grid, int x, int z)
    {
        this.grid = grid;
        this.x = x;
        this.z = z;
        placedRoom = null;
    }

    public void SetPlacedRoom(PlacedRoom_Done placedRoom)
    {
        this.placedRoom = placedRoom;
        grid.TriggerGridObjectChanged(x, z);
    }

    public PlacedRoom_Done GetPlacedRoom()
    {
        return placedRoom;
    }

    public void ClearPlacedRoom()
    {
        placedRoom = null;
        grid.TriggerGridObjectChanged(x, z);
    }

    public bool CanBuild()
    {
        return placedRoom == null;
    }

    public override string ToString()
    {
        return x + " , " + z + ": " + placedRoom;
    }
}
