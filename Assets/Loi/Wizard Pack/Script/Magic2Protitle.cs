using UnityEngine;

[DisallowMultipleComponent]
public class Magic2Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 12f;
    [SerializeField] private float damage = 50f;
    [SerializeField] private float timeToLive = 3f;

    private Vector2 moveDir;

    public void SetTarget(Transform target)
    {
        if (target == null)
        {
            moveDir = Vector2.right;
            return;
        }
        moveDir = ((Vector2)(target.position - transform.position)).normalized;
        // Xoay sprite theo hướng bay (nếu cần)
        float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void Start()
    {
        Destroy(gameObject, timeToLive);
        if (moveDir == Vector2.zero)
        {
            // Nếu chưa được set (spawn thẳng), bắn theo hướng phải
            moveDir = Vector2.right;
        }
    }

    private void Update()
    {
        transform.position += (Vector3)(moveDir * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player p = other.GetComponent<Player>();
            if (p != null)
            {
                p.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        else if (other.CompareTag("Untagged") == false && !other.isTrigger)
        {
            // Nếu bạn có tường/vật thể cản đường (collider không trigger) thì nổ
            Destroy(gameObject);
        }
    }
}
