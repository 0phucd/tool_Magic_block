using UnityEngine;

public class DestroyOnClick : MonoBehaviour
{
    void Update()
    {
        // Check if the mouse button is clicked
        if (Input.GetMouseButtonDown(0))
        {
            // Create a ray from the camera to the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Check if the ray hits the object
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // Check if the clicked object is the tick block
                if (hit.collider.gameObject == gameObject)
                {
                    // Destroy the tick block when clicked
                    Destroy(gameObject);
                }
            }
        }
    }
}