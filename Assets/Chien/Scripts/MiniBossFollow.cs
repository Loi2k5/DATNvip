using UnityEngine;

public class MiniBossFollow : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 2f;
    public float stopDistance = 0.5f;
    public float attackDelay = 1.2f;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    private int attackCounter = 0;
    private float lastAttackTime;

    [HideInInspector] public bool isInAttackRange = false; // dùng để kiểm tra trigger

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        if (player == null)
        {
            GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");
            if (foundPlayer != null)
                player = foundPlayer.transform;
        }
    }

    private void Update()
    {
        if (player == null) return;

        Vector2 direction = (player.position - transform.position).normalized;

        // Flip hướng
        if (spriteRenderer != null)
            spriteRenderer.flipX = direction.x < 0;

        // Di chuyển
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance > stopDistance)
        {
            transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);
        }

        // Tấn công theo combo nếu đang được phép
        if (isInAttackRange && Time.time >= lastAttackTime + attackDelay)
        {
            if (attackCounter < 2)
            {
                animator.SetTrigger("Attack1");
                attackCounter++;
            }
            else
            {
                animator.SetTrigger("Attack2");
                attackCounter = 0;
            }

            lastAttackTime = Time.time;
        }
    }
}
