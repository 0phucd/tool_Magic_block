using UnityEngine;

public class ActivateObjects : MonoBehaviour
    {
    public GameObject object1;
    public GameObject object2;

    private bool parentActive;

    void Start()
    {
        // Lưu trạng thái ban đầu của object cha
        if (object1 != null)
        {
            parentActive = object1.activeSelf;
        }
    }

    void Update()
    {
        // Kiểm tra xem trạng thái của object cha có thay đổi không
        if (object1 != null && object1.activeSelf != parentActive)
        {
            parentActive = object1.activeSelf;

            // Nếu object cha được kích hoạt
            if (parentActive)
            {
                // Kích hoạt object2 nếu chưa được kích hoạt
                if (!object2.activeSelf)
                {
                    object2.SetActive(true);
                }
            }
            else
            {
                // Vô hiệu hóa object2 nếu object cha bị vô hiệu hóa
                if (object2.activeSelf)
                {
                    object2.SetActive(false);
                }
            }
        }
    }
}