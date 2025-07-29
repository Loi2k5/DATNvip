using UnityEngine;

public class HeallEnemy : Enemy
{
    [SerializeField] private float healValue = 20f;
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (player != null)
            {
                player.TakeDamage(enterDamage);
                animator.SetBool("isAttacking", true);
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (player != null)
            {
                player.TakeDamage(stayDamage);
                animator.SetBool("isAttacking", true);
            }
        }
    }
    protected override void Die()
    {
        HealPlayer();
        base.Die();
    }
    private void HealPlayer()
    {
        if (player != null)
        {
            player.Heal(healValue);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            animator.SetBool("isAttacking", false); // tắt animation khi không còn đụng
        }
    }
}
