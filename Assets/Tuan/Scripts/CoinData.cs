using UnityEngine;

/// <summary>
/// Quản lý xu xuyên suốt các scene, lấy dữ liệu ban đầu từ GameManager.
/// </summary>
public class CoinData : MonoBehaviour
{
    public static CoinData Instance;

    [Header("Số xu hiện có")]
    public int currentCoins = 0;


    void Awake()
    {
        // Singleton: chỉ giữ lại một bản
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Tìm GameManager trong scene chơi và lấy điểm ban đầu
            GameManager gm = FindObjectOfType<GameManager>();
            if (gm != null)
            {
                currentCoins = gm.GetCurrentPoints(); // gán xu từ điểm
                Debug.Log($"[CoinData] Xu khởi tạo từ GameManager: {currentCoins}");
            }
            else
            {
                Debug.LogWarning("[CoinData] Không tìm thấy GameManager.");
            }
        }
        else
        {
            Destroy(gameObject); // xóa bản thừa nếu đã tồn tại
        }
    }

    public void AddCoin(int amount)
    {
        currentCoins += amount;
    }

    public void SpendCoin(int amount)
    {
        currentCoins = Mathf.Max(0, currentCoins - amount);
    }

    public int GetCoin()
    {
        return currentCoins;
    }
}