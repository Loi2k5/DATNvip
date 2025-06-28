using UnityEngine;

public class Boss : Enemy
{
    public float attackRange = 5f;
    public float attackCooldown = 3f;
    public int attackDamage = 20;

    private bool canAttack = true;
    private Transform playerTransform;
    private Animator animator;

    private bool useFirstAttack = true; // ?? luân phiên animation

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        animator = GetComponent<Animator>();

        if (playerTransform == null)
        {
            Debug.LogError("Không tìm th?y Player! Hãy gán tag 'Player' cho nhân v?t.");
        }

        if (animator == null)
        {
            Debug.LogError("Boss không có Animator!");
        }
    }

    private void Update()
    {
        if (playerTransform == null || !canAttack) return;

        float distance = Vector2.Distance(transform.position, playerTransform.position);
        if (distance <= attackRange)
        {
            StartCoroutine(Attack());
        }
    }

    private System.Collections.IEnumerator Attack()
    {
        canAttack = false;

        // ??i animation luân phiên
        if (useFirstAttack)
        {
            animator.SetTrigger("Attackone");
        }
        else
        {
            animator.SetTrigger("Attacktwo");
        }

        useFirstAttack = !useFirstAttack; // ??i l??t cho l?n sau

        // Delay theo animation
        yield return new WaitForSeconds(0.5f);

        if (player != null && Vector2.Distance(transform.position, playerTransform.position) <= attackRange)
        {
            player.TakeDamage(attackDamage);
        }

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && player == null)
        {
            player = collision.GetComponent<Player>();
        }
    }
}
