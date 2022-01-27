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

    public GameObject camera;

    public bool editMode;

    public GameObject goBackButton;
    public GameObject editButton;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoBack()
    {
        //pos = 22,63,-32
        //rot = 65,0,0

        camera.GetComponent<CameraController>().MoveCamera(new Vector3(22, 63, -32), Quaternion.Euler(65, 0, 0));
        roomViewed = null;
    }

    public void Edit()
    {
        switch (editMode)
        {
            case true:
                editMode = false;
                break;
            case false:
                editMode = true;
                break;
        } 
    }
}
