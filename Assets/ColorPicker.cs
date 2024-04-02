using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour
{
    public Slider redSlider;
    public Slider greenSlider;
    public Slider blueSlider;
    public Image colorDisplay;
    public InputField hexInputField;
    public RawImage colorChartImage; // Thêm RawImage cho bảng màu

    private Texture2D colorChartTexture;

    private void Start()
    {
        redSlider.onValueChanged.AddListener(UpdateColor);
        greenSlider.onValueChanged.AddListener(UpdateColor);
        blueSlider.onValueChanged.AddListener(UpdateColor);

        hexInputField.onEndEdit.AddListener(UpdateColorFromHex);

        colorChartTexture = colorChartImage.texture as Texture2D;

        UpdateColor(0);
    }

    private void UpdateColor(float value)
    {
        float red = redSlider.value;
        float green = greenSlider.value;
        float blue = blueSlider.value;
        Color selectedColor = new Color(red, green, blue);

        colorDisplay.color = selectedColor;
        hexInputField.text = ColorToHex(selectedColor);
    }

    private void UpdateColorFromHex(string hexCode)
    {
        Color color = HexToColor(hexCode);
        redSlider.value = color.r;
        greenSlider.value = color.g;
        blueSlider.value = color.b;
        colorDisplay.color = color;
    }

    // Lấy màu từ bảng màu khi người dùng chọn
    public void PickColorFromChart(Vector2 position)
    {
        position.x = Mathf.Clamp01(position.x);
        position.y = Mathf.Clamp01(position.y);

        Color pickedColor = colorChartTexture.GetPixelBilinear(position.x, position.y);
        redSlider.value = pickedColor.r;
        greenSlider.value = pickedColor.g;
        blueSlider.value = pickedColor.b;
        colorDisplay.color = pickedColor;
        hexInputField.text = ColorToHex(pickedColor);
    }

    private string ColorToHex(Color color)
    {
        int r = (int)(color.r * 255f);
        int g = (int)(color.g * 255f);
        int b = (int)(color.b * 255f);
        return string.Format("{0:X2}{1:X2}{2:X2}", r, g, b);
    }

    private Color HexToColor(string hex)
    {
        hex = hex.Replace("0x", "").Replace("#", "");
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        return new Color32(r, g, b, 255);
    }
}