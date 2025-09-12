using UnityEngine;

public class WeaponStats : MonoBehaviour
{
    [Header("Weapon Settings")]
    public string weaponName = "Basic Weapon";   // Tên vũ khí
    public float damage = 10f;                   // Sát thương gốc
    public float fireRate = 1f;                  // Tốc độ bắn (số phát/giây)
    public float range = 10f;                    // Tầm bắn
    public float projectileSpeed = 15f;          // Tốc độ đạn (nếu có)

    [Header("Upgrade Multipliers")]
    public float damageMultiplier = 1f;          // Hệ số sát thương
    public float fireRateMultiplier = 1f;        // Hệ số tốc độ bắn

    /// <summary>
    /// Tăng sát thương vũ khí thêm một lượng cụ thể
    /// </summary>
    public void UpgradeDamage(float amount)
    {
        damage += amount;
        Debug.Log($"{weaponName} damage upgraded! New damage: {damage}");
    }

    /// <summary>
    /// Tăng tốc độ bắn
    /// </summary>
    public void UpgradeFireRate(float amount)
    {
        fireRate += amount;
        Debug.Log($"{weaponName} fire rate upgraded! New fire rate: {fireRate}");
    }

    /// <summary>
    /// Nhân hệ số sát thương
    /// </summary>
    public void MultiplyDamage(float factor)
    {
        damage *= factor;
        Debug.Log($"{weaponName} damage multiplied! New damage: {damage}");
    }

    /// <summary>
    /// Nhân hệ số tốc độ bắn
    /// </summary>
    public void MultiplyFireRate(float factor)
    {
        fireRate *= factor;
        Debug.Log($"{weaponName} fire rate multiplied! New fire rate: {fireRate}");
    }
}
