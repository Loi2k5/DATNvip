using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PetFollow2D : MonoBehaviour
{
    public Transform player;
    public float followDistance = 1.5f;
    public float moveSpeed = 3f;

    [Header("Combat")]
    public float attackRange = 1.2f;
    public float attackCooldown = 1.0f;
    public int damage = 10;

    private float lastAttackTime;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                player = p.transform;
        }
    }

    void FixedUpdate()
    {
        if (player == null) return;

        Transform nearestEnemy = FindNearestEnemyInRange();

        if (nearestEnemy != null)
        {
            // Tấn công nếu trong tầm và đủ thời gian hồi chiêu
            float dist = Vector2.Distance(transform.position, nearestEnemy.position);
            if (dist <= attackRange && Time.time - lastAttackTime >= attackCooldown)
            {
                Attack(nearestEnemy);
                lastAttackTime = Time.time;
                return; // không cần di chuyển nếu đang đánh
            }
        }

        // Di chuyển theo player nếu không đánh
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance > followDistance)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            Vector2 newPosition = rb.position + direction * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(newPosition);
        }
    }

    Transform FindNearestEnemyInRange()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange);
        float closestDistance = Mathf.Infinity;
        Transform nearestEnemy = null;

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                float dist = Vector2.Distance(transform.position, hit.transform.position);
                if (dist < closestDistance)
                {
                    closestDistance = dist;
                    nearestEnemy = hit.transform;
                }
            }
        }

        return nearestEnemy;
    }

    void Attack(Transform enemy)
    {
        // Gây sát thương nếu enemy có script EnemyHealth
        EnemyHealth health = enemy.GetComponent<EnemyHealth>();
        if (health != null)
        {
            health.TakeDamage(damage);
            Debug.Log("Pet đánh enemy!");
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
