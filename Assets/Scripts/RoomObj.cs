using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomObj : MonoBehaviour
{

    public static RoomObj Create(Vector3 worldPosition, Vector2Int origin, RoomSO.Dir dir, RoomSO roomSO)
    {
        Transform roomObjectTransform = Instantiate(roomSO.prefab, worldPosition, Quaternion.Euler(0, roomSO.GetRotationAngle(dir), 0)).transform;
        RoomObj roomObj = roomObjectTransform.GetComponent<RoomObj>();

        roomObj.roomSO = roomSO;
        roomObj.origin = origin;
        roomObj.dir = dir;

        return roomObj;
    }

    private RoomSO roomSO;
    private Vector2Int origin;
    private RoomSO.Dir dir;

    public List<Vector2Int> GetGridPositionList()
    {
        return roomSO.GetGridPositionList(origin, dir);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
