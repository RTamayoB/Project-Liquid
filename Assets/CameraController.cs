using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetMouseButtonDown(0) && !GameObject.FindWithTag("AppController").GetComponent<AppController>().editMode)
        {
            StartCoroutine(MoveRoom(new Vector3(10, 10, 10), Quaternion.identity));
        }*/
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
        yield return null;
    }

    public void SetCameraToRoom(Vector3 targetPosition)
    {
        if (!GameObject.FindWithTag("AppController").GetComponent<AppController>().editMode)
        {
            Vector3 targetWithOffset = targetPosition + new Vector3(5.5f, 20f, -20f);
            Debug.Log("Target offset: " + targetWithOffset);
            StartCoroutine(MoveRoom(targetWithOffset, Quaternion.Euler(45,0,0)));
        }
    }

    public void MoveCamera(Vector3 targetPosition, Quaternion rotation)
    {
        if (!GameObject.FindWithTag("AppController").GetComponent<AppController>().editMode)
        {
            StartCoroutine(MoveRoom(targetPosition, rotation));
        }
    }
}
