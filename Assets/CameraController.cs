using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 0.5f;
    private Transform camera;
    [SerializeField] private bool zoomOnRoom = false;
    [SerializeField] private Transform objectToView;
    [SerializeField] private List<WallController> currentlyInTheWay;
    [SerializeField] private List<WallController> alreadyTransparent;

    private void Awake()
    {
        currentlyInTheWay = new List<WallController>();
        alreadyTransparent = new List<WallController>();
        camera = this.gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (zoomOnRoom && objectToView != null)
        {
            Debug.Log("Zooming");
            GetObjectsInTheWay();

            MakeObjectsSolid();
            MakeObjectsTransparent();
        }
        else
        {
            Debug.Log("Not Zooming");
            GetObjectsInTheWay();
            MakeAllSolid();
        }
    }

    internal void SetObjectToView(Transform? toView)
    {
        objectToView = (Transform)toView;
    }

    private void GetObjectsInTheWay()
    {
        currentlyInTheWay.Clear();

        float cameraToRoomDistance = Vector3.Magnitude(camera.position - objectToView.position);

        Ray rayFoward = new Ray(camera.position, objectToView.position - camera.position);
        Ray rayBackward = new Ray(objectToView.position, camera.position - objectToView.position);

        var hitsFoward = Physics.RaycastAll(rayFoward, cameraToRoomDistance);
        var hitsBackward = Physics.RaycastAll(rayBackward, cameraToRoomDistance);

        foreach(var hit in hitsFoward)
        {
            if (hit.collider.gameObject.TryGetComponent(out WallController wallController))
            {
                if (!currentlyInTheWay.Contains(wallController))
                {
                    currentlyInTheWay.Add(wallController);
                }
            }
        }

        foreach (var hit in hitsBackward)
        {
            if (hit.collider.gameObject.TryGetComponent(out WallController wallController))
            {
                if (!currentlyInTheWay.Contains(wallController))
                {
                    currentlyInTheWay.Add(wallController);
                }
            }
        }
    }

    private void MakeObjectsTransparent()
    {
        for(int i = 0; i < currentlyInTheWay.Count; i++)
        {
            WallController wallController = currentlyInTheWay[i];

            if (!alreadyTransparent.Contains(wallController))
            {
                wallController.MakeTransparent();
                alreadyTransparent.Add(wallController);
            }
        }
    }

    private void MakeObjectsSolid()
    {
        for (int i = alreadyTransparent.Count -1; i >= 0; i--)
        {
            WallController wallController = alreadyTransparent[i];

            if (!currentlyInTheWay.Contains(wallController))
            {
                wallController.MakeSolid();
                alreadyTransparent.Remove(wallController);
            }
        }
    }

    private void MakeAllSolid()
    {
        for (int i = 0; i <= alreadyTransparent.Count; i++)
        {
            WallController wallController = alreadyTransparent[i];

            wallController.MakeSolid();
            alreadyTransparent.Remove(wallController);
        }
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
            zoomOnRoom = true;
            Vector3 targetWithOffset = targetPosition + new Vector3(5.5f, 8.8f, -15f);
            Debug.Log("Target offset: " + targetWithOffset);
            StartCoroutine(MoveRoom(targetWithOffset, Quaternion.Euler(30,0,0)));
        }
    }

    public void MoveCamera(Vector3 targetPosition, Quaternion rotation)
    {
        if (!GameObject.FindWithTag("AppController").GetComponent<AppController>().editMode)
        {
            zoomOnRoom = false;
            StartCoroutine(MoveRoom(targetPosition, rotation));
        }
    }
}
