using UnityEngine;

public class Meteor : MonoBehaviour
{
    private Vector3 targetPos;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float damage = 30f;
    [SerializeField] private GameObject explosionEffect;

    public void SetTarget(Vector3 pos)
    {
        targetPos = pos;
    }

    private void Update()
    {
        // Di chuyển xuống target
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            Explode();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Explode();
        }
    }

    private void Explode()
    {
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
