using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorController : MonoBehaviour
{
    public SensorSO sensorSO;
    private SensorActivity sensorActivity;

    // Start is called before the first frame update
    void Start()
    {
        sensorActivity = GameObject.Find("SensorActivity").GetComponent<SensorActivity>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            sensorActivity.FillActivity(sensorSO);
        }
    }
}
