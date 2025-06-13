using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopUIManager : MonoBehaviour
{
    public GameObject shopPanel; // Panel Shop
    public Button shopButton;    // Biểu tượng Shop (Button TMP)
    public Button exitButton;    // Nút Exit
    public Button nextButton;    // Nút Next
    public Button prevButton;    // Nút Previous
    public GameObject[] pages;   // Danh sách các trang Shop
    private int currentPage = 0; // Trang hiện tại

    void Start()
    {
        shopPanel.SetActive(false); // Shop bắt đầu ẩn
        shopButton.gameObject.SetActive(true);
        nextButton.gameObject.SetActive(false);
        prevButton.gameObject.SetActive(false);

        exitButton.onClick.AddListener(CloseShop);
        shopButton.onClick.AddListener(OpenShop);
        nextButton.onClick.AddListener(NextPage);
        prevButton.onClick.AddListener(PrevPage);

        currentPage = 0;
        UpdatePage();
    }

    void OpenShop()
    {
        shopPanel.SetActive(true);
        shopButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(pages.Length > 1); // Hiển thị nếu có nhiều trang
        prevButton.gameObject.SetActive(false); // Ẩn nút Previous khi mở trang đầu
        UpdatePage();
    }

    void CloseShop()
    {
        shopPanel.SetActive(false);
        shopButton.gameObject.SetActive(true);
        nextButton.gameObject.SetActive(false);
        prevButton.gameObject.SetActive(false);

        // Ẩn tất cả các trang khi đóng Shop
        foreach (GameObject page in pages)
        {
            page.SetActive(false);
        }

        currentPage = 0; // Reset về trang đầu
    }

    void UpdatePage()
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(i == currentPage);
        }

        // Kiểm tra nút Next/Previous
        nextButton.gameObject.SetActive(currentPage < pages.Length - 1);
        prevButton.gameObject.SetActive(currentPage > 0);
    }

    void NextPage()
    {
        if (currentPage < pages.Length - 1)
        {
            currentPage++;
            UpdatePage();
        }
    }

    void PrevPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            UpdatePage();
        }
    }
}