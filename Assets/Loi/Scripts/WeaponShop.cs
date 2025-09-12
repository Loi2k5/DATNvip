using UnityEngine;

public class WeaponShop : MonoBehaviour
{
    public GameObject weaponPanel; // Panel chọn súng

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            weaponPanel.SetActive(true); // Bật panel chọn súng khi Player chạm
            Time.timeScale = 0f; // Dừng game để chọn (nếu muốn)
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            weaponPanel.SetActive(false); // Tắt panel khi rời khỏi
            Time.timeScale = 1f;
        }
    }
}
