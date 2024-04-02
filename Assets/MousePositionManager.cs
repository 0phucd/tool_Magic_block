using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MousePositionManager : MonoBehaviour
{
    private List<Vector2> mousePositions = new List<Vector2>();
    private string jsonFilePath = "mouse_positions.json";

    void Update()
    {
        // Bấm chuột trái để thêm vị trí vào danh sách
        if (Input.GetMouseButtonDown(0))
        {
            RecordMousePosition();
        }

        // Bấm chuột phải để lưu danh sách vị trí vào file JSON và thoát chương trình
        if (Input.GetMouseButtonDown(1))
        {
            SaveMousePositionsToJson();
            Application.Quit();
        }
    }

    void RecordMousePosition()
    {
        Vector2 mousePosition = Input.mousePosition;
        mousePositions.Add(mousePosition);

        Debug.Log("Recorded mouse position: " + mousePosition);
    }

    void SaveMousePositionsToJson()
    {
        MousePositionListData dataList = new MousePositionListData(mousePositions);

        string jsonData = JsonUtility.ToJson(dataList);
        File.WriteAllText(jsonFilePath, jsonData);

        Debug.Log("Saved mouse positions to JSON: " + jsonData);
    }
}

[System.Serializable]
public class MousePositionListData
{
    public List<Vector2> positions;

    public MousePositionListData(List<Vector2> positions)
    {
        this.positions = positions;
    }
}