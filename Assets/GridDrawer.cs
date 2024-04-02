using UnityEngine;

public class GridDrawer : MonoBehaviour
{
    Camera mainCamera;
    public int gridSizeX = 10; // Số lượng ô grid theo trục X
    public int gridSizeY = 10; // Số lượng ô grid theo trục Y
    public float spacing = 1f; // Khoảng cách giữa các đường kẻ
    public Material lineMaterial; // Material với cài đặt trong suốt
    public GameObject cube; // Reference đến cube

    void Start()
    {
        PlaceGrid();
        DrawGridLines();
    }

    void PlaceGrid()
    {
        float centerX = gridSizeX * spacing / 2f;
        float centerY = gridSizeY * spacing / 2f;

        float gridHeight = centerY + 1f;
        transform.position = new Vector3(0, 0f, 0);

        cube.transform.position = new Vector3(0, 0f, 0); // Đặt cube ở giữa ô trống
    }

    void DrawGridLines()
    {
        float centerX = gridSizeX * spacing / 2f;
        float centerY = gridSizeY * spacing / 2f;

        for (int x = 0; x <= gridSizeX; x++)
        {
            float xPos = x * spacing - centerX+0.5f;
            DrawLine(new Vector3(xPos, -0.5f, -centerY), new Vector3(xPos, -0.5f, centerY));
        }

        for (int y = 0; y <= gridSizeY; y++)
        {
            float yPos = y * spacing - centerY+0.5f;
            DrawLine(new Vector3(-centerX, -0.5f, yPos), new Vector3(centerX, -0.5f, yPos));
        }
    }

    void DrawLine(Vector3 start, Vector3 end)
    {
        GameObject lineObject = new GameObject("GridLine");
        lineObject.transform.parent = transform;
        
        LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();
        lineRenderer.material = lineMaterial; // Gán Material vào LineRenderer

        lineRenderer.startWidth = 0.1f; // Độ rộng của đường kẻ
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }
}