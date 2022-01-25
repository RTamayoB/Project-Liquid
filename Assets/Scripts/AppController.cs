using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppController : MonoBehaviour
{
    //Current  room viewed
    public RoomSO roomViewed;
    //House name
    public string houseName = "La Baticueva";
    //Owner's name
    //Later on we can add profiles (this is only really useful if we add other types of sensors
    public string owner = "Rafael";
    //Number of house members
    public int members = 6;

    public bool editMode;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
