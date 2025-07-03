using UnityEngine;

public class MinibossMelee : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float maxHp = 100f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackDamage = 20f;
    [SerializeField] private float attackCooldown = 2f;

    private float currentHp;
    private float nextAttackTime = 0f;

    [Header("References")]
    private Player player;
    private Animator animator;
    private Rigidbody2D rb;

    private void Start()
    {
        player = Object.FindFirstObjectByType<Player>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentHp = maxHp;
    }

    private void FixedUpdate()
    {
        if (player == null || !player.enabled)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance <= attackRange)
        {
            rb.linearVelocity = Vector2.zero;

            if (Time.time >= nextAttackTime)
            {
                Attack();
                nextAttackTime = Time.time + attackCooldown;
            }
        }
        else
        {
            MoveTowardsPlayer();
        }
    }

    private void MoveTowardsPlayer()
    {
        if (player == null || !player.enabled) return;

        Vector2 direction = (player.transform.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;

        if (animator != null)
            animator.SetBool("IsMoving", true);
    }

    private void Attack()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack");
            animator.SetBool("IsMoving", false);
        }

        if (player == null || !player.enabled) return;

        float distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance <= attackRange)
        {
            player.TakeDamage(attackDamage);
            Debug.Log("Player bị chém bởi miniboss!");
        }
    }

    public void TakeDamage(float amount)
    {
        currentHp -= amount;
        Debug.Log("Miniboss mất máu: " + amount);

        if (currentHp <= 0)
            Die();
    }

    private void Die()
    {
        Debug.Log("Miniboss chết!");
        if (animator != null) animator.SetTrigger("Die");
        Destroy(gameObject, 1.2f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBullet"))
        {
            // Ví dụ đạn có sát thương cố định, hoặc bạn có thể lấy damage từ script đạn
            TakeDamage(10f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
