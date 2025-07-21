using UnityEngine;

public class ExplosionEnemy : Enemy
{
    [SerializeField] private GameObject explosionPreFabs;
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void CreateExplosion()
    {
        if (explosionPreFabs != null)
        {
            Instantiate(explosionPreFabs, transform.position, Quaternion.identity);
        }
    }
    protected override void Die()
    {
        CreateExplosion();
        base.Die();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CreateExplosion();
            animator.SetBool("isAttacking", true);
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
