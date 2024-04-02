using System.IO;
using UnityEngine;
using UnityEngine.UI;
using AnotherFileBrowser.Windows;

public class Import3D : MonoBehaviour
{
    public Button importButton; // Button để kích hoạt quá trình nhập
    public SpawnCube button;

    void Start()
    {
        // Gắn hàm xử lý sự kiện cho Button
        importButton.onClick.AddListener(ImportGLBFile);
    }

    void ImportGLBFile()
    {
        // Tạo một đối tượng BrowserProperties để cấu hình hộp thoại tệp
        var bp = new BrowserProperties();
        bp.filter = "GLB files (*.glb)|*.glb";
        bp.filterIndex = 0;

        // Mở hộp thoại tệp bằng AnotherFileBrowser
        new FileBrowser().OpenFileBrowser(bp, filePath =>
        {
            // Kiểm tra nếu người dùng đã chọn một file
            if (!string.IsNullOrEmpty(filePath))
            {
                // Đường dẫn đích trong Persistent Data Path
                string destinationPath = Path.Combine(Application.persistentDataPath, Path.GetFileName(filePath));

                // Copy file vào Persistent Data Path
                File.Copy(filePath, destinationPath);

                // Log thông báo thành công
                Debug.Log("GLB file imported successfully: " + destinationPath);

                // Gọi hàm để tạo các buttons
                button.CreateButtons();
            }
        });
    }
}
