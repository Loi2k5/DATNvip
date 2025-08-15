using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public GameObject page1;
    public GameObject page2;
    public Button button1;
    public Button button2;

    void Start()
    {
        // Mặc định hiển thị trang 1
        ShowPage1();
        PlayerPrefs.DeleteKey("SelectedPet");
        // Gán sự kiện cho hai nút
        button1.onClick.AddListener(ShowPage1);
        button2.onClick.AddListener(ShowPage2);
    }

    void ShowPage1()
    {
        page1.SetActive(true);
        page2.SetActive(false);
    }

    void ShowPage2()
    {
        page1.SetActive(false);
        page2.SetActive(true);
    }
}