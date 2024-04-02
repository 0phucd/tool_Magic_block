using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LongPressButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private float longPressDuration = 0.2f;
    private bool isPointerDown = false;
    private float pointerDownTime = 0f;
    private bool hasPerformedLongPress = false;

    public GameObject tutorial;

    // Hàm sự kiện khi nhấn nút
    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true;
        pointerDownTime = Time.time;
        hasPerformedLongPress = false;
        Invoke("CheckLongPress", longPressDuration);
    }

    // Hàm kiểm tra long press
    private void CheckLongPress()
    {
        if (isPointerDown && !hasPerformedLongPress && tutorial != null)
        {
            tutorial.SetActive(true);
            hasPerformedLongPress = true;
            Debug.Log("Long Press Detected!");
        }
    }

    // Hàm sự kiện khi nhả nút
    public void OnPointerUp(PointerEventData eventData)
    {
        isPointerDown = false;

        // Ẩn tutorial nếu đã thực hiện long press
        if (hasPerformedLongPress && tutorial != null)
        {
            tutorial.SetActive(false);
        }
    }
}