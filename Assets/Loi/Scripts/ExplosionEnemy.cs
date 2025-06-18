using UnityEngine;

public class ExplosionEnemy : Enemy
{
    [SerializeField] private GameObject explosionPreFabs;
    [SerializeField] private GameObject xpOrbPrefab;
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

        if (xpOrbPrefab != null)
        {
            for (int i = 0; i < 2; i++)
            {
                Vector2 offset = Random.insideUnitCircle * 0.3f;
                Instantiate(xpOrbPrefab, transform.position + (Vector3)offset, Quaternion.identity);
            }
        }

        base.Die();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CreateExplosion();
        }
    }
}
