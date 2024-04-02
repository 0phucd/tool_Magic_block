using UnityEngine;
using UnityEngine.EventSystems;

public class ColorChartPicker : MonoBehaviour, IPointerClickHandler
{
    public ColorPicker colorPicker;

    public void OnPointerClick(PointerEventData eventData)
    {
        Vector2 localCursor;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform as RectTransform, eventData.position, eventData.pressEventCamera, out localCursor
        );

        // Chuyển đổi vị trí thành tọa độ màu (0-1)
        float x = (localCursor.x + (transform as RectTransform).rect.width * 0.5f) / (transform as RectTransform).rect.width;
        float y = (localCursor.y + (transform as RectTransform).rect.height * 0.5f) / (transform as RectTransform).rect.height;

        colorPicker.PickColorFromChart(new Vector2(x, y));
    }
}