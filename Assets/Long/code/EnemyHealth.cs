using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [Header("Cài đặt máu")]
    public float maxHealth = 100f;
    public float regenRate = 5f;         // Lượng máu hồi mỗi giây
    public float regenDelay = 3f;        // Thời gian chờ trước khi bắt đầu hồi máu

    [Header("Tham chiếu UI (tuỳ chọn)")]
    public Slider healthBar;             // Gắn slider nếu muốn hiển thị thanh máu

    private float currentHealth;
    private float lastDamageTime;

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    void Update()
    {
        // Nếu đủ thời gian sau khi bị đánh, bắt đầu hồi máu
        if (Time.time - lastDamageTime > regenDelay && currentHealth < maxHealth)
        {
            currentHealth += regenRate * Time.deltaTime;
            currentHealth = Mathf.Min(currentHealth, maxHealth);

            UpdateHealthBar();
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0f);
        lastDamageTime = Time.time;

        UpdateHealthBar();

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} đã chết.");
        Destroy(gameObject);
    }
}
