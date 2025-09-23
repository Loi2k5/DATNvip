using UnityEngine;

public class Lightning : MonoBehaviour
{
    [SerializeField] private float damage = 20f;  // damage gây ra
    [SerializeField] private float lifeTime = 0.5f; // tồn tại 0.5s

    private void Start()
    {
        // Tự hủy sau lifeTime giây
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Nếu trúng enemy thì gây damage
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
    }
}
