using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    
    public float panSpeed = 1f;
    public float scrollSpeed = 1.0f;
    public float rotationSpeed = 1f;
    public Transform camPos;
    private bool isScrollingY = true;
    private bool isScrollingX = false;
    private bool isScrollingRolateX = false;
    private bool isScrollingRolateY = true;
    public Transform target; 
    public float rolateSpeed = 40f;
    private float pinchZoomSpeed = 5.0f;
    private float minZoomDistance = 5.0f;
    private float maxZoomDistance = 20.0f;
    private bool isTouchingScrollView = false;
    private float initialPinchDistance;
    public ScrollRect scrollView; 
    private bool isScrollingScrollView = false;
    private bool srcollX = false;
    private bool srcollY = false;
    private bool srcollZ = false;
    private Camera mainCamera;
    public float preCamX10;
    public float preCamX_10;
    public float preCamY10;
    public float preCamY_10;
    public float preCamZ10;
    public float preCamZ_10;
      void Update()
    {
        if (IsPointerOverUI())
        {
            return; // Skip camera movement when interacting with UI
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            // Prevent camera movement when scrolling a scroll view
            return;
        }

        if (isTouchingScrollView)
        {
            // ScrollView is being touched, don't process camera movement
            return;
        }

        if (isScrollingRolateX)
        {
            if (Input.GetMouseButton(0)) // Kiểm tra nút chuột trái được nhấn
            {
                // Di chuyển camera khi giữ nút chuột trái
                CameraMoveWhileLeftMouseButtonHeld();
            }
            float rotation = 0f;
            transform.Rotate(Vector3.forward * rotation);

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

        if (isScrollingRolateY)
        {
            if (target == null)
            {
                return;
            }

            if (Input.GetMouseButton(0)) // Right mouse button
            {
                OrbitCamera();
            }
        }

        // Update previous camera position for orthographic view
        UpdatePreviousCameraPosition();
    }

    void UpdatePreviousCameraPosition()
    {
        float camX = camPos.position.x;
        float camY = camPos.position.y;
        float camZ = camPos.position.z;

        if (camX >= 7)
        {
            preCamX10 = camPos.position.x;
        }
        if (camX <= -7)
        {
            preCamX_10 = camPos.position.x;
        }
        if (camY >= 10)
        {
            preCamY10 = camPos.position.x;
        }
        if (camY <= -10)
        {
            preCamY_10 = camPos.position.x;
        }
        if (camZ >= 7)
        {
            preCamZ10 = camPos.position.x;
        }
        if (camZ <= -7)
        {
            preCamZ_10 = camPos.position.x;
        }
    }

    void OrbitCamera()
    {
        float touchX = Input.GetAxis("Mouse X");
        float touchY = Input.GetAxis("Mouse Y");

        // Rotate the camera around the target based on mouse input
        transform.RotateAround(target.position, Vector3.up, touchX * rotationSpeed);
        transform.RotateAround(target.position, transform.right, -touchY * rotationSpeed);
    }
    public void SetOrthographicViewX()
    {
        srcollX = !srcollX;
        if (srcollX)
        {
           SetOrthographicViewX1();
        }
        else
        {
            SetOrthographicViewX2();
        }
        
    }  
    public void SetOrthographicViewY()
    {
        srcollY = !srcollY;
        if (srcollY)
        {
            SetOrthographicViewY1();
        }
        else
        {
            SetOrthographicViewY2();
        }
        
    }  
    public void SetOrthographicViewZ()
    {
        srcollZ = !srcollZ;
        if (srcollZ)
        {
            SetOrthographicViewZ1();
        }
        else
        {
            SetOrthographicViewZ2();
        }
        
    }  
    public void SetOrthographicViewX1()
    {
        float camY = camPos.position.y;
        if (camY >= 0)
        {
            transform.rotation = Quaternion.Euler(90.0f, 0, 0); // Xoay camera vuông góc với trục X
            Vector3 newPosition = new Vector3(camPos.position.x, camPos.position.y + 15, camPos.position.z);
            transform.position = newPosition;
        }
        else
        {
            transform.rotation = Quaternion.Euler(90.0f, 0, 0); // Xoay camera vuông góc với trục Y
            Vector3 newPosition = new Vector3(camPos.position.x,  preCamY10+ 15, camPos.position.z);
            transform.position = newPosition;
        }
        
    }         
    public void SetOrthographicViewX2()
    {
        float camY = camPos.position.y;
        if (camY < 0)
        {
            transform.rotation = Quaternion.Euler(-90.0f, 0, 0); // Xoay camera vuông góc với trục X
            Vector3 newPosition = new Vector3(camPos.position.x, camPos.position.y - 15, camPos.position.z);
            transform.position = newPosition;
        }
        else
        {
            transform.rotation = Quaternion.Euler(-90.0f, 0, 0); // Xoay camera vuông góc với trục Y
            Vector3 newPosition = new Vector3(camPos.position.x,  preCamY_10-15, camPos.position.z);
            transform.position = newPosition;
        }
        
    }   

    // Hàm xem mặt phẳng vuông góc với trục Y
    public void SetOrthographicViewY1()
    {
        float camZ = camPos.position.z;
        if (camZ <= 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0); // Xoay camera vuông góc với trục Y
            Vector3 newPosition = new Vector3(camPos.position.x, camPos.position.y, camPos.position.z - 10);
            transform.position = newPosition;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0); // Xoay camera vuông góc với trục Y
            Vector3 newPosition = new Vector3(camPos.position.x, camPos.position.y,  preCamZ_10- 10);
            transform.position = newPosition;
        }
    }
    public void SetOrthographicViewY2()
    {
        float camZ = camPos.position.z;
        if (camZ >= 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0); // Xoay camera vuông góc với trục Y
            Vector3 newPosition = new Vector3(camPos.position.x, camPos.position.y, camPos.position.z + 10);
            transform.position = newPosition;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0); // Xoay camera vuông góc với trục Y
            Vector3 newPosition = new Vector3(camPos.position.x, camPos.position.y,   preCamZ10+ 10);
            transform.position = newPosition;
        }
    }


    // Hàm xem mặt phẳng vuông góc với trục Z
    public void SetOrthographicViewZ1()
    {
        float camX = camPos.position.x;
   
        if (camX < 0)
        {
            transform.rotation = Quaternion.Euler(0, 90.0f, 0); // Xoay camera vuông góc với trục Z
            Vector3 newPosition = new Vector3(camPos.position.x - 10, camPos.position.y, camPos.position.z);
            transform.position = newPosition;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 90.0f, 0); // Xoay camera vuông góc với trục Y
            Vector3 newPosition = new Vector3(preCamX_10- 10, camPos.position.y, camPos.position.z);
            transform.position = newPosition;
            
        }
    }
    public void SetOrthographicViewZ2()
    {
        float camX = camPos.position.x;
        
        if (camX >= 0)
        {
            transform.rotation = Quaternion.Euler(0, -90.0f, 0); // Xoay camera vuông góc với trục Z
            Vector3 newPosition = new Vector3(camPos.position.x + 10, camPos.position.y, camPos.position.z);
            transform.position = newPosition;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, -90.0f, 0); // Xoay camera vuông góc với trục Y
            Vector3 newPosition = new Vector3(preCamX10 +10, camPos.position.y, camPos.position.z);
            transform.position = newPosition;
        }
    }
    
    public void ToggleScrollingY()
    { 
        isScrollingY = true; 
         isScrollingX = false;

        
       
    }
    public void ToggleScrollingRolateX()
    { 
       
        isScrollingRolateX = true;
        isScrollingRolateY = false;
       
    }
    public void ToggleScrollingRolateY()
    { 
      
        isScrollingRolateX = false;
        isScrollingRolateY = true;
       
    }
    public void ToggleScrollX()
    { 
        isScrollingY = false; 
        isScrollingX = true;
      
       
    }
    public void SetCameraRotationX90()
    {
        
        transform.rotation = Quaternion.Euler(90.0f, 0, 0);
        Vector3 newPosition = new Vector3(camPos.position.x, camPos.position.y + 15, camPos.position.z);
        transform.position = newPosition;

    }
    public void CameraMobileMoveLeft()
    {
        float moveAmount = -scrollSpeed; // Negative value to move left

        if (isScrollingX)
        {
            Vector3 currentPosition = transform.position;
            currentPosition.x += moveAmount;
            transform.position = currentPosition;
        }

        if (isScrollingY)
        {
            Vector3 currentPosition = transform.position;
            currentPosition.y += moveAmount; // Update Y position for moving upwards
            transform.position = currentPosition;
        }
        
    }
    public void CameraMobileMoveRight()
    {
        float moveAmount = scrollSpeed; // Negative value to move left

        if (isScrollingX)
        {
            Vector3 currentPosition = transform.position;
            currentPosition.x += moveAmount;
            transform.position = currentPosition;
        }

        if (isScrollingY)
        {
            Vector3 currentPosition = transform.position;
            currentPosition.y += moveAmount; // Update Y position for moving upwards
            transform.position = currentPosition;
        }
        
    }
    
  

    // Thêm phương thức để xử lý sự kiện khi ScrollView không còn được chạm vào
 
    public void OnScrollViewTouchStart()
    {
        isTouchingScrollView = true;
    }

    public void OnScrollViewTouchEnd()
    {
        isTouchingScrollView = false;
    }

    bool IsPointerOverUI()
    {
        if (Input.touchCount > 0)
        {
            // Check if the touch is over any UI element
            Touch touch = Input.GetTouch(0);
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = touch.position;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            return results.Count > 0;
        }
        else
        {
            // Check if the mouse is over any UI element
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            return results.Count > 0;
        }
    }
    void CameraMoveWhileLeftMouseButtonHeld()
    {
        // Kiểm tra nếu nút chuột trái được giữ và di chuyển camera
        if (Input.GetMouseButton(0))
        {
            // Lấy giá trị di chuyển chuột theo trục X và Y
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // Tính toán vị trí mới cho camera
            Vector3 newPosition = transform.position;
            newPosition.x += mouseX * panSpeed * Time.deltaTime;
            newPosition.z += mouseY * panSpeed * Time.deltaTime;

            // Áp dụng vị trí mới cho camera
            transform.position = newPosition;
        }
    }
}