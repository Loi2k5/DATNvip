using UnityEngine;
using System.Collections; // Cần thiết cho Coroutine (cho cooldown tấn công)

public class PetMovement : MonoBehaviour
{
    // Loại bỏ tốc độ di chuyển bằng WASD vì pet sẽ chỉ đi theo Player và tấn công Enemy
    // public float moveSpeed = 5f; // Tốc độ di chuyển của pet khi điều khiển bằng WASD

    public float followSpeed = 4f; // Tốc độ khi đi theo Player
    public float attackSpeed = 6f; // Tốc độ khi di chuyển tới Enemy để tấn công
    public float followDistance = 5f; // Khoảng cách tối đa để Pet bắt đầu đi theo Player
    public float stopFollowDistance = 1.5f; // Khoảng cách tối thiểu để Pet dừng lại khi theo Player
    public float attackRange = 1.0f; // Phạm vi tấn công (khoảng cách Pet cần đến gần Enemy)
    public float detectionRange = 10f; // Phạm vi phát hiện Enemy và Player
    public float petDamage = 10f; // Sát thương mà pet gây ra cho Enemy

    public float attackCooldown = 1.5f; // Thời gian hồi chiêu giữa các lần tấn công tự động
    private bool canAttack = true; // Biến kiểm soát hồi chiêu tấn công

    private Rigidbody2D rb;
    private Transform playerTransform; // Tham chiếu đến Transform của Player
    private GameObject currentTargetEnemy; // Enemy hiện tại mà Pet đang tấn công

    // Tùy chọn: Thêm Animator để điều khiển animation cho Pet (Idle, Run, Attack)
    private Animator petAnimator;
    // Tham số Animator để dễ sử dụng và tránh lỗi chính tả
    private readonly int isMovingHash = Animator.StringToHash("IsMoving");
    private readonly int attackTriggerHash = Animator.StringToHash("Attack");


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D not found on this GameObject. Please add one to " + gameObject.name + ".");
        }
        // Đảm bảo Gravity Scale của Rigidbody2D là 0 nếu không muốn pet bị ảnh hưởng bởi trọng lực.

        petAnimator = GetComponent<Animator>();
        if (petAnimator == null)
        {
            Debug.LogWarning("Animator not found on this GameObject. Pet animations (IsMoving, Attack) will not play.");
        }

        // Tìm Player khi bắt đầu game
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("Player GameObject with tag 'Player' not found in the scene. Pet will not follow.");
        }

        // Đảm bảo pet hướng mặt về bên trái khi bắt đầu game (nếu bạn muốn)
        // Nếu sprite của bạn ban đầu đã nhìn trái, bạn có thể bỏ chú thích dòng này.
        // transform.localScale = new Vector3(-1, 1, 1);
    }

    void FixedUpdate()
    {
        Vector2 movement = Vector2.zero; // Mặc định pet không di chuyển
        float currentCalculatedSpeed = 0f; // Tốc độ sẽ được dùng cho Rigidbody

        // Bước 1: Quyết định hành vi của Pet (Ưu tiên tấn công, sau đó follow)
        GameObject nearestEnemy = FindNearestEnemy(); // Tìm Enemy gần nhất trong tầm phát hiện

        if (nearestEnemy != null && Vector2.Distance(transform.position, nearestEnemy.transform.position) <= detectionRange)
        {
            // --- Hành vi: Tấn công Enemy ---
            currentTargetEnemy = nearestEnemy;
            float distanceToEnemy = Vector2.Distance(transform.position, currentTargetEnemy.transform.position);

            if (distanceToEnemy <= attackRange)
            {
                // Trong tầm tấn công: Dừng lại và tấn công
                movement = Vector2.zero;
                TryAttackEnemy(currentTargetEnemy);
            }
            else
            {
                // Ngoài tầm tấn công nhưng trong tầm phát hiện: Di chuyển đến gần Enemy
                movement = (currentTargetEnemy.transform.position - transform.position).normalized;
                currentCalculatedSpeed = attackSpeed;
            }
        }
        else if (playerTransform != null)
        {
            // --- Hành vi: Follow Player (nếu không có Enemy gần hoặc Enemy quá xa) ---
            currentTargetEnemy = null; // Reset mục tiêu Enemy
            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

            if (distanceToPlayer > stopFollowDistance && distanceToPlayer < followDistance)
            {
                // Player ở trong khoảng follow: Di chuyển theo Player
                movement = (playerTransform.position - transform.position).normalized;
                currentCalculatedSpeed = followSpeed;
            }
            // Nếu Player quá gần (<= stopFollowDistance), pet sẽ dừng lại
            else if (distanceToPlayer <= stopFollowDistance)
            {
                movement = Vector2.zero;
            }
            // Nếu Player quá xa (>= followDistance), pet sẽ đứng yên cho đến khi Player lại gần hoặc có Enemy
        }


        // Bước 2: Áp dụng vận tốc cho Rigidbody2D
        if (rb != null)
        {
            rb.linearVelocity = movement * currentCalculatedSpeed;

            // Cập nhật Animator cho di chuyển (nếu có)
            if (petAnimator != null)
            {
                petAnimator.SetBool(isMovingHash, movement.magnitude > 0.1f); // "IsMoving" là true nếu đang di chuyển
            }
        }

        // Bước 3: Lật hình ảnh pet theo hướng di chuyển ngang
        // Chỉ lật khi thực sự có di chuyển theo chiều ngang đáng kể
        if (movement.x < 0 && Mathf.Abs(movement.x) > 0.05f) // Di chuyển sang trái
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (movement.x > 0 && Mathf.Abs(movement.x) > 0.05f) // Di chuyển sang phải
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        // Khi movement.x = 0 (đứng yên hoặc chỉ di chuyển dọc), pet sẽ giữ nguyên hướng quay mặt cuối cùng.
    }


    // --- Hàm tìm kiếm Enemy gần nhất trong tầm phát hiện ---
    GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearestEnemy = null;
        float minDistance = detectionRange + 1f; // Khởi tạo với khoảng cách lớn hơn detectionRange

        foreach (GameObject enemy in enemies)
        {
            if (enemy != null) // Đảm bảo Enemy không phải là null (có thể đã bị hủy)
            {
                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                if (distance < minDistance && distance <= detectionRange) // Chỉ xem xét Enemy trong tầm phát hiện
                {
                    minDistance = distance;
                    nearestEnemy = enemy;
                }
            }
        }
        return nearestEnemy;
    }

    // --- Hàm cố gắng tấn công Enemy ---
    void TryAttackEnemy(GameObject enemyToAttack)
    {
        if (canAttack && enemyToAttack != null)
        {
            if (petAnimator != null)
            {
                petAnimator.SetTrigger(attackTriggerHash); // Kích hoạt animation tấn công
            }
            Debug.Log(gameObject.name + " is initiating attack on " + enemyToAttack.name + "!");
            // Thực hiện việc gây sát thương thực tế trong PetDealDamage()

            canAttack = false; // Bắt đầu thời gian hồi chiêu
            StartCoroutine(AttackCooldownRoutine());
        }
    }

    // --- Coroutine để quản lý thời gian hồi chiêu tấn công ---
    IEnumerator AttackCooldownRoutine()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true; // Cho phép tấn công lại sau khi hết hồi chiêu
    }

    // --- Phương thức này có thể được gọi từ Animation Event của Pet's Attack Animation ---
    // Để thực hiện gây sát thương tại một thời điểm cụ thể trong animation.
    public void PetDealDamage()
    {
        // Kiểm tra xem currentTargetEnemy có còn hợp lệ và trong tầm tấn công không
        if (currentTargetEnemy != null) // Kiểm tra null trước
        {
            // Kiểm tra lại khoảng cách để đảm bảo địch vẫn trong tầm khi damage được kích hoạt (từ animation event)
            if (Vector2.Distance(transform.position, currentTargetEnemy.transform.position) <= attackRange)
            {
                Debug.Log("Pet dealt " + petDamage + " damage to " + currentTargetEnemy.name + "!");
                // Tìm component Enemy trên currentTargetEnemy và gọi hàm TakeDamage
                Enemy enemyScript = currentTargetEnemy.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    enemyScript.TakeDamage(petDamage);
                }
                else
                {
                    Debug.LogWarning("Enemy script not found on " + currentTargetEnemy.name + ". Cannot deal damage.");
                }
            }
            else
            {
                // Nếu địch không còn trong tầm, có thể do địch di chuyển ra xa hoặc đã bị tiêu diệt bởi người chơi
                Debug.Log("Pet's target " + currentTargetEnemy.name + " is out of attack range at time of damage event.");
                currentTargetEnemy = null; // Xóa mục tiêu nếu nó đã thoát khỏi tầm
            }
        }
        else
        {
            Debug.Log("PetDealDamage called but no currentTargetEnemy or it was destroyed.");
        }
    }
}