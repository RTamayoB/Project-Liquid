using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieGraph : MonoBehaviour
{

    public float[] values;
    public Color[] wedgeColors;
    public Image wedgePrefab;

    // Start is called before the first frame update
    void Start()
    {
        MakeGraph();
    }

    void MakeGraph()
    {
        float total = 0f;
        float zRotation = 0f;
        for (int i = 0; i < values.Length; i++)
        {
            total += values[i];
        }

        for (int i = 0; i < values.Length; i++)
        {
            Image newWedge = Instantiate(wedgePrefab) as Image;
            newWedge.transform.SetParent(transform, false);
            newWedge.color = wedgeColors[i];
            newWedge.fillAmount = values[i] / total;
            newWedge.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, zRotation));
            zRotation -= newWedge.fillAmount * 360f;
        }
    }

    public void FillGraph(List<float> data)
    {
        //Delete previous children
        for(int c = 0; c < transform.childCount; c++)
        {
            Destroy(transform.GetChild(c).gameObject);
        } 

        Debug.Log("Filling Graph");
        float total = 0f;
        float zRotation = 0f;

        for (int i = 0; i < data.Count; i++)
        {
            total += data[i];
        }

        for (int i = 0; i < data.Count; i++)
        {
            Debug.Log("Filling " + data[i]);
            Image newWedge = Instantiate(wedgePrefab) as Image;
            newWedge.transform.SetParent(transform, false);
            newWedge.color = wedgeColors[i];
            newWedge.fillAmount = data[i] / total;
            newWedge.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, zRotation));
            zRotation -= newWedge.fillAmount * 360f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
