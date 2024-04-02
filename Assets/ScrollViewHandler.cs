using UnityEngine;
using UnityEngine.EventSystems;

public class ScrollViewHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public CameraController cameraController; // Kéo và thả CameraController từ Inspector

    public void OnPointerDown(PointerEventData eventData)
    {
        cameraController.OnScrollViewTouchStart();
        Debug.Log("pk");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        cameraController.OnScrollViewTouchEnd();
    }
}