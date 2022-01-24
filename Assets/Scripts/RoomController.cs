using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomController : MonoBehaviour
{
    //Rotate room values
    private float _sensitivity;
    private Vector3 _mouseReference;
    private Vector3 _mouseOffset;
    private Vector3 _rotation;
    public bool _isRotating;
    bool allowRotation = false;

    //Move room values
    public bool isRoomSelected = false;
    public bool hasMoved = false;
    public float moveSpeed = 0.1f;

    public RoomSO roomSO;

    public GameObject walls;
    public GameObject sensorModel;

    public bool graphSet = false;

    void Start()
    {
        //Set values for manipulation
        _sensitivity = 0.4f;
        _rotation = Vector3.zero;

        //Set sensors
        for (int i = 0; i < roomSO.sensors.Count; i++)
        {
            GameObject wall;
            SensorSO sensor = roomSO.sensors[i];
            switch (sensor.wall)
            {
                case 0:
                    wall = walls.transform.GetChild(0).gameObject;
                    GameObject s1 = Instantiate(sensorModel, new Vector3(0f, 0f, -1f), Quaternion.Euler(new Vector3(0f, 0f, 0f)));
                    s1.transform.parent = wall.transform;
                    s1.transform.localPosition = new Vector3(0f, 0f, -1f);
                    s1.GetComponent<SensorController>().sensorSO = sensor;
                    break;
                case 1:
                    wall = walls.transform.GetChild(1).gameObject;
                    GameObject s2 = Instantiate(sensorModel, new Vector3(0f, 0f, -1f), Quaternion.Euler(new Vector3(0f, 0f, 0f)));
                    s2.transform.parent = wall.transform;
                    s2.transform.localPosition = new Vector3(0f, 0f, -1f);
                    s2.GetComponent<SensorController>().sensorSO = sensor;
                    break;
            }
        }

    }

    void Update()
    {
        if (_isRotating && allowRotation == true)
        {
            Debug.Log("Rotating Object");
            // offset
            _mouseOffset = (Input.mousePosition - _mouseReference);

            // apply rotation
            _rotation.y = -(_mouseOffset.x + _mouseOffset.y) * _sensitivity;

            // rotate
            transform.Rotate(_rotation);

            // store mouse
            _mouseReference = Input.mousePosition;
        }

        if (isRoomSelected && hasMoved != true)
        {
            moveToSelected();
        }
        
    }

    private void moveToSelected()
    {
        if (!graphSet)
        {
            StartCoroutine(Test());
        }
        StartCoroutine(MoveRoom(new Vector3(0, 3, 0), Quaternion.Euler(-45,0,0)));
    }

    IEnumerator MoveRoom(Vector3 targetPostition, Quaternion targetRotation)
    {
        Debug.Log("Running");
        while (transform.position != targetPostition && transform.rotation != targetRotation)
        {
            transform.position = Vector3.Lerp(transform.position, targetPostition, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPostition;
        transform.rotation = targetRotation;
        hasMoved = true;
        allowRotation = true;
        //Set Deselect to UI button
        Button button = GameObject.FindWithTag("BackButton").GetComponent<Button>();
        button.onClick.AddListener(DeselectRoom);
        
        yield return null;
    }

    IEnumerator Test()
    {
        graphSet = true;

        Debug.Log("This is a test");
        List<float> mockData = new List<float>();
        mockData.Add(1.2f);
        mockData.Add(3.9f);
        GameObject.FindWithTag("Graph").GetComponent<PieGraph>().FillGraph(mockData);
        yield return null;
    }

    IEnumerator DeMoveRoom(Vector3 targetPostition, Quaternion targetRotation)
    {
        while (transform.position != targetPostition && transform.rotation != targetRotation)
        {
            transform.position = Vector3.Lerp(transform.position, targetPostition, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPostition;
        transform.rotation = targetRotation;
        hasMoved = false;
        allowRotation = false;
        //Set Deselect to UI button
        Button button = GameObject.FindWithTag("BackButton").GetComponent<Button>();
        button.onClick.AddListener(null);
        yield return null;
    }

    void OnMouseDown()
    {
        // rotating flag
        _isRotating = true;

        // store mouse
        _mouseReference = Input.mousePosition;

        if (!isRoomSelected)
        {
            isRoomSelected = true;
        }

    }
    
    public void DeselectRoom()
    {
        Debug.Log("Deselecting Room");
        isRoomSelected = false;
        StartCoroutine(DeMoveRoom(new Vector3(0, 8, 24), Quaternion.Euler(-90,0,0)));
    }

    void OnMouseUp()
    {
        // rotating flag
        _isRotating = false;
    }
}
