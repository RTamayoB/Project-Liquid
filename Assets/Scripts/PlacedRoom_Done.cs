using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedRoom_Done : MonoBehaviour
{

    public static PlacedRoom_Done Create(Vector3 worldPosition, Vector2Int origin, RoomSO.Dir dir, RoomSO roomSO)
    {
        Transform placedRoomTransform = Instantiate(roomSO.prefab, worldPosition, Quaternion.Euler(0, roomSO.GetRotationAngle(dir), 0)).transform;
        PlacedRoom_Done placedRoom = placedRoomTransform.GetComponent<PlacedRoom_Done>();
        placedRoom.Setup(roomSO, origin, dir);

        return placedRoom;
    }

    private RoomSO roomSO;
    private Vector2Int origin;
    private RoomSO.Dir dir;

    private void Setup(RoomSO roomSO, Vector2Int origin, RoomSO.Dir dir)
    {
        this.roomSO = roomSO;
        this.origin = origin;
        this.dir = dir;
    }

    public RoomSO GetRoomSO()
    {
        return roomSO;
    }

    public List<Vector2Int> GetGridPositionList()
    {
        return roomSO.GetGridPositionList(origin, dir);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public override string ToString()
    {
        return roomSO.roomName;
    }
}
