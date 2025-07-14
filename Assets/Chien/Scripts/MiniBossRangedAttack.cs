using System.Collections;
using UnityEngine;

public class MiniBossRangedAttack : MonoBehaviour
{
    public GameObject fireBoltPrefab;    // Prefab đạn tia
    public Transform firePoint;          // Vị trí xuất phát đạn
    public float shootForce = 8f;        // Tốc độ bay của đạn
    public float cooldownTime = 7f;      // Thời gian hồi chiêu

    private bool canShoot = true;
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null || !canShoot) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance >= 4f) // Nếu player ở xa thì mới bắn
        {
            StartCoroutine(FireSkill());
        }
    }

    IEnumerator FireSkill()
    {
        canShoot = false;

        Vector2 baseDirection = (player.position - firePoint.position).normalized;

        for (int i = 0; i < 3; i++) // Bắn 3 tia đạn thẳng
        {
            // Tính góc từ vector
            float angle = Mathf.Atan2(baseDirection.y, baseDirection.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, 0, angle); // Xoay theo Z

            GameObject bolt = Instantiate(fireBoltPrefab, firePoint.position, rotation);

            Rigidbody2D rb = bolt.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.linearVelocity = baseDirection * shootForce;

            yield return new WaitForSeconds(0.15f);
        }

        yield return new WaitForSeconds(cooldownTime);
        canShoot = true;
    }
}
