using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NumericInputField : MonoBehaviour
{
    public InputField numberInputField;

    private void Start()
    {
        // Thêm sự kiện lắng nghe sự thay đổi giá trị
        if (numberInputField != null)
        {
            numberInputField.onValueChanged.AddListener(OnNumberValueChanged);
        }
    }

    private void OnNumberValueChanged(string newValue)
    {
        // Kiểm tra giá trị mới và chỉ giữ lại các ký tự số
        string filteredValue = new string(newValue.Where(char.IsDigit).ToArray());

        // Gán giá trị đã lọc lại vào InputField
        numberInputField.text = filteredValue;
    }
}