using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppController : MonoBehaviour
{
    public RoomSO roomViewed;
    public string houseName = "Casa de Rafael";
    public string owner = "Rafael";
    public int members = 6;
    public bool editMode;

    [SerializeField] public Text title;

    public GameObject camera;
    private CameraController cameraController;

    public PieGraph pieGraph;

    public GameObject goBackButton;
    public GameObject editButton;
    public Image editButtonImage;
    public GameObject goLeftButton;
    public GameObject goRightButton;


    // Start is called before the first frame update
    void Start()
    {
        title.text = houseName;
        cameraController = camera.GetComponent<CameraController>();
        editButtonImage = editButton.GetComponent<Image>();
    }

    public void GoBack()
    {
        //pos = 22,63,-32
        //rot = 65,0,0
        goLeftButton.SetActive(false);
        goRightButton.SetActive(false);
        goBackButton.SetActive(false);
        pieGraph.MakeGraph();
        cameraController.MoveCamera(new Vector3(22, 63, -32), Quaternion.Euler(65, 0, 0));
        cameraController.SetObjectToView(null);
        roomViewed = null;
        title.text = houseName;
        
    }

    public void Edit()
    {
        switch (editMode)
        {
            case true:
                editMode = false;
                editButtonImage.color = Color.white;
                break;
            case false:
                editMode = true;
                editButtonImage.color = Color.green;
                break;
        } 
    }

    public void GoRight()
    {
        cameraController.RotateArround(camera.gameObject, -Vector3.up, 90, 1f);
    }

    public void GoLeft()
    {
        cameraController.RotateArround(camera.gameObject, Vector3.up, 90, 1f);
    }

}
