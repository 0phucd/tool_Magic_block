using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideUi : MonoBehaviour
{
    public GameObject duplicateZone;
    private bool isMoved = false;

    public GameObject cameraZone;
    private bool isMovedCamera = false;

    public GameObject createZone;
    private bool isMovedCreate = false;
    public void MoveDuplicateZone()
    {
        if (!isMoved)
        {
            if (duplicateZone != null)
            {
                RectTransform rectTransform = duplicateZone.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    rectTransform.anchoredPosition = new Vector2(1628f, -1015f);
                }
            }
            isMoved = true;
        }
        else
        {
            if (duplicateZone != null)
            {
                RectTransform rectTransform = duplicateZone.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    rectTransform.anchoredPosition = new Vector2(1628f, 758f);
                }
            }
            isMoved = false;
        }
    }
    public void MoveCameraZone()
    {
        if (!isMovedCamera)
        {
            if (cameraZone != null)
            {
                RectTransform rectTransform = cameraZone.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    rectTransform.anchoredPosition = new Vector2(1628f, -1015f);
                }
            }
            isMovedCamera = true;
        }
        else
        {
            if (cameraZone != null)
            {
                RectTransform rectTransform = cameraZone.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    rectTransform.anchoredPosition = new Vector2(1628f, 758f);
                }
            }
            isMovedCamera = false;
        }
    }
    
}
