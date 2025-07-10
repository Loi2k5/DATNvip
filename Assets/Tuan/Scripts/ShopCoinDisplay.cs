using UnityEngine;
using TMPro;

/// <summary>
/// Script gắn vào UI trong scene shop để hiển thị và thao tác xu.
/// </summary>
public class ShopCoinDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;

    void Start()
    {
        UpdateCoinUI();
    }

    /// <summary>Cập nhật text xu hiện tại</summary>
    public void UpdateCoinUI()
    {
        if (coinText != null && CoinData.Instance != null)
        {
            coinText.text = $"Xu: {CoinData.Instance.GetCoin()}";
        }
    }

    /// <summary>Gọi khi mua đồ</summary>
    public void BuyItem(int price)
    {
        if (CoinData.Instance != null && (price <= 0 || CoinData.Instance.GetCoin() >= price))
        {
            CoinData.Instance.SpendCoin(price);
            UpdateCoinUI();
            Debug.Log($"Đã mua vật phẩm giá {price} xu.");
        }
        else
        {
            Debug.Log("Không đủ xu để mua vật phẩm.");
        }
    }
}