using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjExporter : MonoBehaviour
{
    // Chỉ định đường dẫn và tên tệp .obj
    public string objFilePath = "Assets/ExportedModel123.obj";

    void Update()
    {
        // Kiểm tra nút Enter được nhấn
        if (Input.GetKeyDown(KeyCode.A))
        {
            ExportToObj();
        }
    }

    void ExportToObj()
    {
        // Lấy danh sách các block hoặc đối tượng cần xuất ra .obj
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");

        // Mở tệp .obj để ghi
        System.IO.StreamWriter writer = new System.IO.StreamWriter(objFilePath);

        foreach (GameObject block in blocks)
        {
            // Lấy vị trí của block
            Vector3 position = block.transform.position;

            // Ghi định dạng vị trí vào tệp .obj
            string line = string.Format("v {0} {1} {2}", position.x, position.y, position.z);
            writer.WriteLine(line);
        }

        // Ghi thông tin về các mặt (faces)
        for (int i = 0; i < blocks.Length; i += 4)  // Giả sử mỗi block là một hình vuông, và ta có 4 đỉnh
        {
            string faceLine = string.Format("f {0} {1} {2} {3}", i + 1, i + 2, i + 3, i + 4);
            writer.WriteLine(faceLine);
        }

        // Đóng tệp .obj
        writer.Close();

        Debug.Log("Exported to " + objFilePath);
    }
}