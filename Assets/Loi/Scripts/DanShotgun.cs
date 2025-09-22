using UnityEngine;

public class DanShotgun : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 20f;
    [SerializeField] private float timeDestroy = 0.5f;
    [SerializeField] private float damage = 10f;
    [SerializeField] GameObject flamePrefab;
    [SerializeField] private GameObject damagePopupPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, timeDestroy);
    }

    // Update is called once per frame
    void Update()
    {
        MoveBullet();
    }
    void MoveBullet()
    {
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                GameObject blood = Instantiate(flamePrefab, transform.position, Quaternion.identity);
                Destroy(blood, 6f);
                // Spawn damage popup
                GameObject popup = Instantiate(damagePopupPrefab, collision.transform.position, Quaternion.identity);
                popup.GetComponent<DamagePopup>().Setup(damage);
            }
            Destroy(gameObject);
        }

    }
}
