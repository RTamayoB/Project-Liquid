using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    [SerializeField] private RoomSO[] roomSOList;
    [SerializeField] private RoomSO roomSO;
    private GridXZ<GridObj> grid;
    private RoomSO.Dir dir = RoomSO.Dir.Down;

    public int widht = 4;
    public int height = 3;
    public float cellSize = 10f;
    public Vector3 origin = new Vector3(-100,0,200);
    private void Awake()
    {
        grid = new GridXZ<GridObj>(widht, height, cellSize, origin, (GridXZ<GridObj> g, int x, int z) => new GridObj(g, x, z));
    }

    public void Start()
    {
        //Set all Rooms
        for(int x = 0; x < roomSOList.Length; x++)
        {
            InsertRoom(roomSOList[x]);
        }
    }

    private void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            InsertRoom(roomSO);
        }
        
        if (Input.GetMouseButtonDown(1))
        {
            DeleteRoom();
        }
/*
        if (Input.GetKeyDown(KeyCode.R))
        {
            dir = RoomSO.GetNextDir(dir);
            Debug.Log("" + dir);
        }
        */
    }

    public void InsertRoom(RoomSO room)
    {
        //This is used if you put room by mousePosition
        //grid.GetXY(GetMouseWorldPosition(), out int x, out int z);

        int x = room.roomPosition.x;
        int z = room.roomPosition.y;

        List<Vector2Int> gridPositionList = room.GetGridPositionList(room.roomPosition, dir);

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
            Vector3 roomWorldPosition = grid.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();

            RoomObj roomObj = RoomObj.Create(roomWorldPosition, new Vector2Int(x, z), dir, room);

            foreach (Vector2Int gridPosition in gridPositionList)
            {
                grid.GetGridObject(gridPosition.x, gridPosition.y).SetRoomObj(roomObj);
            }
        }
        else
        {
            Debug.Log("Can't build");
        }
    }

    public void DeleteRoom()
    {
        GridObj gridObj = grid.GetGridObject(GetMouseWorldPosition());
        RoomObj roomObj = gridObj.GetRoomObj();
        if (roomObj != null)
        {
            roomObj.DestroySelf();

            List<Vector2Int> gridPositionList = roomObj.GetGridPositionList();
            foreach (Vector2Int gridPosition in gridPositionList)
            {
                grid.GetGridObject(gridPosition.x, gridPosition.y).ClearRoomObj();
            }
        }
    }

    //TODO: Create GetMouseWorldPosition Script
    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        Debug.Log(vec.ToString());
        return vec;
    }

    public static Vector3 GetMouseWorldPositionWithZ()
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
    }

    public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
    }

    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Ray ray = worldCamera.ScreenPointToRay(screenPosition);
        if(Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            Debug.Log("Raycast:" + raycastHit.point);
            return raycastHit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }
}

public class GridObj
{
    private GridXZ<GridObj> grid;
    private int x;
    private int z;
    private RoomObj roomObj;

    public GridObj(GridXZ<GridObj> grid, int x, int z)
    {
        this.grid = grid;
        this.x = x;
        this.z = z;
    }

    public void SetRoomObj(RoomObj roomObj)
    {
        this.roomObj = roomObj;
        grid.TriggerGridObjectChanged(x, z);
    }

    public RoomObj GetRoomObj()
    {
        return roomObj;
    }

    public void ClearRoomObj()
    {
        roomObj = null;
        grid.TriggerGridObjectChanged(x, z);
    }

    public bool CanBuild()
    {
        return roomObj == null;
    }

    public override string ToString()
    {
        return x + " , " + z + ": " + roomObj;
    }
}
