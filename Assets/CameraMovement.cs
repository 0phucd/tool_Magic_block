using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target; // The target object to orbit around
    public float rotationSpeed = 5f;

    void Update()
    {
        if (target == null)
        {
            Debug.LogError("Target object not assigned for camera orbit.");
            return;
        }
        if (Input.GetMouseButton(0)) // Right mouse button
        {
            OrbitCamera();
        }
        // Check for user input to orbit the camera
       
    }

    void OrbitCamera()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Rotate the camera around the target based on mouse input
        transform.RotateAround(target.position, Vector3.up, mouseX * rotationSpeed);
        transform.RotateAround(target.position, transform.right, -mouseY * rotationSpeed);
    }
}