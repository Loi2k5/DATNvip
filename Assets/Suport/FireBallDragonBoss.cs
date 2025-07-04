using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallDragonBoss : MonoBehaviour
{
    public BoxCollider2D fireBallTrigger;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }
    private void Update()
    {
        StartCoroutine(DestroyAfterTime(8f)); // Bắt đầu đếm ngược 8 giây
    }

    public void Initialize(Transform playerTransform)
    {
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        rb.linearVelocity = direction * 15f; // Tốc độ của FireBall
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Dừng FireBall
            rb.linearVelocity = Vector2.zero;
            // Kích hoạt trigger
            StartCoroutine(ActivateTrigger());
        }
    }

    private IEnumerator ActivateTrigger()
    {
        fireBallTrigger.enabled = true;

        // Chờ 0.5 giây
        yield return new WaitForSeconds(0.5f);

        // Hủy đối tượng FireBall
        Destroy(gameObject);
    }

    private IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        // Hủy đối tượng FireBall sau 8 giây nếu chưa chạm vào Player
        Destroy(gameObject);
    }
}
