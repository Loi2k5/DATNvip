using UnityEngine;

public class Pet : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float patrolDistance = 3f;
    public float attackRange = 1.5f;
    public float attackCooldown = 1f;

    private Animator animator;
    private float lastAttackTime;
    private Transform targetEnemy;
    private Vector3 startPoint;
    private int moveDirection = 1;

    void Start()
    {
        animator = GetComponent<Animator>();
        startPoint = transform.position;
    }

    void Update()
    {
        FindNearestEnemy();

        if (targetEnemy != null && Vector2.Distance(transform.position, targetEnemy.position) <= attackRange)
        {
            AttackEnemy();
        }
        else
        {
            Patrol();
        }
    }

    void AttackEnemy()
    {
        Vector2 direction = (targetEnemy.position - transform.position).normalized;

        // Xoay m?t v? phía enemy
        if (direction.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (direction.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        animator.SetFloat("Speed", 0f); // ??ng yên
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            animator.SetBool("IsAttacking", true);
            lastAttackTime = Time.time;
        }
    }

    void Patrol()
    {
        // Xoay m?t theo h??ng ?ang ?i
        if (moveDirection > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);

        // Di chuy?n
        transform.position += new Vector3(moveDirection, 0, 0) * moveSpeed * Time.deltaTime;
        animator.SetBool("IsAttacking", false);
        animator.SetFloat("Speed", moveSpeed);

        // ??i h??ng n?u ?i quá gi?i h?n tu?n tra
        if (Mathf.Abs(transform.position.x - startPoint.x) >= patrolDistance)
        {
            moveDirection *= -1;
        }
    }

    void FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float minDistance = Mathf.Infinity;
        targetEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector2.Distance(transform.position, enemy.transform.position);
            if (dist < minDistance && dist <= attackRange)
            {
                minDistance = dist;
                targetEnemy = enemy.transform;
            }
        }
    }
}
