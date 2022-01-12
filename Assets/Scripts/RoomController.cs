using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    private float _sensitivity;
    private Vector3 _mouseReference;
    private Vector3 _mouseOffset;
    private Vector3 _rotation;
    private bool _isRotating;
    private bool isRoomSelected = false;
    float selectedSpeed = 30;
    public bool rotating = false;

    public RoomSO roomSO;

    public GameObject sensorModel;

    void Start()
    {
        //Set values for manipulation
        _sensitivity = 0.4f;
        _rotation = Vector3.zero;

        //Get Wall GameObject
        GameObject walls = transform.Find("Walls").gameObject;

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
        if (_isRotating)
        {
            // offset
            _mouseOffset = (Input.mousePosition - _mouseReference);

            // apply rotation
            _rotation.y = -(_mouseOffset.x + _mouseOffset.y) * _sensitivity;

            // rotate
            transform.Rotate(_rotation);

            // store mouse
            _mouseReference = Input.mousePosition;
        }

        if (isRoomSelected)
        {
           rotating = true;
           moveToSelected();
        }
        
    }

    private void moveToSelected()
    {
        float step = selectedSpeed * Time.deltaTime;
        Vector3 to = new Vector3(-45, 0, 0);
        transform.parent.position = Vector3.MoveTowards(transform.parent.position, new Vector3(0,3,0), step);
        StartCoroutine(RotateImage(Quaternion.Euler(-45,0,0)));
    }

    IEnumerator RotateImage(Quaternion targetRotation)
    {
        float moveSpeed = .05f;
        while (transform.parent.transform.rotation.y > -45)
        {
            transform.parent.transform.rotation = Quaternion.Lerp(transform.parent.transform.rotation, targetRotation, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.parent.transform.rotation = targetRotation;
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
        isRoomSelected = false;

        float step = selectedSpeed * Time.deltaTime;
        transform.parent.position = Vector3.MoveTowards(transform.parent.position, new Vector3(0, 7, 16), step);
        StartCoroutine(RotateImage(Quaternion.Euler(-90,0,0)));
    }

    void OnMouseUp()
    {
        // rotating flag
        _isRotating = false;
    }
}
