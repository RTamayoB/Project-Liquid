using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Room", menuName = "ScriptableObjects/Room", order = 1)]
public class RoomSO : ScriptableObject
{
    public string roomName;
    public Image roomIcon;
    public List<SensorSO> sensors;
}
