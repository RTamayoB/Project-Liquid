using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppController : MonoBehaviour
{
    //Set AppRoomGrid
    public Vector3[,] grid = new Vector3[3,4];

    //Current  room viewed
    public GameObject currentRoom;
    //House name
    public string houseName = "La Baticueva";
    //Owner's name
    //Later on we can add profiles (this is only really useful if we add other types of sensors
    public string owner = "Rafael";
    //Number of house members
    public int members = 6;
    //List of rooms
    public List<RoomSO> rooms;

    //House Model
    public GameObject houseModel;
    public GameObject roomTopView;

    // Start is called before the first frame update
    void Start()
    {
        //TODO: Get all rooms
        //Instantiate all rooms
        Debug.Log("Called Start");
        for (int i = 0; i < rooms.Count; i++)
        {
            GameObject newRoom = Instantiate(houseModel, new Vector3(0f, 8f, 24f), Quaternion.Euler(new Vector3(-90f, 0f, 0f)));
            newRoom.GetComponentInChildren<RoomController>().roomSO = rooms[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
