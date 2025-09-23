using UnityEngine;

public class Tornado : MonoBehaviour
{
    public Vector2 moveDir;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float lifeTime = 10f;
    [SerializeField] private float damage = 10f;

    public void Init(Vector2 dir)
    {
        moveDir = dir;
        Destroy(gameObject, lifeTime); // tự hủy sau X giây
    }

    private void Update()
    {
        transform.Translate(moveDir * speed * Time.deltaTime, Space.World);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}
