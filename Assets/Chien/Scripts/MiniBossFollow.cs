using UnityEngine;

public class MiniBossFollow : Enemy // Kế thừa từ Enemy
{
    public float stopDistance = 0.5f;
    public float attackDelay = 1.2f;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    private int attackCounter = 0;
    private float lastAttackTime;

    [HideInInspector] public bool isInAttackRange = false;

    protected override void Start()
    {
        base.Start(); // Gọi Start của Enemy (setup máu, player,...)
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    protected override void Update()
    {
        base.Update(); // Gọi logic di chuyển và flip từ Enemy

        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance > stopDistance)
        {
            // Di chuyển đã xử lý trong base.Update()
        }

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