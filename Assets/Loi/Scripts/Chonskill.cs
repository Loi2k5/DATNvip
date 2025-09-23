using UnityEngine;

public class Chonskill : MonoBehaviour
{
    [SerializeField] private Player player;  // tham chiếu tới Player
    public GameObject shieldUI; // Cái "Khien" trong Hierarchy (UI Image)
    public GameObject shieldVFX;

    private WeaponStats GetCurrentWeapon()
    {
        // Kiểm tra trong player có gắn Gun/Weapon không
        if (player == null) return null;

        foreach (Transform child in player.transform)
        {
            WeaponStats weapon = child.GetComponent<WeaponStats>();
            if (weapon != null && child.gameObject.activeSelf)
            {
                return weapon;
            }
        }

        return null;
    }

    

    // --- Skill 1: Giảm sát thương từ quái ---
    public void ReduceEnemyStayDamage()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            enemy.ReduceStayDamage(1f); // gọi hàm trong Enemy để giảm stayDamage
        }

        if (shieldUI != null)
        {
            shieldUI.SetActive(true); // bật cái khiên lên
            shieldVFX.SetActive(true);
        }    
            

        ClosePanel();
    }

    // --- Skill 2: Giảm hồi chiêu Dash ---
    public void ReduceDashCooldown()
    {
        if (player != null)
        {
            player.ReduceDashCooldown(4f); // giảm 4 giây
            Debug.Log("Đã giảm 4 giây cooldown Dash!");
        }
        else
        {
            Debug.LogWarning("Player chưa được gán trong Chonskill!");
        }
        ClosePanel();
    }

    // --- Skill 3: Tăng máu tối đa ---
    public void IncreaseMaxHp()
    {
        if (player != null)
        {
            player.IncreaseMaxHp(100f); // tăng 100 máu
            Debug.Log("Đã tăng thêm 100 máu tối đa!");
        }
        else
        {
            Debug.LogWarning("Player chưa được gán trong Chonskill!");
        }
        ClosePanel();
    }

    void ClosePanel()
    {
        gameObject.SetActive(false);  // ẩn panel chọn skill
        Time.timeScale = 1f;          // resume game
    }
}
