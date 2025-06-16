using UnityEngine;
using TMPro;

public class Gun : MonoBehaviour
{
    private float rotateOffset = 180f;

    [SerializeField] private Transform firePos;
    [SerializeField] private GameObject bulletPrefabs;
    [SerializeField] private float shotDelay = 0.15f;
    private float nextShot;

    [SerializeField] private int maxAmmo = 24;
    public int currentAmmo;

    [SerializeField] private TextMeshProUGUI ammoText;

    [SerializeField] private float reloadDelay = 1.5f;
    private bool isReloading = false;
    private float reloadTimer;

    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private LayerMask enemyLayer; // Set LayerMask to only detect Enemy
    [SerializeField] private Transform shootZoneCenter;
    [SerializeField] private float shootZoneRadius = 5f;


    void Start()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoText();
    }

    void Update()
    {
        RotateGun();
        AutoShoot();
        AutoReload();
    }

    void RotateGun()
    {
        if (Input.mousePosition.x < 0 || Input.mousePosition.x > Screen.width ||
            Input.mousePosition.y < 0 || Input.mousePosition.y > Screen.height)
            return;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mouseWorldPos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + rotateOffset);

        Vector3 localScale = transform.localScale;
        localScale.y = angle < -90 || angle > 90 ? 1 : -1;
        transform.localScale = localScale;
    }

    void AutoShoot()
    {
        if (isReloading) return;

        Transform target = GetNearestEnemy();

        if (Time.time > nextShot && currentAmmo > 0 && target != null)
        {
            nextShot = Time.time + shotDelay;

            // Tính hướng đến enemy
            Vector3 direction = (target.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Xoay súng theo hướng bắn
            transform.rotation = Quaternion.Euler(0, 0, angle + rotateOffset);

            // Flip súng theo hướng để không bị ngược (nếu cần)
            Vector3 localScale = transform.localScale;
            localScale.y = (angle < -90 || angle > 90) ? 1 : -1;
            transform.localScale = localScale;

            // Bắn đạn
            GameObject bullet = Instantiate(bulletPrefabs, firePos.position, Quaternion.Euler(0, 0, angle));
            currentAmmo--;
            UpdateAmmoText();

            if (currentAmmo <= 0)
            {
                StartReload();
            }
        }
    }



    Transform GetNearestEnemy()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(shootZoneCenter.position, shootZoneRadius, enemyLayer);
        Transform nearestEnemy = null;
        float minDistance = Mathf.Infinity;

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                float dist = Vector2.Distance(firePos.position, hit.transform.position);
                if (dist < minDistance)
                {
                    minDistance = dist;
                    nearestEnemy = hit.transform;
                }
            }
        }
        return nearestEnemy;
    }



    void AutoReload()
    {
        if (isReloading)
        {
            reloadTimer -= Time.deltaTime;
            if (reloadTimer <= 0f)
            {
                FinishReload();
            }
        }
    }

    void StartReload()
    {
        isReloading = true;
        reloadTimer = reloadDelay;
    }

    void FinishReload()
    {
        currentAmmo = maxAmmo;
        isReloading = false;
        UpdateAmmoText();
    }

    private void UpdateAmmoText()
    {
        if (ammoText != null)
        {
            ammoText.text = isReloading ? "Reloading..." : (currentAmmo > 0 ? currentAmmo.ToString() : "Empty");
        }
    }
}
