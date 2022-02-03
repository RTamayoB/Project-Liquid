using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensorActivity : MonoBehaviour
{
    [SerializeField] GameObject sensorName;
    private Text sensorText;
    [SerializeField] private GameObject activity;

    private void Start()
    {
        sensorText = sensorName.GetComponent<Text>();
    }

    public void FillActivity(SensorSO sensorSO)
    {
        sensorName.SetActive(true);
        sensorText.text = sensorSO.sensorName;
        //Delete previous children
        for (int c = 0; c < transform.childCount; c++)
        {
            if (transform.GetChild(c).CompareTag("Activity"))
            {
                Destroy(transform.GetChild(c).gameObject);
            }
        }
        for (int s = 0; s < sensorSO.dailyLog.Count; s++)
        {
            Vector3 position = Vector3.zero;
            GameObject newActivity = Instantiate(activity);
            newActivity.transform.SetParent(transform, false);
            newActivity.transform.localPosition = position;
            position += new Vector3(0, -30, 0);
            newActivity.transform.GetChild(0).GetComponent<Text>().text = "Day "+ (s+1)+":";
            newActivity.transform.GetChild(1).GetComponent<Text>().text = sensorSO.dailyLog[s] + " Lts.";
        }
    }
}
