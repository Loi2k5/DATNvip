using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopUIManager : MonoBehaviour
{
    public GameObject shopPanel; // Panel Shop
    public Button shopButton;    // Biểu tượng Shop (Button TMP)
    public Button exitButton;    // Nút Exit

    void Start()
    {
        // Đảm bảo Shop ban đầu ẩn
        shopPanel.SetActive(false);

        // Gán sự kiện khi bấm vào biểu tượng Shop
        shopButton.onClick.AddListener(OpenShop);

        // Gán sự kiện khi bấm nút Exit
        exitButton.onClick.AddListener(CloseShop);
    }

    void OpenShop()
    {
        shopPanel.SetActive(true);  // Hiển thị Shop
        shopButton.gameObject.SetActive(false);  // Ẩn biểu tượng Shop
    }

    void CloseShop()
    {
        shopPanel.SetActive(false); // Ẩn Shop
        shopButton.gameObject.SetActive(true);   // Hiển thị lại biểu tượng Shop
    }
}