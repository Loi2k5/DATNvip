using UnityEngine;

[DisallowMultipleComponent]
public class Magic1Area : MonoBehaviour
{
    [SerializeField] private float damage = 50f;
    [SerializeField] private float lifeTime = 0.6f;  // sống ngắn như một vụ nổ/đốm phép

    private void Start()
    {
        Destroy(gameObject, lifeTime);
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
        }
    }
}
