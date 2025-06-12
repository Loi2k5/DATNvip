using UnityEngine;

public class PetMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Tốc độ di chuyển của pet

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D not found on this GameObject. Please add one.");
        }

        // Đảm bảo pet hướng mặt về bên trái khi bắt đầu game
        // Nếu sprite của bạn ban đầu đã nhìn trái, bạn có thể bỏ dòng này hoặc đảm bảo scale X là -1
        //transform.localScale = new Vector3(1, 1, 1);
    }

    void FixedUpdate()
    {
        // Lấy giá trị đầu vào từ các phím W, S, A, D
        float horizontalInput = Input.GetAxis("Horizontal"); // A (-1) và D (1)
        float verticalInput = Input.GetAxis("Vertical");   // S (-1) và W (1)

        // Tạo vector di chuyển
        Vector2 movement = new Vector2(horizontalInput, verticalInput);

        // Chuẩn hóa vector di chuyển để tránh di chuyển nhanh hơn khi đi chéo
        if (movement.magnitude > 1f)
        {
            movement.Normalize();
        }

        // Áp dụng lực hoặc thay đổi vị trí
        rb.linearVelocity = movement * moveSpeed;

        // Tùy chọn: Lật hình ảnh pet theo hướng di chuyển ngang
        if (horizontalInput < 0) // Di chuyển sang trái
        {
            // Nếu bạn muốn pet quay mặt sang trái, scale X phải là -1
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (horizontalInput > 0) // Di chuyển sang phải
        {
            // Nếu bạn muốn pet quay mặt sang phải, scale X phải là 1
            transform.localScale = new Vector3(-1, 1, 1);
        }
        // Khi horizontalInput = 0 (đứng yên), pet sẽ giữ nguyên hướng quay mặt cuối cùng.
    }
}