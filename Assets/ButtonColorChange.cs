using UnityEngine;
using UnityEngine.UI;

public class ButtonColorChange : MonoBehaviour
{
    public Button[] buttons;

    private Color selectedColor = new Color(0.16f, 0.98f, 0.58f);
    private Color defaultColor = Color.white;
    public Button[] otherButtons;

    void Start()
    {
        // Gán sự kiện cho mỗi button
        for (int i = 0; i < buttons.Length; i++)
        {
            int buttonIndex = i; // Đảm bảo giữ giá trị đúng của biến i
            buttons[i].onClick.AddListener(() => OnButtonClick(buttonIndex));
        }

        for (int i = 0; i < otherButtons.Length; i++)
        {
            int buttonIndex = i; // Đảm bảo giữ giá trị đúng của biến i
            otherButtons[i].onClick.AddListener(() => OnOtherButtonClick(buttonIndex));
        }
        // Thiết lập màu mặc định cho tất cả các button
        SetDefaultColors();
        SetDefaultOtherColors();
    }

    void OnButtonClick(int selectedIndex)
    {
        // Đặt màu cho button được chọn
        buttons[selectedIndex].image.color = selectedColor;

        // Đặt màu mặc định cho tất cả các button khác
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i != selectedIndex)
            {
                buttons[i].image.color = defaultColor;
            }
        }
    }

    void SetDefaultColors()
    {
        // Đặt màu mặc định cho tất cả các button
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].image.color = (i == 1) ?  selectedColor: defaultColor;
        }
    }
    void OnOtherButtonClick(int selectedIndex)
    {
        // Đặt màu cho button được chọn trong mảng otherButtons
        otherButtons[selectedIndex].image.color = selectedColor;

        // Đặt màu mặc định cho tất cả các button khác trong mảng otherButtons
        for (int i = 0; i < otherButtons.Length; i++)
        {
            if (i != selectedIndex)
            {
                otherButtons[i].image.color = defaultColor;
            }
        }
    }
    void SetDefaultOtherColors()
    {
        // Đặt màu mặc định cho tất cả các button
        for (int i = 0; i < buttons.Length; i++)
        {
            otherButtons[i].image.color = (i == 0) ?  selectedColor: defaultColor;
        }
    }
}