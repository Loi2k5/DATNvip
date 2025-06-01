using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 1f; // Tốc độ di chuyển của quái vật
    public float moveDistance = 5f; // Khoảng cách quái vật di chuyển qua lại
    public int attackDamage = 10; // Lượng sát thương quái vật gây ra
    private Vector2 startPosition;
    private bool movingRight = true;

    void Start()
    {
        startPosition = transform.position; // Ghi lại vị trí ban đầu của quái vật
    }

    void Update()
    {
        // Di chuyển quái vật qua lại
        if (movingRight)
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
            if (transform.position.x >= startPosition.x + moveDistance)
            {
                movingRight = false; // Đổi hướng
            }
        }
        else
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
            if (transform.position.x <= startPosition.x - moveDistance)
            {
                movingRight = true; // Đổi hướng
            }
        }
    }

    // Xử lý va chạm vật lý với các đối tượng khác
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Kiểm tra xem đối tượng va chạm có tag là "Player" không
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Quái vật va chạm với người chơi!");

            // Cố gắng lấy component PlayerHealth từ đối tượng người chơi
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

            // Nếu component PlayerHealth tồn tại, gọi hàm TakeDamage
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage); // Gây sát thương cho người chơi
            }
            else
            {
                Debug.LogWarning("Player game object does not have a PlayerHealth component!");
            }
        }
    }

    // ********************************************************************************
    // LƯU Ý QUAN TRỌNG VỀ PHƯƠNG PHÁP TẤN CÔNG:
    // OnCollisionEnter2D: Được gọi khi 2 collider vật lý VA CHẠM.
    // Nếu bạn muốn quái vật chỉ gây sát thương MỘT LẦN KHI CHẠM vào và không liên tục,
    // hoặc bạn muốn nó chỉ gây sát thương khi "đánh" (ví dụ: animation tấn công),
    // bạn sẽ cần logic phức tạp hơn:
    // - Dùng OnTriggerEnter2D (nếu collider của quái vật là trigger)
    // - Dùng Coroutine hoặc thời gian cooldown để không gây sát thương liên tục
    // - Hoặc chỉ gọi TakeDamage khi một animation tấn công cụ thể được kích hoạt.
    // Phương pháp hiện tại sẽ gây sát thương mỗi khi có va chạm vật lý mới.
    // ********************************************************************************
}