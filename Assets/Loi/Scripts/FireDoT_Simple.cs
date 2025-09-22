using UnityEngine;

public class FireDoT_Simple : MonoBehaviour
{
    [Tooltip("Sát thương mỗi giây")]
    public float damagePerSecond = 2f;

    private void OnTriggerStay2D(Collider2D other)
    {
        // Lấy Enemy ở object cha (phòng khi collider nằm ở child)
        var enemy = other.GetComponentInParent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damagePerSecond * Time.deltaTime);
        }
    }
}
