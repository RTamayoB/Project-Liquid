using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Sensor", menuName = "ScriptableObjects/Sensor", order = 2)]
public class SensorSO : ScriptableObject
{
    public string sensorName;
    public Image sensorIcon;
    public RoomSO room;
    //Wall the sensor is in
    public int wall;
    //Exact position the sensor is in
    public Vector3 wallPosition;
    //rn is liters
    public float sensorValue;
    public List<float> dailyLog;
}
