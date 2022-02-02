using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField] private Text title;

    public GameObject goBackButton;
    public GameObject editButton;

    // Start is called before the first frame update
    void Start()
    {
        title.text = houseName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoBack()
    {
        //pos = 22,63,-32
        //rot = 65,0,0

        CameraController controller = camera.GetComponent<CameraController>();
        controller.MoveCamera(new Vector3(22, 63, -32), Quaternion.Euler(65, 0, 0));
        controller.SetObjectToView(null);
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
