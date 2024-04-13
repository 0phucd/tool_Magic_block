using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inputMoblie : MonoBehaviour
{
    Camera mainCamera;

    float touchesPrevPosDifference, touchesCurPosDifference, zoomModifier;

    Vector2 firstTouchPrevPos, secondTouchPrevPos;

    [SerializeField] float zoomModifierSpeed = 1f;
    Vector3 lastMousePosition;

    // Use this for initialization
    void Start()
    {
        mainCamera = GetComponent<Camera>();

    }



    // Update is called once per frame
    void Update()
    {
        

        if (Input.touchCount == 2)
        {
            Touch firstTouch = Input.GetTouch(0);
            Touch secondTouch = Input.GetTouch(1);

            firstTouchPrevPos = firstTouch.position - firstTouch.deltaPosition;
            secondTouchPrevPos = secondTouch.position - secondTouch.deltaPosition;
            touchesPrevPosDifference = (firstTouchPrevPos - secondTouchPrevPos).magnitude;
            touchesCurPosDifference = (firstTouch.position - secondTouch.position).magnitude;

            zoomModifier = (firstTouch.deltaPosition - secondTouch.deltaPosition).magnitude * zoomModifierSpeed;

            if (touchesPrevPosDifference > touchesCurPosDifference)
            {

                if (mainCamera.fieldOfView < 120)
                {
                    mainCamera.fieldOfView += zoomModifier;
                }
                else
                {
                    mainCamera.fieldOfView = 120;
                }

            }

            if (touchesPrevPosDifference < touchesCurPosDifference)
            {
                if (mainCamera.fieldOfView > 60)
                {
                    mainCamera.fieldOfView -= zoomModifier;
                }
                else
                {
                    mainCamera.fieldOfView = 60;
                }
            }
            

        }
    }
}