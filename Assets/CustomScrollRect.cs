using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CustomScrollRect : ScrollRect
{
    public bool isDragging = false;

    public override void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        base.OnBeginDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        base.OnEndDrag(eventData);
    }
}