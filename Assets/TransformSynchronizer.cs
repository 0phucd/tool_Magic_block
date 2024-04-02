using UnityEngine;

public class TransformSynchronizer : MonoBehaviour
{
    public Transform object1; // GameObject thứ nhất
    public Transform object2; // GameObject thứ hai

    private Vector3 lastPositionObject1;

    private void Start()
    {
        if (object1 == null || object2 == null)
        {
            Debug.LogError("One or both objects are not assigned!");
            enabled = false; // Disable script if objects are not assigned
        }
        else
        {
            lastPositionObject1 = object1.position; // Lưu vị trí ban đầu của object1
        }
    }

    private void Update()
    {
        // Kiểm tra sự thay đổi vị trí của object1
        if (object1.position != lastPositionObject1)
        {
            Vector3 deltaPosition = object1.position - lastPositionObject1;
            lastPositionObject1 = object1.position;

            // Kiểm tra xem có bất kỳ thành phần x, y hoặc z nào nhỏ hơn 0 không
            if (object1.position.x < 0 || object1.position.y < 0 || object1.position.z < 0)
            {
                // Nếu có, cộng giá trị thay đổi vào vị trí của object2 tương ứng với thành phần đó
                Vector3 newPositionObject2 = object2.position + deltaPosition;
                object2.position = new Vector3(
                    object1.position.x < 0 ? newPositionObject2.x : object2.position.x,
                    object1.position.y < 0 ? newPositionObject2.y : object2.position.y,
                    object1.position.z < 0 ? newPositionObject2.z : object2.position.z
                );
            }
        }
    }
}