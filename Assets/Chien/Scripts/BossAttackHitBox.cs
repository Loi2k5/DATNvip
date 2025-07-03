using UnityEngine;

public class BossAttackHitBox : MonoBehaviour
{
    public float damageAttack1 = 50f;
    public float damageAttack2 = 100f;

    private Animator animator;
    private bool hasDealtDamage = false;

    private void Start()
    {
        animator = GetComponentInParent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasDealtDamage)
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                if (animator.GetBool("IsAttacking2"))
                {
                    player.TakeDamage(damageAttack2);
                }
                else if (animator.GetBool("IsAttacking1"))
                {
                    player.TakeDamage(damageAttack1);
                }

                hasDealtDamage = true; // Ngăn không bị trừ liên tục trong cùng 1 đòn
            }
        }
    }

    private void Update()
    {
        // Reset lại sau mỗi đòn
        if (!animator.GetBool("IsAttacking1") && !animator.GetBool("IsAttacking2"))
        {
            hasDealtDamage = false;
        }
    }
}