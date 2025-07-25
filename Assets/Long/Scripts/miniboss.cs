using UnityEngine;
using UnityEngine.UI;

public class MinibossMelee : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float maxHp = 100f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackDamage = 20f;
    [SerializeField] private float attackCooldown = 2f;

    [Header("Shooting")]
    [SerializeField] private GameObject enemyBullet;  // Prefab đạn của miniboss
    [SerializeField] private Transform firePoint;     // Vị trí bắn đạn

    [Header("Reward")]
    [SerializeField] private GameObject usbPrefabs;   // USB rớt ra khi chết

    [Header("HP UI")]
    [SerializeField] private Image hpBar;             // Thanh máu

    private float currentHp;
    private float nextAttackTime;

    [Header("References")]
    private Player player;
    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHp = maxHp;
        UpdateHpUI();
    }

    private void FixedUpdate()
    {
        if (player == null) return;

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
        Vector2 direction = (player.transform.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;

        // Xoay mặt miniboss
        if (spriteRenderer != null)
            spriteRenderer.flipX = direction.x < 0;

        animator?.SetBool("IsMoving", true);
    }

    private void Attack()
    {
        animator?.SetTrigger("Attack");
        animator?.SetBool("IsMoving", false);

        float distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance <= attackRange)
        {
            player.TakeDamage(attackDamage);
            Debug.Log("Miniboss tấn công Player!");
        }

        // Bắn đạn nếu có setup
        if (enemyBullet != null && firePoint != null)
        {
            Instantiate(enemyBullet, firePoint.position, Quaternion.identity);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0);
        UpdateHpUI();

        if (currentHp <= 0)
            Die();
    }

    private void UpdateHpUI()
    {
        if (hpBar != null)
            hpBar.fillAmount = currentHp / maxHp;
    }

    private void Die()
    {
        Debug.Log("Miniboss chết!");
        animator?.SetTrigger("Die");

        // Rớt vật phẩm (USB)
        if (usbPrefabs != null)
        {
            Instantiate(usbPrefabs, transform.position, Quaternion.identity);
        }

        Destroy(gameObject, 1.2f); // Delay để animation Die hiển thị
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
