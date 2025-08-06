using UnityEngine;
using System.Collections;

public class PetMovement : MonoBehaviour
{
    public float followSpeed = 4f;
    public float attackSpeed = 6f;
    public float followDistance = 5f;
    public float stopFollowDistance = 1.5f;
    public float attackRange = 1.0f;
    public float detectionRange = 10f;
    public float petAttackDamage = 10f;
    public float attackCooldown = 1.5f;

    private bool canAttack = true;
    private bool isAttacking = false; // 🔒 Khóa trạng thái khi đang đánh

    private Rigidbody2D rb;
    private Transform playerTransform;
    private GameObject currentTargetEnemy;

    private Animator petAnimator;
    private readonly int isMovingHash = Animator.StringToHash("IsMoving");
    private readonly int attackTriggerHash = Animator.StringToHash("Attack");
    private readonly int speedHash = Animator.StringToHash("Speed");

    private Vector3 lockedEnemyPosition;
    private Enemy lockedEnemyScript;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D not found on this GameObject.");
        }

        petAnimator = GetComponent<Animator>();
        if (petAnimator == null)
        {
            Debug.LogWarning("Animator not found. Animations will not play.");
        }

        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("Player GameObject with tag 'Player' not found.");
        }
    }

    void FixedUpdate()
    {
        if (isAttacking)
        {
            // 🔒 Nếu đang tấn công, dừng di chuyển và không đổi animation
            rb.linearVelocity = Vector2.zero;

            if (petAnimator != null)
            {
                petAnimator.SetFloat(speedHash, 0f);
            }

            return; // Không xử lý thêm gì nữa khi đang tấn công
        }

        Vector2 movement = Vector2.zero;
        float currentCalculatedSpeed = followSpeed;

        GameObject nearestEnemy = FindNearestEnemy();

        if (nearestEnemy != null && Vector2.Distance(transform.position, nearestEnemy.transform.position) <= detectionRange)
        {
            currentTargetEnemy = nearestEnemy;
            float distanceToEnemy = Vector2.Distance(transform.position, currentTargetEnemy.transform.position);

            if (distanceToEnemy <= attackRange)
            {
                movement = Vector2.zero;
                TryAttackEnemy(currentTargetEnemy);
            }
            else
            {
                movement = (currentTargetEnemy.transform.position - transform.position).normalized;
                currentCalculatedSpeed = attackSpeed;
            }
        }
        else if (playerTransform != null)
        {
            currentTargetEnemy = null;
            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

            if (distanceToPlayer > stopFollowDistance && distanceToPlayer < followDistance)
            {
                movement = (playerTransform.position - transform.position).normalized;
                currentCalculatedSpeed = followSpeed;
            }
            else if (distanceToPlayer <= stopFollowDistance)
            {
                movement = Vector2.zero;
            }
        }

        if (rb != null)
        {
            rb.linearVelocity = movement * currentCalculatedSpeed;

            if (petAnimator != null)
            {
                float currentSpeed = movement.magnitude * currentCalculatedSpeed;
                petAnimator.SetFloat(speedHash, currentSpeed);
            }
        }

        if (movement.x < -0.05f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (movement.x > 0.05f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    GameObject FindNearestEnemy()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        GameObject nearestEnemy = null;
        float minDistance = detectionRange + 1f;

        foreach (Enemy enemyComponent in enemies)
        {
            if (enemyComponent != null && enemyComponent.gameObject.activeInHierarchy)
            {
                float distance = Vector2.Distance(transform.position, enemyComponent.transform.position);
                if (distance < minDistance && distance <= detectionRange)
                {
                    minDistance = distance;
                    nearestEnemy = enemyComponent.gameObject;
                }
            }
        }
        return nearestEnemy;
    }

    void TryAttackEnemy(GameObject enemyToAttack)
    {
        if (canAttack && enemyToAttack != null)
        {
            lockedEnemyScript = enemyToAttack.GetComponent<Enemy>();
            lockedEnemyPosition = enemyToAttack.transform.position;

            if (petAnimator != null)
            {
                petAnimator.SetTrigger(attackTriggerHash);
            }

            isAttacking = true; // 🔒 Khóa trạng thái
            canAttack = false;
            StartCoroutine(AttackCooldownRoutine());
        }
    }

    IEnumerator AttackCooldownRoutine()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    // Gọi từ Animation Event để gây sát thương
    public void PetDealDamage()
    {
        if (lockedEnemyScript != null)
        {
            lockedEnemyScript.TakeDamage(petAttackDamage);
            Debug.Log("Pet dealt damage to locked enemy.");
        }
        else
        {
            Debug.Log("Locked enemy is null or destroyed.");
        }

        isAttacking = false; // ✅ Hết đòn → trở lại trạng thái bình thường
    }
}
