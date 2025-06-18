using UnityEngine;

public class MiniBoss : Enemy
{
    [SerializeField] private GameObject xpOrbPrefab; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.TakeDamage(enterDamage);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.TakeDamage(stayDamage);
        }
    }

    protected override void Die()
    {
        
        if (xpOrbPrefab != null)
        {
            for (int i = 0; i < 5; i++)
            {
                Vector2 offset = Random.insideUnitCircle * 0.4f;
                Instantiate(xpOrbPrefab, transform.position + (Vector3)offset, Quaternion.identity);
            }
        }

        base.Die();
    }
}
