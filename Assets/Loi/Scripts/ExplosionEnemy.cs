using UnityEngine;

public class ExplosionEnemy : Enemy
{
    [SerializeField] private GameObject explosionPreFabs;
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
        }
    }
}
