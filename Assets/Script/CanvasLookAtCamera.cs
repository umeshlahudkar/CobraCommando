using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasLookAtCamera : MonoBehaviour
{
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // Calculate the direction from the canvas to the camera
        Vector3 directionToCamera = mainCamera.transform.position - transform.position;

        // Use LookAt to make the canvas face the camera, but only rotate on the Y-axis
        transform.LookAt(transform.position + directionToCamera, Vector3.up);
    }
}
