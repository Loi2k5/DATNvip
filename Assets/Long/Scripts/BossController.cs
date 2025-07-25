// BossController.cs
using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour
{
    public int currentHp = 100;

    public void TakeDamage(int amount)
    {
        currentHp -= amount;
        if (currentHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Boss đã bị tiêu diệt!");
        GameFlags.bossIsDead = true; // ✅ báo hiệu boss đã chết
    }
}
