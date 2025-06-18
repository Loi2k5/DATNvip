using UnityEngine;

public class PetShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 1f;
    private float nextFireTime;

    public float searchRadius = 8f;
    public LayerMask enemyLayer;

    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            Transform target = FindNearestEnemy();
            if (target != null)
            {
                ShootAt(target);
                nextFireTime = Time.time + 1f / fireRate;
            }
        }
    }

    Transform FindNearestEnemy()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, searchRadius, enemyLayer);
        Transform nearest = null;
        float shortestDist = Mathf.Infinity;

        foreach (var enemy in enemies)
        {
            float dist = Vector2.Distance(transform.position, enemy.transform.position);
            if (dist < shortestDist)
            {
                shortestDist = dist;
                nearest = enemy.transform;
            }
        }
        return nearest;
    }

    void ShootAt(Transform target)
    {
        // 1. Tính hướng bắn
        Vector2 direction = (target.position - firePoint.position).normalized;

        // 2. Tính góc quay
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 3. Tạo đạn và xoay đúng hướng
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0, 0, angle));

        // 4. Gán vận tốc bay theo hướng
        bullet.GetComponent<Rigidbody2D>().linearVelocity = direction * 10f; // 10f là tốc độ, bạn có thể chỉnh
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, searchRadius);
    }
}
