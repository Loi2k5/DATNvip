using UnityEngine;

public class LightningStrike : MonoBehaviour
{
    public float damage = 70f;
    private bool hasHit = false;
    private bool canDealDamage = false;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.speed = 0f; // Tạm dừng animation
            Invoke(nameof(ResumeAnimation), 3f); // Sau 3s mới đánh
        }
    }

    private void ResumeAnimation()
    {
        animator.speed = 1f;
        canDealDamage = true;
    }

    // Gọi bằng Animation Event đúng lúc sét đánh
    public void DealDamage()
    {
        if (!canDealDamage || hasHit) return;

        Collider2D hit = Physics2D.OverlapCircle(transform.position, 0.5f);
        if (hit != null)
        {
            Player player = hit.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damage);
                hasHit = true;
            }
        }
    }


    // Gọi ở cuối animation
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
