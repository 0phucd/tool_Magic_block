using UnityEngine;

public class ExampleScript : MonoBehaviour
{
    public bool runInEditMode;

    void Update()
    {
        if (runInEditMode)
        {
            // Code to run in Edit mode
            Debug.Log("Running in Edit mode");
        }
        else
        {
            // Code to run only in Play mode
            Debug.Log("Running in Play mode");
        }
    }
}