using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float panSpeed = 10f;
    public float rolateSpeed = 40f;

    void Update()
    {
        if (Input.GetMouseButton(0)) // right mouse button
        {
            var newPosition = new Vector3();
            newPosition.x = Input.GetAxis("Mouse X") * panSpeed * Time.deltaTime;
            newPosition.y = Input.GetAxis("Mouse Y") * panSpeed * Time.deltaTime;
            // translates to the opposite direction of mouse position.
            transform.Translate(-newPosition);
        }
        float rotation = 0f;
       
        transform.Rotate(Vector3.forward * rotation);
        float zMove = 0f;
        Vector3 currentPosition = transform.position;
        if (Input.GetKey(KeyCode.W))
        {
            float newY = currentPosition.y + rolateSpeed * Time.deltaTime;
            transform.position = new Vector3(currentPosition.x, newY, currentPosition.z);

        }
        if (Input.GetKey(KeyCode.S))
        {
            float newY = currentPosition.y - rolateSpeed * Time.deltaTime;
            transform.position = new Vector3(currentPosition.x, newY, currentPosition.z);

        }
        

    }
    }
    

