using System;
using UnityEngine;

public class ZoomInZoomOut : MonoBehaviour
{
    Camera mainCamera;

    [SerializeField] float zoomModifierSpeed = 1f;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
    }

    void Update()
    {
        float scrollDelta = Input.GetAxis("Mouse ScrollWheel");

        if (scrollDelta != 0)
        {
            // Điều chỉnh field of view dựa trên hướng cuộn chuột và tốc độ zoom
            mainCamera.fieldOfView -= scrollDelta * zoomModifierSpeed;

            // Giới hạn giá trị field of view trong khoảng từ 60 đến 120 (hoặc giới hạn tùy ý)
            mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView, 20f, 150f);
        }
    }
}