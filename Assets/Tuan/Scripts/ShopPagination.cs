using UnityEngine;
using UnityEngine.UI;

public class ShopPagination : MonoBehaviour
{
    public GameObject[] pages; // Danh sách các trang
    public Button nextButton, prevButton;
    private int currentPage = 0;

    void Start()
    {
        UpdatePage();
        nextButton.onClick.AddListener(NextPage);
        prevButton.onClick.AddListener(PrevPage);
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

    void UpdatePage()
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(i == currentPage);
        }
    }
}
