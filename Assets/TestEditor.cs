using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public sealed class TestEditor : MonoBehaviour {

    [SerializeField]
    internal KeyCode key = KeyCode.A;

}

#if UNITY_EDITOR
[CustomEditor(typeof(TestEditor))]
public sealed class CustomTestEditorEditor : Editor {
    public void OnSceneGUI() {
        var actual = (TestEditor) target;

        // focus on SceneView.  Assumes only 1 open
        if (EditorWindow.mouseOverWindow is SceneView && !(EditorWindow.focusedWindow is SceneView)) {
            EditorWindow.FocusWindowIfItsOpen<SceneView>();
        }

        switch (Event.current.type) {
            case EventType.MouseDown:
                if (keyDown) {
                    Debug.Log("MouseDown WHILE key is held = success (remember to click back on object with this class)");
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out RaycastHit hit, 500f))
                    {
                        Vector3 sideVector = new Vector3(
                            hit.transform.localScale.x * hit.normal.x,
                            hit.transform.localScale.y * hit.normal.y,
                            hit.transform.localScale.z * hit.normal.z);
                        Instantiate(hit.transform.gameObject,
                            hit.transform.position + sideVector, hit.transform.rotation);
                    }
                } else {
                    Debug.Log("MouseDown while key is NOT held (remember to click back on object with this class)");
                }
                break;

            case EventType.KeyDown:
                if (!keyDown && actual.key == Event.current.keyCode) {
                    Debug.Log("KeyDown");
                    Debug.Log("a");
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out RaycastHit hit, 500f))
                    {
                        Vector3 sideVector = new Vector3(
                            hit.transform.localScale.x * hit.normal.x,
                            hit.transform.localScale.y * hit.normal.y,
                            hit.transform.localScale.z * hit.normal.z);
                        Instantiate(hit.transform.gameObject,
                            hit.transform.position + sideVector, hit.transform.rotation);
                    }
                    
                    keyDown = true;
                }
                break;

            case EventType.KeyUp:
                if (keyDown && actual.key == Event.current.keyCode) {
                    Debug.Log("KeyUp");
                    keyDown = false;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out RaycastHit hit, 500f))
                    {
                        Vector3 sideVector = new Vector3(
                            hit.transform.localScale.x * hit.normal.x,
                            hit.transform.localScale.y * hit.normal.y,
                            hit.transform.localScale.z * hit.normal.z);
                        Instantiate(hit.transform.gameObject,
                            hit.transform.position + sideVector, hit.transform.rotation);
                    }
                }
                break;
        }

        var overWindow = EditorWindow.mouseOverWindow;
        var focusedWindow = EditorWindow.focusedWindow;
        Handles.BeginGUI();
        const int NumLines = 4;
        GUI.Box(new Rect(0, 0, 400, 20 * NumLines), string.Format("ACTIVE!\nkeyDown:{0}\nover:{1} (SceneView:{2})\nfocus:{3} (SceneView:{4})", keyDown, overWindow, overWindow is SceneView, focusedWindow, focusedWindow is SceneView));
        Handles.EndGUI();
    }

    private bool keyDown;
}
#endif // UNITY_EDITOR