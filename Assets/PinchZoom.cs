using UnityEngine;

public class PinchZoom : MonoBehaviour
{
    public float zoomSpeed = 0.5f;
    public float minFOV = 10f;
    public float maxFOV = 60f;

    private void Update()
    {
        // Kiểm tra nếu có ít nhất 2 ngón tay trên màn hình
        if (Input.touchCount >= 2)
        {
            // Lấy thông tin về cảm ứng từ 2 ngón tay
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Lấy vị trí của các ngón tay ở khung hình trước đó
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Tính toán khoảng cách giữa 2 ngón tay ở khung hình trước đó và ở khung hình hiện tại
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Tính toán sự thay đổi khoảng cách giữa 2 ngón tay
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            // Lấy camera chính
            Camera mainCamera = Camera.main;

            // Điều chỉnh trường nhìn (field of view) của camera dựa trên sự thay đổi khoảng cách giữa 2 ngón tay
            mainCamera.fieldOfView += deltaMagnitudeDiff * zoomSpeed;

            // Giới hạn giá trị trường nhìn trong khoảng [minFOV, maxFOV]
            mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView, minFOV, maxFOV);
        }
    }
}