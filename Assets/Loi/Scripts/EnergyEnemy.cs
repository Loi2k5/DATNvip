using UnityEngine;

public class EnergyEnemy : Enemy
{
    [SerializeField] private GameObject energyObject;
    [SerializeField] private GameObject xpOrbPrefab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (player != null)
            {
                player.TakeDamage(enterDamage);
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
            }
        }
    }

    protected override void Die()
    {
        if (energyObject != null)
        {
            GameObject energy = Instantiate(energyObject, transform.position, Quaternion.identity);
            Destroy(energy, 5f);
        }

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
}
