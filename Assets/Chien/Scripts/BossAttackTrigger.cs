using UnityEngine;

public class BossAttackTrigger : MonoBehaviour
{
    private Animator animator;
    private MiniBossFollow followScript;

    private void Start()
    {
        animator = GetComponentInParent<Animator>();
        followScript = GetComponentInParent<MiniBossFollow>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("IsAttacking1", true);
            followScript.isInAttackRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("IsAttacking1", false);
            followScript.isInAttackRange = false;
        }
    }
}
