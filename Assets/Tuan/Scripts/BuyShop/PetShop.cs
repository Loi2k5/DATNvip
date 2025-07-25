using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Script dùng cho mỗi nút mua PET trong scene Shop.
/// Kết nối với ShopCoinDisplay để trừ xu và cập nhật UI.
/// </summary>
public class PetShop : MonoBehaviour
{
    [Header("Thông tin PET")]
    [SerializeField] private int petPrice = 50;              // Giá PET
    [SerializeField] private string petID = "pet_wolf";      // Mã định danh PET

    [Header("Tham chiếu UI")]
    [SerializeField] private Button buyButton;               // Nút mua PET
    [SerializeField] private TextMeshProUGUI feedbackText;   // Text hiển thị kết quả
    [SerializeField] private ShopCoinDisplay coinUI;         // Script hiển thị và xử lý xu

    void Start()
    {
        if (buyButton != null)
            buyButton.onClick.AddListener(BuyPet);

        if (feedbackText != null)
        {
            feedbackText.text = "";
            feedbackText.gameObject.SetActive(false);
        }
    }

    void BuyPet()
    {
        if (feedbackText != null)
            feedbackText.gameObject.SetActive(true);

        if (coinUI != null && CoinData.Instance != null && (petPrice <= 0 || CoinData.Instance.GetCoin() >= petPrice))
        {
            coinUI.BuyItem(petPrice); // Trừ xu qua hệ thống hiển thị xu
            PlayerPrefs.SetInt(petID, 1); // Đánh dấu đã mua PET

            feedbackText.text = "🎉 Mua PET thành công!";
        }
        else
        {
            feedbackText.text = "❌ Không đủ xu hoặc không tìm thấy hệ thống xu!";
        }
    }
}